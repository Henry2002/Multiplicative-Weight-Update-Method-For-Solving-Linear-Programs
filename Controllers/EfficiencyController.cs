using Microsoft.AspNetCore.Mvc;
using TYP.Angular.Core.Contracts.Modules;
using TYP.Angular.Core.Models.BaseResult;
using TYP.Angular.Core.Models.Web;

namespace TYP.Angular.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EfficiencyController : ControllerBase
    {
        private IEfficiencyModule Module;

        public EfficiencyController(IEfficiencyModule module)
        {
            this.Module = module;
        }

        [HttpPost("compareEfficiency")]

        public BaseResult<string> CompareEfficiency(WebEfficiencyRequest Request)
        {
            return this.Module.CompareEfficiency(Request.X, Request.Y, Request.Accuracy, Request.epsilons);
        }
    }
}
