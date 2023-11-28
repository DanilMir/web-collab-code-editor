import ProjectsStore from "./moduls/projects/ProjectsStore";
import React from "react";

class RootStore {
    projectStore;

    constructor() {
        this.projectStore = new ProjectsStore();
    }
}

export const rootStore =  new RootStore();


export const storesContext = React.createContext(rootStore);