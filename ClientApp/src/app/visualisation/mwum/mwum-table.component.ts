import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ElementRef, inject, Inject, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { MWUMTableDataSource } from '../../../models/mwumtabledatasource';
import { SimplexTableDataSource } from '../../../models/simplextabledatasource';
import { WebService } from '../../../services/WebService';



@Component({
  selector: 'app-mwum-table-component',
  templateUrl: './mwum-table.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./mwum-table.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0', visibility: 'hidden' })),
      state('expanded', style({ height: '*', visibility: 'visible' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class MWUMTableComponent implements OnInit
{


  @Input('data') public data!: MWUMTableDataSource;
  displayedColumns!: any;
  dataLoaded: boolean = false;

  isExpansionDetailRow = (i: number, row: Object) => row.hasOwnProperty('detailRow');
  expandedElement: any;

  ngOnInit() {
    this.displayedColumns = this.data.columnHeaders;
    this.displayedColumns.splice(0, 0, "Step");
    this.dataLoaded = true;
  }
}
