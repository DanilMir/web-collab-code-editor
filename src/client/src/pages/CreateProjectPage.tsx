import React, {useState} from "react";
import {CssBaseline, FormControl, Grid, InputLabel, Select, Tab, Tabs, TextField} from "@mui/material";
import MenuItem from "@mui/material/MenuItem";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Container from "@mui/material/Container";
import Avatar from "@mui/material/Avatar";
import Typography from "@mui/material/Typography";
import CodeIcon from '@mui/icons-material/Code';
import {useRootStore} from "../hooks/useRootStore";
import {useAuth} from "react-oidc-context";
import {Navigate, useNavigate} from "react-router-dom";
import ProjectEditorForm from "../components/projects/ProjectEditorForm";
import LoginWarning from "../components/generic/LoginWarning";
import {red} from "@mui/material/colors";
import {ProjectEditorModel} from "../moduls/project/ProjectModel";
import TabList from "@mui/lab/TabList";
import TabContext from "@mui/lab/TabContext";
import Paper from "@mui/material/Paper";

interface TabPanelProps {
    children?: React.ReactNode;
    index: number;
    value: number;
}

function TabPanel(props: TabPanelProps) {
    const {children, value, index, ...other} = props;

    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            id={`vertical-tabpanel-${index}`}
            aria-labelledby={`vertical-tab-${index}`}
            {...other}
        >
            {value === index && (
                <Box sx={{p: 3}}>
                    <Typography>{children}</Typography>
                </Box>
            )}
        </div>
    );
}

function a11yProps(index: number) {
    return {
        id: `vertical-tab-${index}`,
        'aria-controls': `vertical-tabpanel-${index}`,
    };
}


export default function CreateProjectPage() {
    const {projectStore} = useRootStore()
    const auth = useAuth();

    const [value, setValue] = React.useState(0);

    const navigate = useNavigate();

    const [formData, setFormData] = useState<ProjectEditorModel>({
        title: '',
        description: "",
        visibility: 0,
        programmingLanguage: 'python'
    });


    if (auth.isLoading) {
        return null;
    }

    if (!auth.isAuthenticated) {
        return <LoginWarning/>;
    }

    const handleChange = (e: any) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };

    const handleChanges = (event: React.SyntheticEvent, newValue: number) => {
        setValue(newValue);
    };

    const handleSubmit = (e: any) => {
        e.preventDefault();
        projectStore.createProject(formData, auth.user?.access_token!);
        navigate("/projects");
    };

    return (
        <Container sx={{mt: 2}}>
            <Paper variant="elevation">
            <Box
                sx={{bgcolor: 'background.paper', display: 'flex', height: '60vh'}}
            >
                <Tabs
                    orientation="vertical"
                    variant="scrollable"
                    value={value}
                    onChange={handleChanges}
                    aria-label="Vertical tabs example"
                    sx={{borderRight: 1, borderColor: 'divider'}}
                >
                    <Tab label="Create project from form" {...a11yProps(0)} />
                </Tabs>
                <Container maxWidth="sm">
                    <TabPanel value={value} index={0}>
                        <ProjectEditorForm handleSubmit={handleSubmit} handleChange={handleChange} formData={formData}
                                           title="Create Project"/>
                    </TabPanel>
                </Container>
            </Box>
            </Paper>
        </Container>


    )
}
