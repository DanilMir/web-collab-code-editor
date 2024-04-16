import {Visibility} from "../common/ProjectModel";

export class ProjectEditorModel {
    constructor(
        public title: string,
        public description: string,
        public visibility: Visibility,
        public programmingLanguage? : string,
    ){}
}
