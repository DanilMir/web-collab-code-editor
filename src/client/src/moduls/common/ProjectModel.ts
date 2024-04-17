export default class ProjectsModel {
  constructor(
    public id: number,
    public title: string,
    public description: string,
    public visibility: Visibility,
    public programmingLanguage: string,
    public createdAt: Date,
  ) {}
}

export enum Visibility {
  Public,
  Private,
}
