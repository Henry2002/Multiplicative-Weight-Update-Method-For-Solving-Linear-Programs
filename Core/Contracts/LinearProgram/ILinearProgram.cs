using TYP.Angular.Core.Models.LP;
using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Contracts.LP
{
    public interface ILinearProgram
    {
        //MinOrMax isnt stored as we will convert the LP to its dual form as soon as it is inputted//
        //This will also be useful for when we implement the MWUM//
        //Simplex should always maximise//
        public IList<Variable> ObjectiveFunction { get; set; }

        public IList<Constraint> Constraints { get; set; }

        public string MinOrMax { get; set; }

        public double passToObjectiveFunction(params Element[] parameters);

        public ILinearProgram GetDual();
    }
}
