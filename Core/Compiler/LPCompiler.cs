
using TYP.Angular.Core.Compiler;
using TYP.Angular.Core.Contracts.LinearProgram;
using TYP.Angular.Core.Contracts.LP;
using TYP.Angular.Core.Models.LP;

namespace TYP.Angular.Core.Compile
{
    public class LPCompiler
    {

        public static ILPMatrix compileAsPCLP(string linearProgram)
        {
            //Store the string as a text file, to allow output to be downloaded in the future
            //A text file is created on input for purposes of using streamreader and giving user option of 
            //Dowloading and Uploading their LPs.

            File.WriteAllText("LinearProgram.txt", linearProgram);

            using (StreamReader fileReader = new StreamReader("LinearProgram.txt"))

            {
                var parser = new Parser(fileReader); 

                var linearProgramAST = parser.parseLinearProgram();//Lex and parse LP

                var PCLP = LPGenerator.generatePackingCoveringLinearProgram(linearProgramAST); //generate a packing-covering LP

                return new LPMatrix(PCLP); //Get its matrix form

            }
        }


    }

    
}