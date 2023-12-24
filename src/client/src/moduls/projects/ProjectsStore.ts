import ProjectsService from "./ProjectsService";
import {action, makeAutoObservable} from "mobx"
import ProjectModel, {GetProjectResponseModel, ProjectEditorModel} from "./ProjectModel";

export default class ProjectsStore {
    projectService;
    isLoading = true;

    projectArray: ProjectModel[] = [];
    allProjectsCount: number = 0

    project: ProjectEditorModel = new ProjectEditorModel("", "", 0, "")



    constructor() {
        this.projectService = new ProjectsService();
        makeAutoObservable(this, {
            updateProjects: action,
            setLoading: action
        });
    }


    updateProjects(projectResponseModel: GetProjectResponseModel) {
        this.projectArray = projectResponseModel.projects;
        this.allProjectsCount = projectResponseModel.allProjectsCount;
    }

    setLoading(isLoading: boolean) {
        this.isLoading = isLoading
    }

    setProject(project: ProjectEditorModel) {
        this.project = project
    }

    getProject(id: string, token: string) {
        this.setLoading(true);
        this.projectService
            .getProject(id, token)
            .then(result => {
                this.setProject(result.data)
            })
            .catch(error => {
                console.log(error);
            })
            .finally(() => {
                this.setLoading(false);
            })
    }

    createProject(project: ProjectEditorModel, token: string) {
        this.projectService
            .createProject(project, token)
            .then(result => {
                console.log(result)
            })
            .catch(error => {
                console.log(error);
            })
            .finally(() => {
            })
    }

    getActiveProjects(offset: number, limit: number, token: string) {
        this.setLoading(true);
        this.projectService.getActiveProjects(offset, limit, token)
            .then(result => {
                this.updateProjects(result.data)
                console.log(result)
            })
            .catch(error => {
                console.log(error);
            })
            .finally(() => {
                this.setLoading(false);
            })
    }

    editProject(project: ProjectEditorModel, token: string) {
        this.projectService
            .editProject(project, token)
            .then(result => {
                console.log(result)
            })
            .catch(error => {
                console.log(error);
            })
            .finally(() => {
            })
    }

    deleteProject(project: ProjectEditorModel, token: string) {
        this.projectService
            .createProject(project, token)
            .then(result => {
                console.log(result)
            })
            .catch(error => {
                console.log(error);
            })
            .finally(() => {
            })
    }
}
