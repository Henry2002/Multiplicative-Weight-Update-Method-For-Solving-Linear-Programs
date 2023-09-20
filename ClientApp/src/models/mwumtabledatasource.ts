import { CollectionViewer, DataSource } from "@angular/cdk/collections";

import { Observable, of } from "rxjs";

export interface MWUMTableRow {
  step: number;
  guess: string;
  maxStep: string;
  solvedDual: string;
  isSatisfactory: string;
  rowSummary: MWUMTableRowSummaryDataSource;
}



export interface MWUMTableRowSumary {

  [key: string]: string;
}

export class MWUMTableDataSource extends DataSource<MWUMTableRow>{

  TableData: MWUMTableRow[] = [];
  columnHeaders: string[];

  constructor(DataArray: any[], columnHeaders: string[]) {
    super();
    this.columnHeaders = columnHeaders;
    var i: number;

    for (i = 0; i < DataArray.length; i++)
    {
      let model: MWUMTableRow = {
        guess: DataArray[i].guess,
        solvedDual: DataArray[i].solvedDual,
        isSatisfactory: DataArray[i].isSatisfactory,
        maxStep: DataArray[i].maxStep,
        rowSummary: new MWUMTableRowSummaryDataSource(DataArray[i].summaryTable, DataArray[i].summaryTableColumnHeaders),
        step: i+1
      };
      this.TableData[i] = model;
    }

   
  }

  connect(collections: CollectionViewer): Observable<MWUMTableRow[]> {
    const rows: any[] = [];
    this.TableData.forEach(element => rows.push(element, { detailRow: true, element }));
    return of(rows);
  }

  disconnect() { }


}

export class MWUMTableRowSummaryDataSource extends DataSource<MWUMTableRowSumary>
{
  TableData: MWUMTableRowSumary[] = [];
  columnHeaders: string[];

  constructor(DataArray: string[][], columnHeaders: string[]) {
    super();
    this.columnHeaders = columnHeaders;

    var j: number;
    var i: number;

    for (j = 0; j < DataArray[0].length; j++) {

      let model: MWUMTableRowSumary = {};

      for (i = 0; i < DataArray.length; i++) {

        model[columnHeaders[i]] = DataArray[i][j];

        this.TableData[j] = model;
      }

    }
  }



  connect(collections: CollectionViewer): Observable<MWUMTableRowSumary[]> {
    return of(this.TableData)
  }

  disconnect() { }
}
