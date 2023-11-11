import {useParams} from "react-router-dom";

type Params = {
    id: string
};

export default function Project() {
    const {id} = useParams<Params>();

    function accessibleProject(id: string) {
        return true;
    }

    if(typeof(id) === "string" && !accessibleProject(id)) {
        return (
            <> 403 Error </>
        )
        //todo: return 403 status code page
    }

    return (
        <>
            <h1>Project № {id}</h1>

            
        </>
    )
}