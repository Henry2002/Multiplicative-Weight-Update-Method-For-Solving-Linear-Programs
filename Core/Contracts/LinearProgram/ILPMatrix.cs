using TYP.Angular.Core.Contracts.LP;
using TYP.Angular.Core.Models.LP;
using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Contracts.LinearProgram
{
    public interface ILPMatrix
    {

        string MinOrMax { get; set; }

        Vector ObjectiveFunctionVector { get; set; }

        Vector ConstrainingValueVector { get; set; }

        Matrix ConstraintsMatrix { get; set; }


        Element? MinNonZero();

        Element? MaxNonZero();
        void ConvertToDual();

        Matrix ScaledConstraints();

    }
}
