
import { Component, ElementRef, inject, Inject, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { BaseResult } from '../../models/baseresult';
import { WebLinearProgram } from '../../models/weblinearprogram';
import { WebService } from '../../services/WebService';


@Component({
  selector: 'app-solver-component',
  templateUrl: './solver.component.html',
  styleUrls: ['./solver.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class SolverComponent{

  selectedAlgorithm: string;

  @ViewChild('textarea') textarea!: ElementRef;
  output: any;
  htmlString: string;
  algorithms: Array<string>;
  epsilonOptions: Array<number>;
  algorithm: string;
  selectedEpsilon: number;
  linearProgram: string;
  isSuccess: boolean;
  comparisonMode: boolean;
  buttonText: string;


  constructor(private webService: WebService, private router: Router) {
    this.output = "";
    this.htmlString = "<span></span>";
    this.algorithm = "";
    this.linearProgram = "";
    this.selectedAlgorithm = "";
    this.algorithms = new Array("Simplex", "MWUM");
    this.epsilonOptions = new Array(0.5, 0.1, 0.01, 0.005);
    this.selectedEpsilon = 0.01;
    this.isSuccess = false;
    this.comparisonMode = false;
    this.buttonText = "Solve";
  }


  onKey(textarea: HTMLTextAreaElement) {
      this.htmlString = new Array(textarea.value.split('\n').length)
      .fill('<span></span>')
      .join("")
  }
 
  changeButtonText() {
    if (this.buttonText == "Solve") {
      this.buttonText = "Compare";
    } else {
      this.buttonText = "Solve";
    }
  }
  
  solveLinearProgram() {
    if (!this.comparisonMode) {
      this.webService.post("solver/solveLinearProgram", {
        selectedAlgorithm: this.selectedAlgorithm, linearProgram: this.linearProgram,
        selectedEpsilon: this.selectedEpsilon
      },
        (result: any, isSuccess: boolean) => {
          this.output = result;
          this.isSuccess = isSuccess;
        })


    } else {
      this.webService.post("solver/compareAlgorithms", {linearProgram: this.linearProgram },
        (result: any, isSuccess: boolean) => {
          this.output = result;
          this.isSuccess = isSuccess;
          if (isSuccess) {
            this.router.navigate(["/visualisation/", "Comparison"])
          }
        })
    }
  }

  goToSteps() {
    this.router.navigate(["/visualisation/", this.selectedAlgorithm])
  }

  
 
}
