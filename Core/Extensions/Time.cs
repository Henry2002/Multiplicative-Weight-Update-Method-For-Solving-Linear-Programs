using System.Diagnostics;

namespace TYP.Angular.Core.Extensions
{
    public class Time<T>
    {
        public static (T Result, double Performance) TimeInMilliseconds (Func<T> Action)
        {
            Stopwatch Stopwatch = Stopwatch.StartNew();

            var Result=Action();

            Stopwatch.Stop();

            return (Result,Stopwatch.Elapsed.TotalMilliseconds);


        }
    }
}
