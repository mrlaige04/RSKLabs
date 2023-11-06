import {AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import {ApiResult} from "../ApiResult";
import {Coord} from "./Coord";
import * as d3 from 'd3'
import {ISet} from "./Set";
import {Data} from "../force-directed-graph/force-directed-graph.component";


@Component({
  selector: 'app-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.scss']
})
export class ResultComponent implements OnInit, AfterViewInit{
  @Input() result : ApiResult | undefined;

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
