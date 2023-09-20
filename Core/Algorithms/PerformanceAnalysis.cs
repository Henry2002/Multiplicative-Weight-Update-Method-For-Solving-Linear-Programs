
using System.Drawing;
using TYP.Angular.Core.Contracts.LinearProgram;
using TYP.Angular.Core.Extensions;
using TYP.Angular.Core.Models.LP;
using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Algorithms
{
    public class PerformanceAnalysis
    {

        public static PerformanceResult ComparePerformance(int X, int Y, int? Accuracy=null, params double[]? epsilons)
        {
            var Result = new PerformanceResult();

            Result.Accuracy = Accuracy ?? 100;

            Result.Dimensions = (X, Y);

            var SimplexEfficencies = new List<double>();

            for (int i = 0; i < (Accuracy ?? 100); i++) try
                {
                    SimplexEfficencies.Add(GetRandomSimplexEfficiency(X, Y));

                } catch { continue; };


            Result.SimplexTime= Queryable.Average(SimplexEfficencies.AsQueryable());

            foreach (var epsilon in epsilons ?? new double[] { 0.005, 0.01, 0.1, 0.5 })
            {
                var MWUMEfficencies = new List<double>();

                for (int i = 0; i < (Accuracy ?? 100); i++) try
                    {
                        MWUMEfficencies.Add(GetRandomMWUMEfficiency(X, Y, epsilon));

                    } catch {continue;}
                   
                Result.MWUMEpsilonTime.Add(epsilon, Queryable.Average(MWUMEfficencies.AsQueryable()));
            }

            return Result;

        }

        public static double GetRandomSimplexEfficiency(int X, int Y)
        {
            return GetRunningTime(RandomLP(X,Y));
        }

        public static double GetRandomMWUMEfficiency(int X, int Y, double epsilon)
        {
            return GetRunningTime(RandomLP(X, Y), epsilon);
        }

        public static double GetRunningTime(ILPMatrix LP, double? epsilon=null)
        {
            return epsilon is null ?

            Time<SimplexTableau>.TimeInMilliseconds(() =>
            {
                return new SimplexTableau(LP).runSimplex();

            }).Performance
            :
            Time<(double ObjectiveValue, IList<Element> Variables)>.TimeInMilliseconds(() =>
            {
                return new MWUM(LP, (double)epsilon).RunStaticWhackAMole();
            }).Performance; 
        }

        public static List<Element> RandomElements(int Size)
        {
            Random random = new Random();

            return Enumerable.Range(0, Size).Select(i => new Element { Name = "Rand" + i, Value = random.Next(100) }).ToList();
        }

        public static List<List<Element>> RandomElements(int X, int Y)
        {

            return Enumerable.Range(0, X).Select(i => RandomElements(Y)).ToList();
        }

        public static ILPMatrix RandomLP(int X, int Y)
        {
            return new LPMatrix
            {
                ObjectiveFunctionVector = new Vector(RandomElements(X)),
                ConstrainingValueVector = new Vector(RandomElements(Y)),
                ConstraintsMatrix = new Matrix(RandomElements(X, Y)),
                MinOrMax = "MAX",
            };
        }
    }

    public class PerformanceResult
    {
        public double SimplexTime { get; set; }

        public Dictionary<double, double> MWUMEpsilonTime{ get; set; } = new Dictionary<double, double>();

        public int Accuracy { get; set; }

        public double[] Epsilons { get; set; }

        public (int X,int Y) Dimensions { get; set; }   
    }
}
