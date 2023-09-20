using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Models.MWUM
{
    public class MWUMSummaryTable
    {
        public double MaxStep { get; set; }

        public Matrix ScaledGuessMatrix { get; set; }

        public Matrix PrimalVariableMatrix { get; set; }

        public Matrix DualVariableMatrix { get; set; }

        public MWUMSummaryTable(int PrimalVectorSize, int DualVectorSize)
        {
            PrimalVariableMatrix = new Matrix(DualVectorSize, PrimalVectorSize + 1);

            DualVariableMatrix = new Matrix(DualVectorSize, DualVectorSize + 1);

            PrimalVariableMatrix.InsertRange(0, PrimalVariableMatrix.Rows, 0, PrimalVariableMatrix.Columns, (x, y) =>
            {
                return new Element
                {
                    Name = y == PrimalVariableMatrix.Columns - 1 ? "StepSize" : "P" + (y + 1),
                    Value = 0,
                };
            });

            DualVariableMatrix.InsertRange(0, DualVariableMatrix.Rows, 0, DualVariableMatrix.Columns, (x, y) =>
            {
                return new Element
                {
                    Name = y==DualVariableMatrix.Columns-1? "StepSize" : "W" + (y + 1),
                    Value = 0,
                };
            });


        }


        public void AddRowSummary(Vector X, Vector Y, double StepSize, int RowIndex)
        {
           
            for (int i=0; i<X.Size+1; i++) PrimalVariableMatrix[RowIndex, i].Value = i== PrimalVariableMatrix.Columns - 1 ? StepSize : X[i].Value;

            for (int j = 0; j < Y.Size+1; j++) DualVariableMatrix[RowIndex, j].Value = j == DualVariableMatrix.Columns - 1 ? StepSize : Y[j].Value;

        }

        public Matrix GetFinalTable(bool SolvedDual)
        {
            if (SolvedDual)
            {
                var FinalTable = new Matrix(ScaledGuessMatrix.Rows, ScaledGuessMatrix.Columns + DualVariableMatrix.Columns);

                FinalTable.InsertRange(0, FinalTable.Rows, 0, FinalTable.Columns, (x, y) =>
                {
                    return new Element
                    {
                        Name = y < ScaledGuessMatrix.Columns ? ScaledGuessMatrix[x, y].Name : DualVariableMatrix[x, y-ScaledGuessMatrix.Columns].Name,
                        Value = y < ScaledGuessMatrix.Columns ? ScaledGuessMatrix[x, y].Value : DualVariableMatrix[x, y-ScaledGuessMatrix.Columns].Value
                    };
                });

                return FinalTable;
            } else
            {
                var FinalTable = new Matrix(ScaledGuessMatrix.Rows, ScaledGuessMatrix.Columns + PrimalVariableMatrix.Columns);

                FinalTable.InsertRange(0, FinalTable.Rows, 0, FinalTable.Columns, (x, y) =>
                {
                    return new Element
                    {
                        Name = y < ScaledGuessMatrix.Columns ? ScaledGuessMatrix[x, y].Name : PrimalVariableMatrix[x, y-ScaledGuessMatrix.Columns].Name,
                        Value = y < ScaledGuessMatrix.Columns ? ScaledGuessMatrix[x, y].Value : PrimalVariableMatrix[x, y-ScaledGuessMatrix.Columns].Value
                    };
                });

                return FinalTable;
            }
        }
    }
}

