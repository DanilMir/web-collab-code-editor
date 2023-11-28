import ProjectsRepository from "./ProjectsRepository";
import ProjectModel, {ProjectCreateModel} from "./ProjectModel";

export default class ProjectsService {
    projectsRepository;

    constructor() {
        this.projectsRepository = new ProjectsRepository();
    }
    async getActiveProjects (offset: number, limit: number, token: string)
    {
        return await this.projectsRepository.getActiveProjects(offset, limit, token);
    }

    async createProjects (project: ProjectCreateModel, token: string)
    {
        return await this.projectsRepository.createProjects(project, token);
    }
}