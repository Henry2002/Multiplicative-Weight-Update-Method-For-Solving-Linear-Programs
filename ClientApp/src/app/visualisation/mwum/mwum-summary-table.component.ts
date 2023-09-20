import { Component, ElementRef, inject, Inject, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { MWUMTableDataSource, MWUMTableRowSummaryDataSource } from '../../../models/mwumtabledatasource';
import { SimplexTableDataSource } from '../../../models/simplextabledatasource';
import { WebService } from '../../../services/WebService';



@Component({
  selector: 'app-mwum-summary-table-component',
  templateUrl: './mwum-summary-table.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./mwum-summary-table.component.css'],
})
export class MWUMSummaryTableComponent implements OnInit {

  @Input('data') public data!: MWUMTableRowSummaryDataSource;
  displayedColumns!: any;

  ngOnInit() {
    this.displayedColumns = this.data.columnHeaders;
    onpause;
  }
}
