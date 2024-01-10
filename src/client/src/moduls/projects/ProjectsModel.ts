import ProjectsModel from "../common/ProjectModel";

export class GetProjectsResponseModel {
    constructor(
        public projects: ProjectsModel[],
        public allProjectsCount: number
    ){}
}
