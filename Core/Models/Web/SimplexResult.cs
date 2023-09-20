using TYP.Angular.Core.Models.BaseResult;
using TYP.Angular.Core.Models.Simplex;

namespace TYP.Angular.Core.Models.Web
{
    public class SimplexResult: BaseResult<string>
    {
        public SimplexResult(IList<TableauElement> Result, double Performance): base(generateWebResult(Result, Performance)){}

        public static string generateWebResult(IList<TableauElement> Result, double Performance)
        {
            string webResult = " Result:\n" +
                " Z = " + Result[0].Value + "\n"
                + "\n" + " Basis: \n";
                

            for (int i = 1; i < Result.Count; i++) { 
                webResult += " " + Result[i].VariableBasis + " = " + Result[i].Value + "\n";
            }

            return webResult + "\n Performance (ms):" + Performance;  //Return the results
        }
    }
}
