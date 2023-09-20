using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Algorithms
{
    public class ConditionalBinarySearch
    {
        private Func<int, (bool isSatisfactory, dynamic Value)> FindingCondition;

        public ConditionalBinarySearch(Func<int, (bool isSatisfactory, dynamic Value)> FindingCondition)
        {
            this.FindingCondition = FindingCondition;
        }

        public (int index, dynamic MinimumFound) SearchForMinimum(int MinIndex, int MaxIndex)
        {
            if (MinIndex > MaxIndex)
            {
                throw new MathematicalError("A minimimum could not be found");
            }

            var Midpoint = (MinIndex + MaxIndex) / 2;

            var CurrentItem = FindingCondition(Midpoint);


            if (CurrentItem.isSatisfactory)
            {
                var LowerValueItem = FindingCondition(Midpoint - 1);

                if (!LowerValueItem.isSatisfactory)
                {
                    return (Midpoint, CurrentItem.Value);
                }
                else
                {
                    return SearchForMinimum(MinIndex, Midpoint - 1);
                }
            }
            else
            {
                return SearchForMinimum(Midpoint + 1, MaxIndex);
            }

        }

       
    }
}
