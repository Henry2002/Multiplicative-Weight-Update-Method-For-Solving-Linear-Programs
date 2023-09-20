using TYP.Angular.Core.Models.BaseResult;
using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Models.Web
{
    public class MWUMResult:BaseResult<string>
    {
        public MWUMResult(double Result, IList<Element> Variables, double Performance): base(generateWebResult(Result,Variables, Performance)) { }

        public static string generateWebResult(double Result,IList<Element> Variables, double Performance)
        {
            //Change this, this is just so I can see what the hell is going on//

            string webResult = "Result: \n" + " Z = " + Result + "\n";

            foreach (Element element in Variables)
            {
                webResult += element.ToString() +" \n";
            }

            return webResult +"\n Performance (ms) :"+Performance;
        }
    }
}
