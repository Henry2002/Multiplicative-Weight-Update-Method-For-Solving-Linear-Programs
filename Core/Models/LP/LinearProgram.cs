using TYP.Angular.Core.Compiler;
using TYP.Angular.Core.Contracts.LinearProgram;
using TYP.Angular.Core.Contracts.LP;
using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Models.LP
{


    //Linear programs will contain enumerable data structures for its constrints and OF, but also
    //contain a matrix, 
    public class LinearProgram : ILinearProgram
    {
        public IList<Variable> ObjectiveFunction { get; set; }

        public IList<Constraint> Constraints { get; set; }

        public string MinOrMax { get; set; }


        public double passToObjectiveFunction(params Element[] parameters)
        {
            double result = 0;

            if (parameters.Length != ObjectiveFunction.Count) throw new ArgumentException("Parameter mismatch when passing to Objective Function");

            for(int i=0; i<parameters.Length; i++)
            {
               var OFVariable= ObjectiveFunction.Where(variable => variable.Name == parameters[i].Name).First();

                result += OFVariable.Coefficient * parameters[i].Value;
                
            }

            return result;
        }

        public ILinearProgram GetDual()
        {
            var LPMatrix = new LPMatrix(this);

            LPMatrix.ConvertToDual();

            return LPGenerator.generateLinearProgram(LPMatrix);
        }
    }
}
