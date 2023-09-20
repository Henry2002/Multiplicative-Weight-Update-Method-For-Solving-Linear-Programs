using TYP.Angular.Core.ExtensionMethods;
using TYP.Angular.Core.Models.Compiler;
using TYP.Angular.Core.Models.LP;
using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Models.Simplex
{
    public class TableauElement : Element
    {
        //Tableau entries will be treated in a similar way to complex numbers, where there is a real part and an imaginary part

        public string VariableBasis { get; set; }

        public double BigMCoefficient { get; set; }


        //Implements CompareTo so Min() and Max() functions can work on enumerables containing TableauEntrys
        public int CompareTo(TableauElement other)
        { 

            if (other.Value ==0 ^ other.BigMCoefficient == 0)
            {
                if (other.Value == 0)
                {
                    return BigMCoefficient.CompareTo(other.BigMCoefficient);
                }
                else
                {
                    return Value.CompareTo(other.Value);
                }
            }

            if (other.BigMCoefficient== BigMCoefficient)
            {
                return Value.CompareTo(other.Value);

            }
            else
            {
                return BigMCoefficient.CompareTo(other.BigMCoefficient);
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override TableauElement DeepClone()
        {
            return new TableauElement
            {
                Value = Value, //Primitive types dont need special treatment
                BigMCoefficient = BigMCoefficient,
                VariableBasis = new string(VariableBasis), //string non primitive so we need a new reference for it
                Name = new string(Name),
            };
        }


        //This is a non commutative + as the new variable will take the name of the one on the lhs.
        public static TableauElement operator +(TableauElement lhs, TableauElement rhs)
        {
        
            return new TableauElement
            {
                BigMCoefficient = lhs.BigMCoefficient + rhs.BigMCoefficient,
                Value = lhs.Value + rhs.Value,
                VariableBasis = lhs.VariableBasis,
                Name = lhs.Name,
            };
        }
        //This is a non commutative - as the new variable will take the name of the one on the lhs.
        public static TableauElement operator -(TableauElement lhs, TableauElement rhs)
        { 
            return new TableauElement
            {
                BigMCoefficient = lhs.BigMCoefficient - rhs.BigMCoefficient,
                Value = lhs.Value - rhs.Value,
                VariableBasis = lhs.VariableBasis,
                Name = lhs.Name,
            };
        }

        //This is a non commutative * as the new variable will take the name of the one on the lhs.

        public static TableauElement? operator *(TableauElement lhs, TableauElement rhs)
        {
            //BigMs should never be multiplied together in the tableau
            if (lhs.BigMCoefficient != 0 && rhs.BigMCoefficient != 0) throw new MathematicalError("BigM multiplication is not allowed");

            if (lhs.BigMCoefficient == 0)
            {
                return new TableauElement
                {
                    BigMCoefficient = lhs.Value * rhs.BigMCoefficient,
                    Value = lhs.Value * rhs.Value,
                    VariableBasis = lhs.VariableBasis,
                    Name = lhs.Name,
                };
            }
            else
            {
                return new TableauElement
                {
                    BigMCoefficient = lhs.BigMCoefficient * rhs.Value,
                    Value = lhs.Value * rhs.Value,
                    VariableBasis = lhs.VariableBasis,
                    Name = lhs.Name,
                };

            }
        }

        public static TableauElement operator /(TableauElement lhs, TableauElement rhs)
        {
            if (rhs.BigMCoefficient != 0) throw new MathematicalError("BigM Divison is not allowed");

            //Allow division by zero
            return new TableauElement
            {
                BigMCoefficient = rhs.Value == 0 ? //Nested Terneray operators deal with behaviour of dividing by zero
                lhs.BigMCoefficient==0? 0:lhs.BigMCoefficient :
                lhs.BigMCoefficient / rhs.Value,

                Value = rhs.Value <= 0 ? double.PositiveInfinity : lhs.Value / rhs.Value, //When dividing by less than zero it implies that Z-value not constrained

                VariableBasis = lhs.VariableBasis,
                Name = lhs.Name,
            };

        }

        public static TableauElement operator -(TableauElement operand)
        {
            return new TableauElement
            {
                BigMCoefficient = -operand.BigMCoefficient,
                Value = -operand.Value,
                VariableBasis = operand.VariableBasis,
                Name = operand.Name,
            };
        }

        public static bool operator == (TableauElement lhs, TableauElement rhs)
        {

            return lhs.CompareTo(rhs) == 0;
        }

        public static bool operator != (TableauElement lhs, TableauElement rhs)
        {
            return lhs.CompareTo(rhs) != 0;
        }

        public static bool operator < (TableauElement lhs, TableauElement rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }
        public static bool operator > (TableauElement lhs, TableauElement rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }
         //Define an implicit conversion between TableauEntries and doubles
        public static implicit operator TableauElement(double value)
        {
            return new TableauElement { Value= value };
        }




    }
}
