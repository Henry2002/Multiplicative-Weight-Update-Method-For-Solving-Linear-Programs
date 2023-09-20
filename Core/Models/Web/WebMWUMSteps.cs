using TYP.Angular.Core.Models.MWUM;

namespace TYP.Angular.Core.Models.Web
{
    public class WebMWUMSteps
    {
        public List<MWUMStep> Steps { get; set; } = new List<MWUMStep>();

        public string UpperBound { get; set; }

        public string LowerBound { get; set; }

        public WebMWUMSteps(MWUMIterations iterations)
        {
            foreach (var iteration in iterations.Iterations) 
            {
                Steps.Add(new MWUMStep(iteration));
            }

            UpperBound = iterations.UpperBound.ToString(); LowerBound=iterations.LowerBound.ToString();
        }
    }

    public class MWUMStep
    {
        public List<string> ColumnHeaders { get; set; } = new List<string> { "Guess","Max Step", "Satisfactory", "Solved Primal/Dual" };

        public List<string> SummaryTableColumnHeaders { get; set; } = new List<string>();
        public List<List<string>> SummaryTable { get; set; }=new List<List<string>>();

        public string Guess { get; set; }

        public string MaxStep { get; set; }

        public string IsSatisfactory { get; set; }

        public string SolvedDual { get; set; }
        public MWUMStep(MWUMIteration Iteration) 
        {
            Guess = Iteration.Guess.ToString();
            IsSatisfactory = Iteration.IsSatisfactory ? "Yes" : "No";
            SolvedDual = Iteration.SolvedDual ? "Dual" : "Primal";

            for(int i = 0; i<Iteration.WhackAMoleSummary.Rows;i++) {
                if (Iteration.WhackAMoleSummary[i, Iteration.WhackAMoleSummary.Columns-1].Name is not null)
                {
                    SummaryTableColumnHeaders.AddRange(Iteration.WhackAMoleSummary.GetRow(i).Select(i=>i.Name));
                    break;
                }
            }

            SummaryTable=Iteration.WhackAMoleSummary.ToStringList();

            MaxStep = Iteration.MaxStep.ToString();
        }
    }
}
