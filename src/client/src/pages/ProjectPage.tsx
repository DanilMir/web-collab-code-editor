import {useParams} from "react-router-dom";
import CollaborativeEditor from "../components/editor/CollaborativeEditor";
import {CircularProgress, Grid, SvgIcon, Tab} from "@mui/material";
import Box from "@mui/material/Box";
import TabContext from '@mui/lab/TabContext';
import TabList from '@mui/lab/TabList';
import TabPanel from '@mui/lab/TabPanel';
import {useEffect, useState} from "react";
import Terminal from "../components/remote-run/Terminal";
import {useRootStore} from "../hooks/useRootStore";
import {useAuth} from "react-oidc-context";
import LoginWarning from "../components/generic/LoginWarning";
import {observer} from "mobx-react";
import Loading from "../components/generic/Loading";
import BrowserIcon from "../components/icons/BrowserIcon"
import Typography from "@mui/material/Typography";

type Params = {
    id: string
};

export const ProjectPage = observer(() => {
        const {id} = useParams<Params>();
        const [tab, setTab] = useState<string>('1');
        const {projectStore, sandboxStore} = useRootStore();
        const auth = useAuth();


        const handleChange = (event: React.SyntheticEvent, newValue: string) => {
            setTab(newValue);
        };

        useEffect(() => {
            projectStore.getProject(id!, auth.user?.access_token!)
        }, [id, auth.user?.access_token]);


        if (auth.isLoading) {
            return <Loading/>
        }

        if (!auth.isAuthenticated) {
            return <LoginWarning/>;
        }

        if (projectStore.isLoading) {
            return <Loading/>
        }

        // if (!projectStore.haveAccessToProject) {
        //     return <Forbidden/>
        // }

        function lang(language: string) {
            switch(language) {
                case "csharp":
                    return "csharp";
                default:
                    return "plaintext";
            }
        }

        let room = `${id}::${GetRoomFile(projectStore.project.programmingLanguage)}`;

        console.log(room);

        return (
            <>
                <Grid container>

                    <Grid item xs={6}>
                        <CollaborativeEditor
                            height="calc(100vh - 68.5px)"
                            width="100%"
                            language={lang(projectStore.project.programmingLanguage)}
                            readOnly={false}
                            room={room}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <TabContext value={tab}>
                            <Box sx={{borderBottom: 1, borderColor: 'divider'}}>
                                <TabList variant="fullWidth" onChange={handleChange} aria-label="lab API tabs example">
                                    <Tab label="Sandbox" value="1"/>
                                </TabList>
                            </Box>
                            <TabPanel value="1">
                                <Box sx={{
                                    display: "flex",
                                    alignItems: "center",
                                    justifyContent: "center",
                                    flexDirection: 'column'
                                }}>
                                    {
                                        sandboxStore.isLoading ?
                                            <Box sx={{
                                                display: "flex",
                                                height: '75vh',
                                                alignItems: "center",
                                                justifyContent: "center",
                                                flexDirection: 'column'
                                            }}>
                                                <svg width={0} height={0}>
                                                    <defs>
                                                        <linearGradient id="my_gradient" x1="0%" y1="0%" x2="0%" y2="100%">
                                                            <stop offset="0%" stopColor="#e11cd5"/>
                                                            <stop offset="100%" stopColor="#2CB5E0"/>
                                                        </linearGradient>
                                                    </defs>
                                                </svg>
                                                <CircularProgress sx={{'svg circle': {stroke: 'url(#my_gradient)'}}}/>
                                            </Box>
                                            :
                                            <Box>
                                                {
                                                    sandboxStore.isActive ?
                                                        <Terminal port={sandboxStore.sandbox.port}
                                                                  password={"vncpassword"}/>
                                                        :
                                                        <Box sx={{
                                                            display: "flex",
                                                            alignItems: "center",
                                                            justifyContent: "center",
                                                            flexDirection: 'column'
                                                        }}>
                                                            <SvgIcon sx={{width: "500px", height: "500px"}}>
                                                                <BrowserIcon/>
                                                            </SvgIcon>
                                                            <Typography variant="h4" gutterBottom>To build the code
                                                                press <b>RUN</b></Typography>
                                                        </Box>
                                                }
                                            </Box>
                                    }
                                </Box>
                            </TabPanel>
                        </TabContext>
                    </Grid>
                </Grid>

            </>

        )
    }
);

function GetRoomFile(projectType: string) {
    switch (projectType) {
        case "csharp":
            return "Program.cs";
    }
}