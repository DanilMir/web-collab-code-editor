import { Editor } from "@monaco-editor/react";
import {useRef} from "react";
import * as Y from "yjs"
import {WebsocketProvider} from "y-websocket";
import {MonacoBinding} from "y-monaco";


interface Props {
    height?: string | number | undefined,
    width?: string | number | undefined,
    language?: string | undefined,
    readOnly? : boolean | undefined,
    room : string
}

export default function CollaborativeEditor(props: Props) {

    const editorRef = useRef(null)

    function handleEditorDidMount(editor: any, monaco : any) {
        editorRef.current = editor;

        const doc = new Y.Doc();

        const provider = new WebsocketProvider("ws:\\localhost:1234", props.room, doc);
        const type = doc.getText("monaco");

        // @ts-ignore
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        const binding = new MonacoBinding(type, editorRef.current.getModel(), new Set([editorRef.current]), provider.awareness);

        console.log(provider.awareness)
    }



    return (
        <>
            <Editor
                height={props.height}
                width={props.width}
                language={props.language}
                defaultValue=""
                options={{ readOnly: props.readOnly }} //TODO: readOnly: false, if only read access granted
                onMount={handleEditorDidMount}
            />
        </>
    )
}