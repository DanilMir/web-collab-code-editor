import AxiosClient from "../../api/axios/AxiosClient";
import { PROJECT_MANAGEMENT_URL, SANDBOX_URL } from "../../env";
import { GetProjectsResponseModel } from "../projects/ProjectsModel";
import { SandboxModel } from "./SandboxModel";

export default class SandboxRepository {
  apiClient;

  constructor() {
    this.apiClient = new AxiosClient({
      baseURL: SANDBOX_URL,
    });
  }

  runProject(projectId: string, token: string) {
    return this.apiClient.post<SandboxModel>({
      url: `/containers`,
      config: {
        params: {
          projectId: projectId,
        },
        headers: { Authorization: `Bearer ${token}` },
      },
    });
  }

  deleteProject(projectId: string, token: string) {
    return this.apiClient.delete<SandboxModel>({
      url: `/containers`,
      config: {
        params: {
          projectId: projectId,
        },
        headers: { Authorization: `Bearer ${token}` },
      },
    });
  }

  updateProject(projectId: string, token: string) {
    return this.apiClient.put<SandboxModel>({
      url: `/containers`,
      config: {
        params: {
          projectId: projectId,
        },
        headers: { Authorization: `Bearer ${token}` },
      },
    });
  }
}
