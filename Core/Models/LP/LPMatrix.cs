using TYP.Angular.Core.Contracts.LinearProgram;
using TYP.Angular.Core.Contracts.LP;
using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Models.LP
{
    public class LPMatrix: ILPMatrix
    {
       
        public string MinOrMax { get; set; }
        
        public Vector ObjectiveFunctionVector { get; set; }

        public Vector ConstrainingValueVector { get; set; }

        public Matrix ConstraintsMatrix { get; set; }


        public LPMatrix() { }
        public LPMatrix(ILinearProgram LinearProgram)
         {

            InitialiseMatrixForm(LinearProgram);
         }

        //----------------------------------Matrix Form Conversion------------------------------------------//

        private void InitialiseMatrixForm(ILinearProgram LinearProgram)
        {
            MinOrMax = LinearProgram.MinOrMax;

            ConstrainingValueVector = new Vector(LinearProgram.Constraints.Select(i => new Element
            {
                Value = i.ConstrainingValue,
                Name = "Constraining Value"

            }).ToList());

            //Also order the objective function alphabetically so that variables in the objective function line
            //up with variabkes in the constraints
            ObjectiveFunctionVector = new Vector(LinearProgram.ObjectiveFunction.Select(i=>(Element)i).OrderBy(element => element.Name).ToList());

            ConstraintsMatrix = new Matrix(LinearProgram.Constraints.Select(i => Reformat(i, LinearProgram.ObjectiveFunction)).ToList());

        }


        private static string VariableNameSelector(Variable variable) => variable.Name;
        private static List<Element> Reformat(Constraint Constraint, IList<Variable> ObjectiveFunction)
        {
            var Variables=Constraint.Variables;

            //foreach variable in the objective function that does not appear in the constraint
            foreach (var DistinctVariable in ObjectiveFunction.ExceptBy(Variables.Select(VariableNameSelector), VariableNameSelector))
            {
                Variables.Add(new Variable
                {
                    Name = DistinctVariable.Name,
                    Coefficient = 0,
                }); //Add new "blank" variables
            }
            //Order by name alphabetically
           return Variables.OrderBy(Variable => Variable.Name).Select(i => (Element)i).ToList(); 

        }
        

        //---------------------------------------------------------------------------------------------------------//
       

        //Dual Conversion Algorithm
        public void ConvertToDual()
        {
            ConstraintsMatrix = ConstraintsMatrix.Transpose();

            var ObjectiveFunction = ObjectiveFunctionVector.DeepClone();

            var ConstrainingValues = ConstrainingValueVector.DeepClone();

            ObjectiveFunctionVector=new Vector(ConstrainingValues.Size);

            ConstrainingValueVector=new Vector(ObjectiveFunction.Size);

            ObjectiveFunctionVector.InsertRange(0,ObjectiveFunctionVector.Size, (index) => {
                return new Element
                {
                    Name = "W" + (index + 1),
                    Value = ConstrainingValues[index].Value,
                };
            });

            ConstrainingValueVector.InsertRange(0,ConstrainingValueVector.Size, (index) => {
                return new Element
                {
                    Name = "Constraining Value",
                    Value = ObjectiveFunction[index].Value
                };
                });
        
            if (MinOrMax == "MIN")
            {
                MinOrMax = "MAX";
            } else
            {
                MinOrMax = "MIN";
            }
        }

        public Matrix ScaledConstraints()
        {
            var CPrime = new Matrix(ConstraintsMatrix.Rows, ConstraintsMatrix.Columns);

            CPrime.InsertRange(0, CPrime.Rows, 0, CPrime.Columns, (x, y) =>
            {
                return new Element
                {
                    Name = new string(ConstraintsMatrix[x, y].Name),
                    Value = ConstraintsMatrix[x, y].Value / (ConstrainingValueVector[x].Value * ObjectiveFunctionVector[y].Value),
                };
            });

            return CPrime;
        }

        public Element? MinNonZero()
        {
            return new List<Element> { ConstraintsMatrix.MinNonZero(),
                ConstrainingValueVector.MinNonZero(), ObjectiveFunctionVector.MinNonZero()}.Min();
        }

        public Element? MaxNonZero()
        {
            return new List<Element> { ConstraintsMatrix.MaxNonZero(),
                ConstrainingValueVector.MaxNonZero(), ObjectiveFunctionVector.MaxNonZero()}.Max();
        }
    }

    


}
