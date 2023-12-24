import React, {useEffect, useState} from "react";
import {ProjectEditorModel} from "../moduls/projects/ProjectModel";
import {useRootStore} from "../hooks/useRootStore";
import {useAuth} from "react-oidc-context";
import ProjectEditorForm from "../components/ProjectEditorForm";
import LoginWarning from "../components/LoginWarning";

export default function EditProjectPage() {
    const [formData, setFormData] = useState<ProjectEditorModel>({
        title: '',
        description: "",
        visibility: 0,
        programmingLanguage: 'python'
    });

    const {projectStore} = useRootStore()
    const auth = useAuth();




    useEffect(() => {

        }, []
    )

    if(auth.isLoading) {
        return null;
    }

    if (!auth.isAuthenticated) {
        return <LoginWarning/>;
    }

    const handleChange = (event : any) => {
        setFormData({
            ...formData,
            [event.target.name]: event.target.value,
        });
    };

    const handleSubmit = (e: any) => {
        e.preventDefault();
        projectStore.createProject(formData, auth.user?.access_token!);
        // navigate("/projects");
    };

    return (
        <>
            <ProjectEditorForm handleSubmit={handleSubmit} handleChange={handleChange} formData={formData} title="Edit project"/>
        </>
    )
}
