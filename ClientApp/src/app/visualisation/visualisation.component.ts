import { Component, ElementRef, inject, Inject, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';




@Component({
  selector: 'app-visualisation-component',
  templateUrl: './visualisation.component.html',
  encapsulation: ViewEncapsulation.None
})
export class VisualisationComponent implements OnInit {

  constructor(private route: ActivatedRoute) { }

  option: any;

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.option = params.get('option');
    });
  }

  }

