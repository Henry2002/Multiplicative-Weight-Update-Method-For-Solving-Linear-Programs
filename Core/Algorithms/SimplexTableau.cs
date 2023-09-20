using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.Data.Common;
using System.Xml.Linq;
using TYP.Angular.Core.Contracts.LinearProgram;
using TYP.Angular.Core.Contracts.LP;
using TYP.Angular.Core.Models.Compiler;
using TYP.Angular.Core.Models.LP;
using TYP.Angular.Core.ExtensionMethods;
using TYP.Angular.Core.Models.Simplex;
using TYP.Angular.Core.Models.MatrixModels;
using TYP.Angular.Core.Models;
using System.Diagnostics;

namespace TYP.Angular.Core.Algorithms
{
    //Going to have to code an LP Converter thing to turn the AST nodes into semantically checked proper formatted LPs. :( ffs
    public class SimplexTableau : Matrix<TableauElement>
    {
        //This just renames the base matrix attribute to Tableau for the algorithm//

        public SimplexTableauIterations Iterations { get; set; }=new SimplexTableauIterations();

        public SimplexTableau(ILPMatrix LPMatrix):base(LPMatrix.ConstraintsMatrix.Rows + 1,
                  LPMatrix.ObjectiveFunctionVector.Size + LPMatrix.ConstraintsMatrix.Rows + 2)
        {
            InitialiseTableau(LPMatrix);

            
        }
        private void InitialiseTableau(ILPMatrix LPMatrix)
        {

            var Constraints=LPMatrix.ConstraintsMatrix;

            var ObjectiveFunction=LPMatrix.ObjectiveFunctionVector;

            var ConstrainingValues = LPMatrix.ConstrainingValueVector;

            InsertRange(0,Rows, 0, 1, (x, y) =>
            {
                return new TableauElement
                {
                    Value = x == 0 ? 1 : 0,
                    VariableBasis = x == 0 ? "Objective Function" : "Slack" + x,
                    Name = "Z",
                };
            });

            InsertRange(0,Rows,1,ObjectiveFunction.Size, (x,y) => {

                return new TableauElement
                {
                    VariableBasis = x==0? "Objective Function": "Slack" + x,
                    Value = x==0?-ObjectiveFunction[y].Value: Constraints[x-1, y].Value,
                    Name = ObjectiveFunction[y].Name,
                };
            }) ;


            var identity = Matrix.Identity(Constraints.Rows);

            InsertRange(0,Rows, Constraints.Columns+1, Constraints.Rows, (x, y) =>
            {
                return new TableauElement
                {
                    Value = x==0? 0:identity[x-1, y].Value,
                    Name = "Slack " + (y+1),
                    VariableBasis = x==0?"Objective Function": "Slack " + (x + 1),
                }; 
            });

            InsertRange(0, Rows ,Columns-1,1, (x,y) =>
            {
                return new TableauElement
                {
                    Name = "Constraining Value",
                    VariableBasis = y == 0 ? "Objective Function" : "Slack" + (y + 1),
                    Value = x == 0 ? 0 : ConstrainingValues[x-1].Value,
                };
            });

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------//
        //----------------------------------------------------------SIMPLEX ALGORITHM ----------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------------------//


        public SimplexTableau runSimplex()
        {
            
            int? variableToMakeBasicIndex;

            while ((variableToMakeBasicIndex = findVariableToMakeBasic()) != null)
            {
                var pivotRowIndex = getPivotRowIndex((int)variableToMakeBasicIndex);

                updateTableau(pivotRowIndex, (int)variableToMakeBasicIndex);

            }
           

            IterationsManager.CurrentSimplexTableauIterations = Iterations;

            return this;
        }

        public int? findVariableToMakeBasic()
        {

            var objectiveFunctionRow = GetRow(0, 0, Columns - 1); //Get OF row but skip the CVs

            var minValue = objectiveFunctionRow.Min(); //Find minimum value in objective function

            if (minValue.Value >= 0)
            {
                Iterations.Add(this, null, null);

                return null;
            }; //Make sure it is negative if not return null

            var minIndex = objectiveFunctionRow.ToList().IndexOf(minValue); //Get the index of the minimum/maximum value

            return minIndex;
        }
        public int getPivotRowIndex(int columnIndex)
        {
            var column = GetColumn(columnIndex, 1, Rows - 1); //Get the column as IEnumerable, but skip the OF row

            var constrainingValues = GetColumn(Columns - 1, 1, Rows - 1); //Get the CVs as IEnumerable,skip OF row

            var RatioTestedConstrainingValues = constrainingValues.Zip(column, (a, b) => a / b).Where(i => i > 0).ToList();

            if (!RatioTestedConstrainingValues.Any()) throw new MathematicalError("The feasible region for the LP inputted is unbounded");

            var pivotRowIndex = RatioTestedConstrainingValues.IndexOf(RatioTestedConstrainingValues.Min())+1;  //Divide CV by column values sequentially then find the minimum/maximum- exclude negative values

            Iterations.Add(this, pivotRowIndex, columnIndex);

            return pivotRowIndex;

        }

        public void updateTableau(int pivotRowIndex, int variableToMakeBasicIndex)
        {
            var basicVariable = this[pivotRowIndex, variableToMakeBasicIndex]; //Get the basic variable

            for (int i = 0; i < Columns; i++)
            {
                this[pivotRowIndex, i].VariableBasis = basicVariable.Name;

                this[pivotRowIndex, i]= this[pivotRowIndex, i]/basicVariable;
            }

            var pivotRow = GetRow(pivotRowIndex);  //Get full pivot row

            var basicColumn = GetColumn(variableToMakeBasicIndex); // Get full column of elements to make basic

            for (int i = 0; i < Rows; i++)
            {
                var rowToUpdate = GetRow(i, 0, Columns).ToList(); //Get the full row to update

                if (i == pivotRowIndex) continue; //Skip the pivot row

                var updatedRow = rowToUpdate.Zip(pivotRow, (a, b) => a + b * -basicColumn.ElementAt(i)).ToList();

                //Since 2d arrays arent enumerable nested for loops need to be used to update them
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] = updatedRow.ElementAt(j);
                }
            }

            

        }
    }



}



