using Microsoft.VisualBasic;
using System.Collections.Generic;
using TYP.Angular.Core.Algorithms;
using TYP.Angular.Core.Models.Simplex;

namespace TYP.Angular.Core.Models.Web
{
    public class WebSimplexSteps : WebAlgorithmSteps
    {
        public List<WebSimplexStep> Steps { get; set; }

        public WebSimplexSteps(SimplexTableauIterations Iterations)
        {
            Steps=new List<WebSimplexStep>();

            foreach (var iteration in Iterations.Iterations)
            {
                Steps.Add(new WebSimplexStep(iteration));

            }

        }

        public class WebSimplexStep
        {
            public List<string> ColumnHeaders { get; set; }
            public List<List<string>> Step { get; set; }

            public int? VariableEnteringBasisIndex { get; set; }

            public int? PivotRowIndex { get; set; }

            public WebSimplexStep(SimplexTableauIteration iteration)
            {
                this.VariableEnteringBasisIndex = iteration.VariableEnteringBasisIndex;
                this.PivotRowIndex = iteration.PivotRowIndex;
                this.Step = new List<List<string>> { iteration.Tableau.GetColumn(0).Select(i => i.VariableBasis).ToList() };
                this.Step.AddRange(iteration.Tableau.ToStringList());
                this.ColumnHeaders = new List<string> {"Basis"};
                this.ColumnHeaders.AddRange(iteration.Tableau.GetRow(0).Select(i => i.Name).ToList());
            }

        }


    }
}
