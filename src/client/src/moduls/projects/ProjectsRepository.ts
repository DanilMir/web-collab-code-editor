import AxiosClient from "../../api/axios/AxiosClient";
import {PROJECT_MANAGEMENT_URL} from "../../env";
import {useAuth} from "react-oidc-context";
import ProjectModel, {GetProjectResponseModel, ProjectEditorModel} from "./ProjectModel";

export default class ProjectsRepository {
    apiClient;

    constructor() {
        this.apiClient = new AxiosClient({
            baseURL: PROJECT_MANAGEMENT_URL
        });
    }

    getActiveProjects(offset: number, limit: number, token: string) {
        return this.apiClient.get<GetProjectResponseModel>({
            url: `/projects`,
            config: {
                params: {
                    pending: false,
                    offset: offset,
                    limit: limit,
                },
                headers: {Authorization: `Bearer ${token}`}
            }
        });
    }

    createProject(projectCreateModel: ProjectEditorModel, token: string) {
        return this.apiClient.post<ProjectModel>({
            url: `/projects`,
            config: {
                headers: {Authorization: `Bearer ${token}`}
            },
            data: projectCreateModel
        });
    }

    updateProject(projectEditModel: ProjectEditorModel, token: string) {
        return this.apiClient.put<ProjectModel>({
            url: `/projects`,
            config: {
                headers: {Authorization: `Bearer ${token}`}
            },
            data: projectEditModel
        });
    }

    getProject(id: string, token: string) {
        return this.apiClient.get<ProjectModel>({
            url: `/projects/${id}`,
            config: {
                headers: {Authorization: `Bearer ${token}`}
            }
        });
    }

    deleteProject(id: string,token: string) {
        return this.apiClient.delete<any>({
            url: `/projects/${id}`,
            config: {
                headers: {Authorization: `Bearer ${token}`}
            }
        });
    }
}
