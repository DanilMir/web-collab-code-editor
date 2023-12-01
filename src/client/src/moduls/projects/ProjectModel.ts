export default class ProjectModel {
    constructor(
        public id: number,
        public title: string,
        public description: string,
        public visibility: Visibility,
        public programmingLanguage : string,
        public createdAt: Date
    ){}
}

enum Visibility {
    Public,
    Private
}

export class ProjectCreateModel {
    constructor(
        public title: string,
        public description: string,
        public visibility: Visibility,
        public programmingLanguage : string,
    ){}
}

export class GetProjectResponseModel {
    constructor(
        public projects: ProjectModel[],
        public allProjectsCount: number
    ){}
}