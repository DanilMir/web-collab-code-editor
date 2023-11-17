import { Editor } from "@monaco-editor/react";

interface Props {
    height?: string | number | undefined,
    width?: string | number | undefined,
    language?: string | undefined,
    readOnly? : boolean | undefined
}

export default function CollaborativeEditor(props: Props) {
    return (
        <>
            <Editor
                height={props.height}
                width={props.width}
                language={props.language}
                defaultValue=""
                options={{ readOnly: props.readOnly }} //todo: readOnly: false, if only read access granted 
            />
        </>
    )
}