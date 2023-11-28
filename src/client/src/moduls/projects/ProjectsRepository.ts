import AxiosClient from "../../api/axios/AxiosClient";
import {PROJECT_MANAGEMENT_URL} from "../../env";
import {useAuth} from "react-oidc-context";
import ProjectModel, {GetProjectResponseModel, ProjectCreateModel} from "./ProjectModel";

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

    createProjects(projectCreateModel: ProjectCreateModel,token: string) {
        return this.apiClient.post<ProjectModel>({
            url: `/projects`,
            config: {
                headers: {Authorization: `Bearer ${token}`}
            },
            data: projectCreateModel
        });
    }
}