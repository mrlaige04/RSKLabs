import {Component, Input, OnInit} from '@angular/core';
import {ApiResult} from "../ApiResult";
import {Coord} from "./Coord";
import {
  logBuilderStatusWarnings
} from "@angular-devkit/build-angular/src/builders/browser-esbuild/builder-status-warnings";
import {ISet} from "./Set";

@Component({
  selector: 'app-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.scss']
})
export class ResultComponent implements OnInit{
  @Input() result : ApiResult | undefined;

  currentIndex = 0;
  ngOnInit() {

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
