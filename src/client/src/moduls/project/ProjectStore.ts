import { ProjectEditorModel } from "./ProjectModel";
import ProjectService from "./ProjectService";
import { makeAutoObservable } from "mobx";

export default class ProjectStore {
  projectService;

  project;
  haveAccessToProject: boolean = false;
  isLoading = true;

  currentId: string = "";
  constructor() {
    makeAutoObservable(this);
    this.projectService = new ProjectService();
    this.project = new ProjectEditorModel("", "", 0, undefined);
  }

  setLoading(isLoading: boolean) {
    this.isLoading = isLoading;
  }

  setProject(project: ProjectEditorModel) {
    this.project = project;
  }

  getProject(id: string, token: string) {
    this.setLoading(true);
    this.projectService
      .getProject(id, token)
      .then((result) => {
        this.haveAccessToProject = true;
        this.setProject(result.data);
        this.currentId = id;
      })
      .catch((error) => {
        this.haveAccessToProject = false;
      })
      .finally(() => {
        this.setLoading(false);
      });
  }

  createProject(project: ProjectEditorModel, token: string) {
    this.projectService
      .createProject(project, token)
      .then(() => {})
      .catch((error) => {
        console.log(error);
      })
      .finally(() => {});
  }

  editProject(project: ProjectEditorModel, token: string) {
    this.projectService
      .editProject(project, token)
      .then()
      .catch((error) => {
        console.log(error);
      })
      .finally(() => {});
  }

  deleteProject(project: ProjectEditorModel, token: string) {
    this.projectService
      .createProject(project, token)
      .then()
      .catch((error) => {
        console.log(error);
      })
      .finally(() => {});
  }
}
