import React, {useState} from "react";
import {CssBaseline, FormControl, InputLabel, Select, TextField} from "@mui/material";
import MenuItem from "@mui/material/MenuItem";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Container from "@mui/material/Container";
import Avatar from "@mui/material/Avatar";
import Typography from "@mui/material/Typography";
import CodeIcon from '@mui/icons-material/Code';
import {useRootStore} from "../hooks/useRootStore";
import {useAuth} from "react-oidc-context";
import {ProjectEditorModel} from "../moduls/projects/ProjectModel";
import {Navigate, useNavigate} from "react-router-dom";
import ProjectEditorForm from "../components/ProjectEditorForm";
import LoginWarning from "../components/LoginWarning";

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
        <ProjectEditorForm handleSubmit={handleSubmit} handleChange={handleChange} formData={formData} title="Create project"/>
    )
}
