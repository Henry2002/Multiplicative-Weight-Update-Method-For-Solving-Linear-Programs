using TYP.Angular.Core.Contracts.LP;

namespace TYP.Angular.Core.Models.LP
{
    public class Constraint : IConstraint
    {
        public IList<Variable> Variables { get; set; }

        public string Operation { get; set; }

        public double ConstrainingValue { get; set; }

       
    }
}
