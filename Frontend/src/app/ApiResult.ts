import {GroupIteration} from "./GroupIteration";
import {Group} from "./result/Group";

export class ApiResult {

  constructor(public sets: Array<string>,
              public similarityMatrix: Array<Array<number>>,
              public groupIterations: Array<GroupIteration>,
              public clarifiedGroups: Array<Group>
             ) {
  }
}
