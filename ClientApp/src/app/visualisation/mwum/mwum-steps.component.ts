import { Component, ElementRef, inject, Inject, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { MWUMTableDataSource } from '../../../models/mwumtabledatasource';
import { SimplexTableDataSource } from '../../../models/simplextabledatasource';
import { WebService } from '../../../services/WebService';



@Component({
  selector: 'app-mwum-steps-component',
  templateUrl: './mwum-steps.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./mwum-steps.component.css']
})
export class MWUMStepsComponent implements OnInit {

  dataSource!: MWUMTableDataSource;
  lowerBound!: string;
  upperBound!: string;
  dataLoaded: boolean=false;


  constructor(private webService: WebService) {

  }

  ngOnInit() {
    this.webService.get("iterations/getAlgorithmSteps/MWUM",
      (res: any) => {
        this.lowerBound = res.lowerBound;
        this.upperBound = res.upperBound;
        var displayedColumns = res.steps[0].columnHeaders;
        this.dataSource = new MWUMTableDataSource(res.steps, displayedColumns);
        this.dataLoaded = true;
      });
    
  }
}
