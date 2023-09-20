using TYP.Angular.Core.Models.BaseResult;
using TYP.Angular.Core.Models.Web;

namespace TYP.Angular.Core.Contracts.Modules
{
    public interface IIterationsModule
    {
        public BaseResult<WebSimplexSteps> ReturnSimplexSteps();

        public BaseResult<WebMWUMSteps> ReturnMWUMSteps();

        public BaseResult ReturnAlgorithmSteps(string Algorithm);
    }
}
