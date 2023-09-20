using TYP.Angular.Core.Algorithms;
using TYP.Angular.Core.Models.BaseResult;
using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Models.Web
{
    public class EfficiencyResult : BaseResult<string>
    {
        public EfficiencyResult(PerformanceResult Result) : base(generateWebResult(Result)) { }
        public static string generateWebResult(PerformanceResult Result)
        {

            string webResult =" Dimensions = "+Result.Dimensions.ToString()+ "\n Accuracy = " + Result.Accuracy+ "\n SimplexTime = " + Result.SimplexTime + "\n MWUMTime =";

            foreach (var element in Result.MWUMEpsilonTime)
            {
                webResult += element.ToString() +", ";
            }

            return webResult;

        }
    }
}
