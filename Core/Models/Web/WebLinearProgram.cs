namespace TYP.Angular.Core.Models.BaseResult
{
    //WebLinearProgram- A class that allows the SolverController to accept the LP and to know what algorithm it should use
    public class WebLinearProgram
    {
       public string LinearProgram { get; set; }

       public string? SelectedAlgorithm { get; set;}

       public double? SelectedEpsilon { get; set;}
    }
}
