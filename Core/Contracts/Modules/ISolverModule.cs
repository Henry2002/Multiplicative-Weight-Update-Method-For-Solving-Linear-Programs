using TYP.Angular.Core.Models.BaseResult;
using TYP.Angular.Core.Models.Web;

namespace TYP.Angular.Core.Contracts.Modules
{
    public interface ISolverModule
    {
        public BaseResult<string> SolveLinearProgram(string linearProgram, string selectedAlgorithm, double SelectedEpsilon);

        public BaseResult<string> SolveWithSimplex(string linearProgram);

        public BaseResult<string> SolveWithMWUM(string linearProgram, double epsilon);

        public BaseResult CompareAlgorithms(string LinearProgram);
    }
}
