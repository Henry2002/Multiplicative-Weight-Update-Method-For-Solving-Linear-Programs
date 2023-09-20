import { Component, ElementRef, inject, Inject, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { SimplexTableDataSource } from '../../../models/simplextabledatasource';
import { WebService } from '../../../services/WebService';
import { SimplexTableComponent } from './simplex-table.component';



@Component({
  selector: 'app-simplex-steps-component',
  templateUrl: './simplex-steps.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class SimplexStepsComponent implements OnInit {

  dataSources: Array<SimplexTableDataSource> = [];


  constructor(private webService: WebService) {
  }

  ngOnInit() {
    this.webService.get("iterations/getalgorithmsteps/"+"simplex",
      (res: any) => {
        var displayedColumns = res.steps[0].columnHeaders;
        var i: number;

        for (i = 0; i < res.steps.length; i++) {

          this.dataSources[i] = new SimplexTableDataSource(res.steps[i].step, displayedColumns,

            res.steps[i].pivotRowIndex, res.steps[i].variableEnteringBasisIndex);
        }
      } );
  }


  

}
