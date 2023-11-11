import {AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import {ApiResult} from "../ApiResult";
import {Coord} from "./Coord";
import {ISet} from "./Set";
import {Graph} from "../force-directed-graph/force-directed-graph.component";


@Component({
  selector: 'app-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.scss']
})
export class ResultComponent implements OnInit, AfterViewInit{
  @Input() result : ApiResult | undefined;

  // TODO: REMOVE
  test2: Graph = {groupNumber:0,nodes:[{id:"\u04242",label:"\u04242"},{id:"\u04224",label:"\u04224"},{id:"\u04225",label:"\u04225"},{id:"\u04212",label:"\u04212"},{id:"\u04201",label:"\u04201"},{id:"\u04211",label:"\u04211"},{id:"\u04202 \u04241",label:"\u04202 \u04241"},{id:"\u04221 \u04223 \u04213",label:"\u04221 \u04223 \u04213"}],links:[{source:"\u04221 \u04223 \u04213",target:"\u04242"},{source:"\u04242",target:"\u04224"},{source:"\u04224",target:"\u04225"},{source:"\u04221 \u04223 \u04213",target:"\u04212"},{source:"\u04212",target:"\u04224"},{source:"\u04224",target:"\u04202 \u04241"},{source:"\u04221 \u04223 \u04213",target:"\u04242"},{source:"\u04242",target:"\u04201"},{source:"\u04201",target:"\u04224"},{source:"\u04221 \u04223 \u04213",target:"\u04211"},{source:"\u04211",target:"\u04212"},{source:"\u04212",target:"\u04202 \u04241"},{source:"\u04224",target:"\u04202 \u04241"}]}

  @ViewChild('test') test!: ElementRef;

  currentIndex = 0;
  ngOnInit() {

  }

  ngAfterViewInit() {

  }

  getCodesFromSets(sets: Array<ISet>) {
    return sets.map(s=>s.code).join(', ')
  }

  nextSlide() {
    if (this.currentIndex < this.result!.groupIterations.length - 1) {
      this.currentIndex++;
    }
  }

  prevSlide() {
    if (this.currentIndex > 0) {
      this.currentIndex--;
    }
  }

  hasCoord(maxItemIndexes: Array<Coord>, row: number, col: number): boolean {
    return maxItemIndexes.find(c=> {
      return c.col == col && c.row == row
    }) != undefined
  }

  protected readonly Coord = Coord;
}
