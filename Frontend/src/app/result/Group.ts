import {ISet} from "./Set";

export class Group {
  constructor(public code: number, public sets: Array<ISet>, public operations: Array<string>) {
  }
}
