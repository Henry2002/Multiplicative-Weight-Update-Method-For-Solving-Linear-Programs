
using TYP.Angular.Core.Models.MatrixModels;
using TYP.Angular.Core.Models.Web;

namespace TYP.Angular.Core.Models.MWUM
{
    public class MWUMIteration
    {

        public double Guess { get; set; }

        public bool SolvedDual { get; set; }

        public bool IsSatisfactory { get; set; }

        public double MaxStep { get; set; }

        public Matrix WhackAMoleSummary { get; set; }
    }

    public class MWUMIterations
    {
        public LinkedList<MWUMIteration> Iterations { get; set; } = new LinkedList<MWUMIteration>();

        public double UpperBound { get; set; }

        public double LowerBound { get; set; }

        public void Add(MWUMSummaryTable WhackAMoleSummary, double Guess, bool IsSatisfactory, bool SolvedDual)
        {
            Iterations.AddLast(new MWUMIteration
            {
                Guess = Guess,
                IsSatisfactory = IsSatisfactory,
                SolvedDual = SolvedDual,
                WhackAMoleSummary = WhackAMoleSummary.GetFinalTable(SolvedDual),
                MaxStep=WhackAMoleSummary.MaxStep,
            });
        }
        public MWUMIterations() { }

        public WebMWUMSteps GetWebFormat()
        {
            return new WebMWUMSteps(this);
        }
    }
}
