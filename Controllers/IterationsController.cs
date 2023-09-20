using Microsoft.AspNetCore.Mvc;
using TYP.Angular.Core.Contracts.Modules;
using TYP.Angular.Core.Models.BaseResult;

namespace TYP.Angular.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IterationsController:ControllerBase
    {
        private IIterationsModule IterationsModule;

        public IterationsController(IIterationsModule IterationsModule)
        {
            this.IterationsModule = IterationsModule; //Dependency Injection
        }

        [HttpGet("getAlgorithmSteps/{selectedAlgorithm}")]

        public BaseResult getAlgorithmSteps(string SelectedAlgorithm)
        {
            try
            {
                return this.IterationsModule.ReturnAlgorithmSteps(SelectedAlgorithm);
            }
            catch (Exception e)
            {
                return new BaseResult(false, e.Message);
            }
        }
    }
}
