import {useParams} from "react-router-dom";
import CollaborativeEditor from "../components/editor/CollaborativeEditor";
import {ButtonGroup, Grid} from "@mui/material";
import Button from "@mui/material/Button";
import Box from "@mui/material/Box";
import {useEffect, useState} from "react";
import Terminal from "../components/remote-run/Terminal";
import AIChat from "../components/ai-chat/AIChat";
import Settings from "../components/projects/Settings";
import {useRootStore} from "../hooks/useRootStore";
import {useAuth} from "react-oidc-context";
import LoginWarning from "../components/generic/LoginWarning";
import {observer} from "mobx-react";
import Typography from "@mui/material/Typography";
import Loading from "../components/generic/Loading";
import Forbidden from "./errors/Forbidden";

type Params = {
    id: string
};

export const ProjectPage = observer(() => {
        const {id} = useParams<Params>();
        const [menu, setMenu] = useState<number>(0);
        const {projectStore} = useRootStore();
        const auth = useAuth();

        useEffect(() => {
            projectStore.getProject(id!, auth.user?.access_token!)
        }, [id, auth.user?.access_token]);


        if (auth.isLoading) {
            return <Loading/>
        }

        if (!auth.isAuthenticated) {
            return <LoginWarning/>;
        }

        if(projectStore.isLoading) {
            return <Loading/>
        }

        // if (!projectStore.haveAccessToProject) {
        //     return <Forbidden/>
        // }


        let content;

        switch (menu) {
            case 0:
                content = <Terminal/>;
                break;
            case 1:
                content = <AIChat/>;
                break;
            case 2:
                content = <Settings/>;
                break;
            default:
                content = <div>Неизвестный тип</div>;
        }

        return (
            <>
                <Grid container>

                    <Grid item xs={6}>
                        <CollaborativeEditor
                            height="100vh"
                            width="100wh"
                            language="javascript"
                            readOnly={false}
                            room={id!.toString()}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <ButtonGroup>
                            <Button
                                onClick={() => setMenu(0)}
                                variant={menu == 0 ? "contained" : "outlined"}
                            >
                                Terminal
                            </Button>
                            <Button
                                onClick={() => setMenu(1)}
                                variant={menu == 1 ? "contained" : "outlined"}
                            >
                                AI Chat
                            </Button>
                            {/*<Button*/}
                            {/*    onClick={() => setMenu(2)}*/}
                            {/*    variant={menu == 2 ? "contained" : "outlined"}*/}
                            {/*>Settings</Button>*/}
                        </ButtonGroup>
                        <Box>
                            {content}
                        </Box>
                    </Grid>
                </Grid>

            </>

        )
    }
);
