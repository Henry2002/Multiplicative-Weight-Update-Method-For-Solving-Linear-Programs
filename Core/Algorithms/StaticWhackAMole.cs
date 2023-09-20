using TYP.Angular.Core.Algorithms;
using TYP.Angular.Core.Models.MatrixModels;
using TYP.Angular.Core.Models.MWUM;

namespace TYP.Angular.Core.Algorithms
{
    public class StaticWhackAMole {

        public static (Vector variables, bool solvedDual, MWUMSummaryTable SummaryTable) Run (Matrix GuessMatrix, double Epsilon, double Lambda)
        {
            //Pre-processing//

            var SummaryTable = new MWUMSummaryTable(GuessMatrix.Columns, GuessMatrix.Rows);

            var MaxStep = (int) Math.Round((Lambda * Math.Log(GuessMatrix.Rows)) / Math.Pow(Epsilon,2));

            SummaryTable.MaxStep = MaxStep; SummaryTable.ScaledGuessMatrix = GuessMatrix;

            var CurrentStep = 1;

            var PrimalVector = Vector.Identity(GuessMatrix.Columns);

            var DualVector = new Vector(GuessMatrix.Rows);

        Start:

            var OVAttempt = PrimalVector.L1Norm();

            for (int CurrentConstraint = 0; CurrentConstraint < GuessMatrix.Rows; CurrentConstraint++)
            {
                var Constraint=GuessMatrix.GetRowAsVector(CurrentConstraint);

                if (Vector.DotProduct(Constraint, PrimalVector / OVAttempt) < 1 - Epsilon / 2)
                {
                    //----------------------------------Enforce Subroutine--------------------------------------------------//
                    int Delta;
                    
                    if (Vector.DotProduct(Constraint, Whack(PrimalVector, Constraint, MaxStep-CurrentStep, Epsilon, Lambda) / OVAttempt) < 1)
                    {
                        Delta = MaxStep - CurrentStep;
                    }
                    else
                    {
                        var BinarySearchResult = new ConditionalBinarySearch((Index) =>
                        {
                            var Item = Whack(PrimalVector, Constraint, Index ,Epsilon, Lambda);

                            return (Vector.DotProduct(Constraint, Item / OVAttempt) >= 1, Item);

                        }).SearchForMinimum(0, MaxStep - CurrentStep); 

                        Delta = BinarySearchResult.index; //else find the smallest vector zk such that DotProduct(zk,Constraint)>CV

                        PrimalVector = BinarySearchResult.MinimumFound;
                    }

                    DualVector.DoToAll((ref Element element, int index) =>
                    {
                        element.Value += Delta / (double)MaxStep * (index == CurrentConstraint ? 1 : 0); //This needs to be the minumum OF value that variable appears in
                    });

                    //--------------------------------------------------------------------------------------------------------//

                    CurrentStep += Delta;

                    SummaryTable.AddRowSummary(PrimalVector, DualVector, CurrentStep, CurrentConstraint);

                    if (CurrentStep == MaxStep) return (DualVector, true, SummaryTable);
                     
                    if (PrimalVector.L1Norm() > (1 / (1 - Epsilon / 2)) * OVAttempt) goto Start;
                    
            }
        }

        return (PrimalVector / PrimalVector.L1Norm(), false, SummaryTable);

    }


    private static Vector Whack (Vector PrimalVector,Vector Constraint,int WhackingNumber, double Epsilon, double Lambda)
    {
        var zk = new Vector(PrimalVector.Size);

        zk.InsertRange(0, PrimalVector.Size, (index) =>
        {
            return new Element
            {
                Value = Math.Pow(1 + Epsilon * (Constraint[index].Value / Lambda), WhackingNumber) * PrimalVector[index].Value,
                Name = new string(PrimalVector[index].Name),
            };
        });

        return zk;

    }
}


}
