import ProjectRepository from "./ProjectRepository";
import { ProjectEditorModel } from "./ProjectModel";

export default class ProjectService {
  projectRepository;

  constructor() {
    this.projectRepository = new ProjectRepository();
  }

  async getProject(id: string, token: string) {
    return await this.projectRepository.getProject(id, token);
  }

  async createProject(project: ProjectEditorModel, token: string) {
    return await this.projectRepository.createProject(project, token);
  }

  async editProject(project: ProjectEditorModel, token: string) {
    return await this.projectRepository.updateProject(project, token);
  }

  async deleteProject(id: string, token: string) {
    return await this.projectRepository.deleteProject(id, token);
  }
}
