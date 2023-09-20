using TYP.Angular.Core.Models.MWUM;
using TYP.Angular.Core.Models.Simplex;

namespace TYP.Angular.Core.Models
{
    public static class IterationsManager
    {
        public static SimplexTableauIterations? CurrentSimplexTableauIterations { get; set; }

        public static MWUMIterations? CurrentMWUMIterations { get; set; }
    }
}
