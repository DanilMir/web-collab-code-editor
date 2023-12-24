import ProjectsRepository from "./ProjectsRepository";
import ProjectModel, {ProjectEditorModel} from "./ProjectModel";

export default class ProjectsService {
    projectsRepository;

    constructor() {
        this.projectsRepository = new ProjectsRepository();
    }
    async getActiveProjects (offset: number, limit: number, token: string)
    {
        return await this.projectsRepository.getActiveProjects(offset, limit, token);
    }

    async getProject (id: string, token: string)
    {
        return await this.projectsRepository.getProject(id, token);
    }

    async createProject (project: ProjectEditorModel, token: string)
    {
        return await this.projectsRepository.createProject(project, token);
    }

    async editProject (project: ProjectEditorModel, token: string)
    {
        return await this.projectsRepository.updateProject(project, token);
    }

    async deleteProject (id: string, token: string)
    {
        return await this.projectsRepository.deleteProject(id, token);
    }
}
