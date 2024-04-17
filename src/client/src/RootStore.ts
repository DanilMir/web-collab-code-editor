import ProjectsStore from "./moduls/projects/ProjectsStore";
import React from "react";
import ProjectStore from "./moduls/project/ProjectStore";
import SandboxStore from "./moduls/sandbox/SandboxStore";

class RootStore {
  projectsStore;
  projectStore;
  sandboxStore;

  constructor() {
    this.projectsStore = new ProjectsStore();
    this.projectStore = new ProjectStore();
    this.sandboxStore = new SandboxStore();
  }
}

export const rootStore = new RootStore();

export const storesContext = React.createContext(rootStore);
