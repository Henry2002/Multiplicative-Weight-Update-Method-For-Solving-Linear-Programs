using System.Text.RegularExpressions;
using TYP.Angular.Core.Algorithms;

namespace TYP.Angular.Core.ExtensionMethods
{
    public static class CharExtensions
    {
        

        //Helper method to check if character is alphabetic
        public static bool IsAlpha(this char @this)
        {
            return !Regex.IsMatch(@this.ToString(), "[^a-zA-Z]");
        }

        public static bool IsNum(this char @this)
        {
            return !Regex.IsMatch(@this.ToString(), "[^0-9]");
        }

        //Helper method to check if character is alphanumeric
        public static bool IsAlphaNum(this char @this)
        {
            return !Regex.IsMatch(@this.ToString(), "[^a-zA-Z0-9]");
        }

        
    }


}
