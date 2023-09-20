using TYP.Angular.Core.Contracts.LP;
using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Models.LP
{
    public class Variable :IVariable
    {
        public string Name { get; set; }

        public double Coefficient { get; set; }

        public static implicit operator Variable(Element v)
        {
            return new Variable
            {
                Name = v.Name,
                Coefficient = v.Value
            };

        }
    }
}
