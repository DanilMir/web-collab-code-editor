import ProjectsStore from "./moduls/projects/ProjectsStore";
import React from "react";
import ProjectStore from "./moduls/project/ProjectStore";

class RootStore {
    projectsStore;
    projectStore;

    constructor() {
        this.projectsStore = new ProjectsStore();
        this.projectStore = new ProjectStore();
    }
}

export const rootStore =  new RootStore();


export const storesContext = React.createContext(rootStore);
