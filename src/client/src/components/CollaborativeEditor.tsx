import {Editor} from "@monaco-editor/react";
import {useRef} from "react";
import * as Y from "yjs"
import {WebsocketProvider} from "y-websocket";
import {MonacoBinding} from "y-monaco";


interface Props {
    height?: string | number | undefined,
    width?: string | number | undefined,
    language?: string | undefined,
    readOnly?: boolean | undefined,
    room: string
}

export default function CollaborativeEditor(props: Props) {

    const editorRef = useRef(null)

    function handleEditorDidMount(editor: any, monaco: any) {
        editorRef.current = editor;

        const doc = new Y.Doc();

        const provider = new WebsocketProvider("ws:\\localhost:1234", props.room, doc);
        const type = doc.getText("monaco");


        const awareness = provider.awareness

        // You can observe when a user updates their awareness information
        awareness.on('change', (changes: any) => {
            // Whenever somebody updates their awareness information,
            // we log all awareness information from all users.
            console.log(Array.from(awareness.getStates().values()))
        })

        // You can think of your own awareness information as a key-value store.
        // We update our "user" field to propagate relevant user information.
        awareness.setLocalStateField('user', {
            // Define a print name that should be displayed
            name: "#" + ((1 << 24) * Math.random() | 0).toString(16).padStart(6, "0"),
            // Define a color that should be associated to the user:
            color: "#" + ((1 << 24) * Math.random() | 0).toString(16).padStart(6, "0")
        })


        // @ts-ignore
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        const binding = new MonacoBinding(type, editorRef.current.getModel(), new Set([editorRef.current]), awareness);

        // All of our network providers implement the awareness crdt

    }


    return (
        <>
            <Editor
                height={props.height}
                width={props.width}
                language={props.language}
                defaultValue=""
                options={{readOnly: props.readOnly}} //TODO: readOnly: false, if only read access granted
                onMount={handleEditorDidMount}
                theme="vs-dark"
            />
        </>
    )
}
