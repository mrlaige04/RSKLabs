import {AfterViewInit, Component, ElementRef, Input, ViewChild,} from '@angular/core';
import {Graph, GraphLink, GraphNode} from "../force-directed-graph/force-directed-graph.component";
import ForceGraph from "force-graph";


@Component({
  selector: 'app-force-graph-cytoscape',
  templateUrl: './force-graph-cytoscape.component.html',
  styleUrls: ['./force-graph-cytoscape.component.scss']
})
export class ForceGraphCytoscapeComponent implements AfterViewInit{
  @Input() graph!: Graph;
  @ViewChild('graph') cont!: ElementRef;
  ngAfterViewInit() {
    const myGraph = ForceGraph();
    this.cont.nativeElement.style.maxWidth = 'min-content';

    myGraph(this.cont.nativeElement)
      .graphData(this.graph)
      .width(700)
      .height(500)
      .zoom(3)
      .linkDirectionalArrowLength(6)
      .linkCanvasObjectMode(()=>'after')
      .linkDirectionalArrowRelPos(9)
      .nodeLabel('id')
      .nodeCanvasObject((node, ctx, globalScale) => {
        const radius = 5;

        ctx.beginPath();
        ctx.arc(node.x!, node.y!, radius, 0, 2 * Math.PI);
        ctx.fillStyle = 'lightblue';
        ctx.fill();

        ctx.fillStyle = 'black';
        ctx.font = '8px Arial';
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        ctx.fillText(<string>node.id, node.x!, node.y!);
      })
      .nodePointerAreaPaint((node, color, ctx) => {
        const label = <string>node.id;

        ctx.font = `${15}px Sans-Serif`;
        const textWidth = ctx.measureText(label).width;
        const bckgDimensions = [textWidth, 15].map(n => n + 15 * 0.2) as [number,number]; // some padding

        ctx.fillStyle = color;
        ctx.fillRect(node.x! - bckgDimensions[0] / 2, node.y! - bckgDimensions[1] / 2, ...bckgDimensions);
      })
      .nodeAutoColorBy('group')
      .onNodeDragEnd(node=>{
        node.fx = node.x;
        node.fy = node.y;
      });
  }
}
