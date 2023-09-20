import { Component, ElementRef, inject, Inject, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { SimplexTableDataSource } from '../../../models/simplextabledatasource';
import { WebService } from '../../../services/WebService';



@Component({
  selector: 'app-simplex-table-component',
  templateUrl: './simplex-table.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls:['./simplex-table.component.css']
})
export class SimplexTableComponent implements OnInit {
  @Input('data') public data!: SimplexTableDataSource;
  displayedColumns!: any;

  ngOnInit() {
    this.displayedColumns = this.data.columnHeaders;
  }

  getHeaderClass(columnName: string) {
    if (this.data.TableData[0][columnName].isEnteringVariable) { return 'green'; } else {
      return 'header';
    }
  }

  getClass(isPivotRow: boolean, isEnteringVariable: boolean):string {

    if (isPivotRow && isEnteringVariable) return 'bold';
    if (isPivotRow) { return 'red' } else {
      if (isEnteringVariable) return 'green'
      return 'purple'
    }
   
  }
}
