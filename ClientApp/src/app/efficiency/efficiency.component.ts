
import { Component, ElementRef, inject, Inject, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { BaseResult } from '../../models/baseresult';
import { WebLinearProgram } from '../../models/weblinearprogram';
import { WebService } from '../../services/WebService';


@Component({
  selector: 'app-efficiency',
  templateUrl: './efficiency.component.html',
  styleUrls: ['./efficiency.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class EfficiencyComponent {


  X: number;
  Y: number;
  Accuracy: number;
  result: string;

  XYOptions = new Array(2, 3, 4, 5);
  AccuracyOptions = new Array(5, 10, 20, 50, 100);

  constructor(private webService: WebService) {
    this.X = 1;
    this.Y = 1;
    this.Accuracy = 20;
    this.result = "";
  }

  efficiencyRequest() {
    this.webService.post("efficiency/compareEfficiency", { X: this.X, Y: this.Y, Accuracy: this.Accuracy },
      (result: any) => {
        this.result = result;
      });
  }
}
