import { Editor } from "@monaco-editor/react";
import { useEffect, useMemo, useRef, useState } from "react";
import * as Y from "yjs";
import { WebsocketProvider } from "y-websocket";
import { MonacoBinding } from "y-monaco";
import "./CollaborativeEditor.css";
import { keys } from "mobx";
import { Doc } from "yjs";
import { useAuth } from "react-oidc-context";

interface Props {
  height?: string | number | undefined;
  width?: string | number | undefined;
  language?: string | undefined;
  readOnly?: boolean | undefined;
  room: string;
}

interface User {
  name: string;
  // Define a color that should be associated to the user:
  color: string;
}

export default function CollaborativeEditor(props: Props) {
  const editorRef = useRef(null);
  const [cssStyle, setCssStyle] = useState("");
  const [awarenessUsers, setAwarenessUsers] = useState<Map<number, User>>(
    new Map<number, User>(),
  );
  const auth = useAuth();

  // Insert awareness info into cursors with styles

  const styleSheet = useMemo(() => {
    let cursorStyles = "";
    awarenessUsers.forEach((user, clientId) => {
      if (user) {
        cursorStyles += `
          .yRemoteSelection-${clientId},
          .yRemoteSelectionHead-${clientId}  {
            --user-color: ${user.color || "orangered"};
          }

          .yRemoteSelectionHead-${clientId}::after {
            content: "${user.name}";
          }
        `;
      }
    });

    return { __html: cursorStyles };
  }, [awarenessUsers]);

  function handleEditorDidMount(editor: any, monaco: any) {
    editorRef.current = editor;
    const doc = new Y.Doc();
    const provider = new WebsocketProvider(
      "ws:\\localhost:1234",
      props.room,
      doc,
    );
    const type = doc.getText("monaco");
    const awareness = provider.awareness;

    function setUsers() {
      let users: Map<number, User> = new Map();
      provider.awareness.getStates().forEach((x, y) => {
        if (x["user"] != undefined) {
          users.set(y, { name: x["user"]["name"], color: x["user"]["color"] });
        }
      });

      setAwarenessUsers(users);
    }

    // // You can observe when a user updates their awareness information
    awareness.on("change", setUsers);

    awareness.setLocalStateField("user", {
      // Define a print name that should be displayed
      name: auth.user?.profile.email,
      // Define a color that should be associated to the user:
      color:
        "#" + (((1 << 24) * Math.random()) | 0).toString(16).padStart(6, "0"),
    });

    // @ts-ignore
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const binding = new MonacoBinding(
      type,
      editorRef.current.getModel(),
      new Set([editorRef.current]),
      awareness,
    );
  }

  return (
    <div>
      <style dangerouslySetInnerHTML={styleSheet} />
      <Editor
        height={props.height}
        width={props.width}
        language={props.language}
        defaultValue=""
        options={{ readOnly: props.readOnly }} //TODO: readOnly: false, if only read access granted
        onMount={handleEditorDidMount}
        theme="vs-dark"
      />
    </div>
  );
}
