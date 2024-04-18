import AxiosClient from "../../api/axios/AxiosClient";
import { PROJECT_MANAGEMENT_URL } from "../../env";
import { useAuth } from "react-oidc-context";
import { GetProjectsResponseModel } from "./ProjectsModel";

export default class ProjectsRepository {
  apiClient;

  constructor() {
    this.apiClient = new AxiosClient({
      baseURL: PROJECT_MANAGEMENT_URL,
    });
  }

  getActiveProjects(offset: number, limit: number, token: string) {
    return this.apiClient.get<GetProjectsResponseModel>({
      url: `/projects`,
      config: {
        params: {
          pending: false,
          offset: offset,
          limit: limit,
        },
        headers: { Authorization: `Bearer ${token}` },
      },
    });
  }
}
