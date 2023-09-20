using TYP.Angular.Core.Algorithms;
using TYP.Angular.Core.Contracts.Modules;
using TYP.Angular.Core.Models.BaseResult;
using TYP.Angular.Core.Models.Web;

namespace TYP.Angular.Core.Modules
{
    public class EfficiencyModule : IEfficiencyModule
    {

        public BaseResult<string> CompareEfficiency (int X, int Y, int? Accuracy = null, params double[]? epsilons)
        {
            return new EfficiencyResult(PerformanceAnalysis.ComparePerformance(X, Y, Accuracy, epsilons));
        }
    }
}
