import { Visibility } from "../common/ProjectModel";

export class SandboxModel {
  constructor(public port: number, public containerName: string) {}
}
