using TYP.Angular.Core.ExtensionMethods;
using TYP.Angular.Core.Models.LP;

namespace TYP.Angular.Core.Models.MatrixModels
{
    public class Element : IComparable<Element>, IDeepCloneable<Element>
    {
        public string Name { get; set; }

        public double Value { get; set; }


        public virtual int CompareTo(Element other)
        {
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return " "+ Name + " = " + Value;
        }

        public virtual Element DeepClone()
        {
            return new Element
            {
                Name = new string(Name),
                Value = Value,

            };
        }

        public static Element operator *(Element lhs, Element rhs)
        {
            return new Element {
                Name = new string(lhs.Name),
                Value = lhs.Value * rhs.Value };

        }

        public static Element operator *(Element lhs, double rhs)
        {
            return new Element
            {
                Name = new string( lhs.Name),
                Value = lhs.Value * rhs
        };
    

        }

        public static Element operator / (Element lhs, double rhs)
        {
            return new Element
            {
                Name = new string(lhs.Name),
                Value = lhs.Value / rhs,
            };
        }

        public static Element operator / (Element lhs, Element rhs)
        {
            return new Element
            {
                Name = new string(lhs.Name),
                Value = lhs.Value / rhs.Value,
            };
        }

        public static Element operator + (Element lhs, Element rhs)
        {
            return new Element
            {
                Name = new string(lhs.Name),
                Value = rhs.Value + lhs.Value,
            };
        }

        public static explicit operator double (Element operand)
        {
            return operand.Value;
        }

        public static explicit operator Element(Variable variable)
        {
            return new Element
            {
                Name = new string(variable.Name),
                Value = variable.Coefficient,
            };
        }
    }
}
