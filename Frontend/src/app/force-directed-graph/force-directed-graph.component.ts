import {AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import * as d3 from 'd3';
import { SimulationNodeDatum} from "d3";

@Component({
  selector: 'app-force-directed-graph',
  templateUrl: './force-directed-graph.component.html',
  styleUrls: ['./force-directed-graph.component.scss']
})
export class ForceDirectedGraphComponent implements AfterViewInit {
  @Input() data!: Graph;
  @ViewChild("container") container!: ElementRef;

  constructor() {
  }
  ngAfterViewInit() {
    const width = 600;
    const height = 600;

    const svg = d3.select(this.container.nativeElement)
      .append('svg')
      .attr('width', width)
      .attr('height', height);

    const simulation = d3.forceSimulation(<SimulationNodeDatum[]>this.data.nodes)
      .force('link', d3.forceLink(this.data.links).id((d: any) => d.id).distance(60))
      .force('charge', d3.forceManyBody().strength(-200))
      .force('center', d3.forceCenter(width / 2, height / 2))
      .force('collision', d3.forceCollide(70));

    const marker = svg.append('defs')
      .selectAll('marker')
      .data(['end'])
      .enter()
      .append('marker')
      .attr('id', (d: any) => d)
      .attr('viewBox', '0 -5 10 10')
      .attr('refX', 15)
      .attr('markerWidth', 4)
      .attr('markerHeight', 4)
      .attr('orient', 'auto')
      .append('path')
      .attr('d', 'M0,-5L10,0L0,5')
      .attr('class', 'arrowHead');

    const link = svg.selectAll('.link')
      .data(this.data.links)
      .enter()
      .append('line')
      .attr('class', 'link')
      .attr('marker-end', (d: any) => `url(#end${d.direction === 'left' ? 'Reverse' : ''})`)
      .style('stroke', 'gray')
      .style('stroke-width', 3);


    const node = svg.selectAll('.node')
      .data(this.data.nodes)
      .enter()
      .append('g')
      .attr('class', 'node');

    node.append('circle')
      .attr('r', 10)
      .attr('fill', 'skyblue');

    node.append('text')
      .text((d: any) => d.label)
      .attr('text-anchor', 'middle')
      .attr('dy', 4);

    simulation.on('tick', () => {
      link.attr('x1', (d: any) => d.source.x)
        .attr('y1', (d: any) => d.source.y)
        .attr('x2', (d: any) => d.target.x)
        .attr('y2', (d: any) => d.target.y);

      node.attr('transform', (d: any) => `translate(${d.x},${d.y})`);
    });
  }
}

export interface Graph {
  groupNumber: number;
  nodes: Array<GraphNode>;
  links: Array<GraphLink>;
}

export interface GraphNode {
  id: string;
  label: string;
}

export interface GraphLink {
  source: string;
  target: string;
}
