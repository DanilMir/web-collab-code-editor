import ProjectsRepository from "./ProjectsRepository";

export default class ProjectsService {
  projectsRepository;

  constructor() {
    this.projectsRepository = new ProjectsRepository();
  }
  async getActiveProjects(offset: number, limit: number, token: string) {
    return await this.projectsRepository.getActiveProjects(
      offset,
      limit,
      token,
    );
  }
}
