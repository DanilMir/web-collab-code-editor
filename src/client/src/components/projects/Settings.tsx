import React, { useEffect, useState } from "react";
import { ProjectEditorModel } from "../../moduls/project/ProjectModel";
import { useRootStore } from "../../hooks/useRootStore";
import { useAuth } from "react-oidc-context";
import LoginWarning from "../generic/LoginWarning";
import ProjectEditorForm from "./ProjectEditorForm";

export default function Settings() {
  const [formData, setFormData] = useState<ProjectEditorModel>({
    title: "",
    description: "",
    visibility: 0,
    programmingLanguage: "python",
  });

  const { projectStore } = useRootStore();
  const auth = useAuth();

  useEffect(() => {}, []);

  if (auth.isLoading) {
    return null;
  }

  if (!auth.isAuthenticated) {
    return <LoginWarning />;
  }

  const handleChange = (event: any) => {
    setFormData({
      ...formData,
      [event.target.name]: event.target.value,
    });
  };

  const handleSubmit = (e: any) => {
    e.preventDefault();
    projectStore.editProject(formData, auth.user?.access_token!);
  };

  return (
    <>
      <ProjectEditorForm
        handleSubmit={handleSubmit}
        handleChange={handleChange}
        formData={formData}
        title="Edit Project"
      />
    </>
  );
}
