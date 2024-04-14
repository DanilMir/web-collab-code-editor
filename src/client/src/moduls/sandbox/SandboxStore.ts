
import {action, makeAutoObservable} from "mobx"
import SandboxRepository from "./SandboxRepository";
import {SandboxModel} from "./SandboxModel";
export default class SandboxStore {
    sandboxRepository;
    isLoading: boolean = false;

    sandbox: SandboxModel;
    isActive: boolean = false;

    constructor() {
        this.sandboxRepository = new SandboxRepository();
        this.sandbox = new SandboxModel(0);

        makeAutoObservable(this, {
            setSandbox: action,
            setLoading: action
        });
    }


    setSandbox(sandbox: SandboxModel) {
        this.sandbox = sandbox;
    }

    setLoading(isLoading: boolean) {
        this.isLoading = isLoading
    }

    runProject(projectId: string, token: string) {
        this.setLoading(true);
        this.sandboxRepository.runProject (projectId, token)
            .then(result => {
                this.setSandbox(result.data)
                this.isActive = true;
            })
            .catch(error => {
                console.log(error);
            })
            .finally(() => {
                this.setLoading(false);
            })
    }

    deleteProject(projectId: string, token: string) {
        this.setLoading(true);
        this.sandboxRepository.deleteProject (projectId, token)
            .then(() => {
                this.isActive = false;
            })
            .catch(error => {
                console.log(error);
            })
            .finally(() => {
                this.setLoading(false);
            })
    }

    updateProject(projectId: string, token: string) {
        this.isActive = false;
        this.setLoading(true);
        this.sandboxRepository.updateProject(projectId, token)
            .then(result => {
                this.isActive = true;
                this.setSandbox(result.data)
            })
            .catch(error => {
                console.log(error);
            })
            .finally(() => {
                this.setLoading(false);
            })
    }
}
