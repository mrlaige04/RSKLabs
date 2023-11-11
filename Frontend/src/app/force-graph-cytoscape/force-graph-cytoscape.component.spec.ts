import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForceGraphCytoscapeComponent } from './force-graph-cytoscape.component';

describe('ForceGraphCytoscapeComponent', () => {
  let component: ForceGraphCytoscapeComponent;
  let fixture: ComponentFixture<ForceGraphCytoscapeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ForceGraphCytoscapeComponent]
    });
    fixture = TestBed.createComponent(ForceGraphCytoscapeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
