import ProjectsService from "./ProjectsService";
import { action, makeAutoObservable } from "mobx";
import ProjectsModel from "../common/ProjectModel";
import { GetProjectsResponseModel } from "./ProjectsModel";

export default class ProjectsStore {
  projectsService;
  isLoading = true;

  projectArray: ProjectsModel[] = [];
  allProjectsCount: number = 0;
  constructor() {
    this.projectsService = new ProjectsService();
    makeAutoObservable(this, {
      updateProjects: action,
      setLoading: action,
    });
  }

  updateProjects(projectResponseModel: GetProjectsResponseModel) {
    this.projectArray = projectResponseModel.projects;
    this.allProjectsCount = projectResponseModel.allProjectsCount;
  }

  setLoading(isLoading: boolean) {
    this.isLoading = isLoading;
  }

  getActiveProjects(offset: number, limit: number, token: string) {
    this.setLoading(true);
    this.projectsService
      .getActiveProjects(offset, limit, token)
      .then((result) => {
        this.updateProjects(result.data);
      })
      .catch((error) => {
        console.log(error);
      })
      .finally(() => {
        this.setLoading(false);
      });
  }
}
