//Since we have to create a variable number of tables, with a varying number of entries and varying column headers
//It is probably best to create a custom data source with a dynamic 

import { CollectionViewer, DataSource } from "@angular/cdk/collections";
import { Observable } from "rxjs";
import { of } from 'rxjs';


interface SimplexTableElement {
  value: string;
  isEnteringVariable: boolean;
  isInPivotRow: boolean;
}

interface SimplexTableRow {

  [key: string]: SimplexTableElement; //Indexable type allows for multiple class attributes with different names
  
}

export class SimplexTableDataSource extends DataSource<SimplexTableRow> {

  TableData: SimplexTableRow[] = [];
  columnHeaders: string[];

  constructor(DataArray: string[][], ColumnHeaders: string[], pivotRow: number, enteringVariable: number){
    super();
    var i: number;
    var j: number;

    this.columnHeaders = ColumnHeaders;

    for (j = 0; j < DataArray[0].length; j++) {

      let model: SimplexTableRow = {};

      for (i = 0; i < DataArray.length; i++) {

        model[ColumnHeaders[i]] = {
          value:DataArray[i][j],
          isEnteringVariable:i-1 == enteringVariable ? true : false,
          isInPivotRow:j == pivotRow ? true : false,
        }

        this.TableData[j] = model;
      }

    }
    
  }
  

  connect(collections: CollectionViewer): Observable<SimplexTableRow[]> {
    return of(this.TableData)
  }

  disconnect() { }

}
