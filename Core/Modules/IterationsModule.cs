using TYP.Angular.Core.Contracts.Modules;
using TYP.Angular.Core.Models;
using TYP.Angular.Core.Models.BaseResult;
using TYP.Angular.Core.Models.Simplex;
using TYP.Angular.Core.Models.Web;

namespace TYP.Angular.Core.Modules
{
    public class IterationsModule: IIterationsModule
    {
      
        public BaseResult<WebMWUMSteps> ReturnMWUMSteps()
        {
            if (IterationsManager.CurrentMWUMIterations is null)
                throw new NullReferenceException("The MWUM Algorithm has not been ran so algorithm iterations cannot be returned");

            return new BaseResult<WebMWUMSteps>(IterationsManager.CurrentMWUMIterations.GetWebFormat());
        }

        public BaseResult<WebSimplexSteps> ReturnSimplexSteps()
        {
            if (IterationsManager.CurrentSimplexTableauIterations is null) 
                throw new NullReferenceException("The Simplex Algorithm has not been ran so algorithm iterations cannot be returned");

            return new BaseResult<WebSimplexSteps>(IterationsManager.CurrentSimplexTableauIterations.getWebFormat());

        }

        public BaseResult ReturnAlgorithmSteps(string Algorithm)
        {
            if (Algorithm == "simplex")
            {
                return ReturnSimplexSteps();
            } else
            {
                return ReturnMWUMSteps();
            }
        }
    }
}
