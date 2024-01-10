import React, {useState} from "react";
import {CssBaseline, FormControl, Grid, InputLabel, Select, TextField} from "@mui/material";
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

export default function CreateProjectPage() {
    const {projectStore} = useRootStore()
    const auth = useAuth();

    const navigate = useNavigate();

    const [formData, setFormData] = useState<ProjectEditorModel>({
        title: '',
        description: "",
        visibility: 0,
        programmingLanguage: 'python'
    });


    if(auth.isLoading) {
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
    const handleSubmit = (e: any) => {
        e.preventDefault();
        projectStore.createProject(formData, auth.user?.access_token!);
        navigate("/projects");
    };

    return (
        <Container>
            <Box>
                <Grid container sx={{paddingTop: '20px'}}>
                    <Grid item xs={4} sx={{display: 'flex', flexDirection: 'column', gap: '10px', border: 1, justifyContent: "flex-start", paddingTop: "10px", paddingBottom: "10px"}}>
                        <Button variant="text">Create project from form</Button>
                        {/*<Button variant="text">Get project from github</Button>*/}
                    </Grid>
                    <Grid item xs={8} sx={{border: 1}}>
                        <ProjectEditorForm handleSubmit={handleSubmit} handleChange={handleChange} formData={formData} title="Create Project"/>
                    </Grid>
                </Grid>
            </Box>
        </Container>


    )
}
