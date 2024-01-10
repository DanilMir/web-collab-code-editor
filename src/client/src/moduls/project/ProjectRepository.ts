import {ProjectEditorModel} from "./ProjectModel";
import ProjectsModel from "../common/ProjectModel";
import AxiosClient from "../../api/axios/AxiosClient";
import {PROJECT_MANAGEMENT_URL} from "../../env";

export default class ProjectRepository {
    apiClient;

    constructor() {
        this.apiClient = new AxiosClient({
            baseURL: PROJECT_MANAGEMENT_URL
        });
    }

    createProject(projectCreateModel: ProjectEditorModel, token: string) {
        return this.apiClient.post<ProjectsModel>({
            url: `/projects`,
            config: {
                headers: {Authorization: `Bearer ${token}`}
            },
            data: projectCreateModel
        });
    }

    updateProject(projectEditModel: ProjectEditorModel, token: string) {
        return this.apiClient.put<ProjectsModel>({
            url: `/projects`,
            config: {
                headers: {Authorization: `Bearer ${token}`}
            },
            data: projectEditModel
        });
    }

    getProject(id: string, token: string) {
        return this.apiClient.get<ProjectsModel>({
            url: `/projects/${id}`,
            config: {
                headers: {Authorization: `Bearer ${token}`}
            }
        });
    }

    deleteProject(id: string, token: string) {
        return this.apiClient.delete<any>({
            url: `/projects/${id}`,
            config: {
                headers: {Authorization: `Bearer ${token}`}
            }
        });
    }
}
