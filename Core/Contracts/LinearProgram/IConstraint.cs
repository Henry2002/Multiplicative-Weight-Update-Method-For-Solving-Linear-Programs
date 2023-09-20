using TYP.Angular.Core.Models.LP;

namespace TYP.Angular.Core.Contracts.LP
{
    public interface IConstraint
    {
        public IList<Variable> Variables { get; set; }

        public string Operation { get; set; }

        public double ConstrainingValue { get; set; }
    }
}
