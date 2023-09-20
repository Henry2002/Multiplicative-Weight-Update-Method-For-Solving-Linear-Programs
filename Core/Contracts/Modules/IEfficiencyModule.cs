using TYP.Angular.Core.Models.BaseResult;

namespace TYP.Angular.Core.Contracts.Modules
{
    public interface IEfficiencyModule
    {
       BaseResult<string> CompareEfficiency(int X, int Y, int? Accuracy = null, params double[]? epsilons);
    }
}
