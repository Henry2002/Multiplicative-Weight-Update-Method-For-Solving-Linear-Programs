using Microsoft.AspNetCore.Mvc;
using TYP.Angular.Core.Contracts.LP;
using TYP.Angular.Core.Contracts.Modules;
using TYP.Angular.Core.Models.BaseResult;
using TYP.Angular.Core.Models.Compiler;
using TYP.Angular.Core.Models.LP;

namespace TYP.Angular.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolverController : ControllerBase
    {

        private ISolverModule SolverModule;

        public SolverController(ISolverModule SolverModule)
        {
            this.SolverModule = SolverModule; //Dependency Injection
        }

        [HttpPost("solveLinearProgram")]

        public BaseResult SolveLinearProgram(WebLinearProgram linearProgram)
        {
            try
            {
                return this.SolverModule.SolveLinearProgram(linearProgram.LinearProgram, linearProgram.SelectedAlgorithm, (double) linearProgram.SelectedEpsilon);
            }
            catch (Exception e)
            {
                return new BaseResult(false, e.Message);
            }
        }

        [HttpPost("compareAlgorithms")]

        public BaseResult CompareAlgorithms(WebLinearProgram LinearProgram)
        {
            try
            {
                return this.SolverModule.CompareAlgorithms(LinearProgram.LinearProgram);
            }
            catch (Exception e)
            {
                return new BaseResult(false, e.Message);
            }
        }


    }
}