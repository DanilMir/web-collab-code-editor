import {useParams} from "react-router-dom";
import CollaborativeEditor from "../components/CollaborativeEditor";

type Params = {
    id: string
};

export default function ProjectPage() {
    const {id} = useParams<Params>();

    function accessibleProject(id: string) {
        return true;
    }

    if(typeof(id) === "string" && !accessibleProject(id)) {
        return (
            <> 403 Error </>
        )
        //TODO: return 403 status code page
    }

    return (
        <>
            <CollaborativeEditor
                height="100vh"
                width="100wh"
                language="javascript"
                readOnly={false}
                room={id!.toString()}
            />
        </>
    )
}
