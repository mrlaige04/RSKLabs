import {GroupIteration} from "./GroupIteration";
import {Group} from "./result/Group";
import {Graph} from "./force-directed-graph/force-directed-graph.component";

export class ApiResult {

  constructor(public sets: Array<string>,
              public similarityMatrix: Array<Array<number>>,
              public groupIterations: Array<GroupIteration>,
              public clarifiedGroups: Array<Group>,
              public clarified: Array<Group>,
              public graphDatas: Array<Graph>
             ) {
  }
}
