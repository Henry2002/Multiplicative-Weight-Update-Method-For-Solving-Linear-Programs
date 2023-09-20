using TYP.Angular.Core.Algorithms;
using TYP.Angular.Core.Models.MatrixModels;
using TYP.Angular.Core.Models.Web;

namespace TYP.Angular.Core.Models.Simplex
{
    public class SimplexTableauIteration
    {
        public Matrix<TableauElement> Tableau { get; set; }

        public int? PivotRowIndex { get; set; }

        public int? VariableEnteringBasisIndex { get; set; }

        public SimplexTableauIteration(Matrix<TableauElement> Tableau) 
        {
            this.Tableau= Tableau.DeepClone();
        }
    }

    public class SimplexTableauIterations
    {
        public  LinkedList<SimplexTableauIteration> Iterations { get; set; }= new LinkedList<SimplexTableauIteration>();

        public SimplexTableauIterations()
        {
            Iterations= new LinkedList<SimplexTableauIteration>();
        }

        public void Add(SimplexTableau Tableau, int? PivotRowIndex, int? VariableEnteringBasisIndex)
        {
            var iteration = new SimplexTableauIteration(Tableau)
            {
                PivotRowIndex = PivotRowIndex,
                VariableEnteringBasisIndex = VariableEnteringBasisIndex,
            };
            Iterations.AddLast(iteration);
        }

        public WebSimplexSteps getWebFormat()
        {
            return new WebSimplexSteps(this);
        }


    }
}
