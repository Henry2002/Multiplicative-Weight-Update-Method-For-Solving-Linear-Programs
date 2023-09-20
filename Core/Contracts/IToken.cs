
namespace TYP.Angular.Core.Contracts
{
    public interface ITOKEN
    {

        string lexeme { get; set; }

        double value { get; set; }

        int type { get; set; }

        int lineNumber { get; set; }

        int columnNumber { get; set; }

    }

}