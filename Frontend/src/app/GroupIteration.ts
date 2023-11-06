import {Coord} from "./result/Coord";

export class GroupIteration {
  constructor(public matrix: Array<Array<number>>, public group: Array<number>, public maxItem: number, public maxItemIndexes: Array<Coord>) {
  }
}
