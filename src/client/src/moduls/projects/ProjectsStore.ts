import ProjectsService from "./ProjectsService";
import {action, makeAutoObservable, toJS} from "mobx"
import ProjectModel, {GetProjectResponseModel, ProjectCreateModel} from "./ProjectModel";

export default class ProjectsStore {
    projectService;
    isLoading = true;

    projectArray: ProjectModel[] = [];
    allProjectsCount: number = 0

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

    getActiveProjects(offset: number, limit: number, token: string) {
        this.setLoading(true);
        this.projectService
            .getActiveProjects(offset, limit, token)
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

    createProject(project: ProjectCreateModel, token: string) {
        this.projectService
            .createProjects(project, token)
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