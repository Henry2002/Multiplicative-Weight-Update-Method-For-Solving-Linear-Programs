using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using TYP.Angular.Core.Extensions;
using TYP.Angular.Core.Models.LP;
using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Models.MatrixModels
{
    public class Vector<T> where T: Element, new()
    {
        private T[] Values;

        public int Size;

        //Indexer for the vector class
        public T this[int index]
        {
            get { return Values[index]; }
            set { Values[index] = (T) value.DeepClone(); }
        }

        public Vector(int size)
        {
            Values = new T[size];
            Size = size;

            for(int i=0; i < Size; i++)
            {
                this[i] = new T();
            }
        }

        public Vector(IList<T> Elements)
        {
            Values=new T[Elements.Count];
            Size=Elements.Count;

            for(int i=0; i<Size; i++)
            {
                this[i] = Elements[i];
            }
        }

        public IList<T> ToList()
        {
            var list = new List<T>();

            for(int i=0; i<Size; i++)
            {
                list.Add(this[i]);
            }

            return list;
        }

        public void InsertRange(int Index, int Iterations, Func<int, T> predicate)
        {
            for (int i = Index; i < Iterations + Index; i++)
            {
                this[i] = predicate(i - Index);

            }
        }

        

        public Vector<T> DoToAll(ActionReference<T> Action)
        {
            for(int i = 0; i < Size; i++)
            {
                var elementRef= Values[i];

                Action(ref elementRef,i);

                this[i] = (T) elementRef.DeepClone();
            }

            return this;
        }

       
        //Merge with predicate but capture the index
        
     

        public T Min()
        {
            return this.ToList().Min();
        }

        public T MinNonZero()
        {
            return this.ToList().Where(i => i.Value != 0).Min();
        }

        public T Max()
        {
            return this.ToList().Max();
        }

        public T MaxNonZero()
        {
            return this.ToList().Where(i => i.Value != 0).Max();
        }

       

        public Vector<T> DeepClone()
        {
            var newValues = new Vector<T>(Size);
            for (var i = 0; i < Size; i++)
            {
                    newValues[i] = (T)this[i].DeepClone();
            }
            return newValues;
        }

        public Vector<T> Zip(Vector<T> Vector2, Func<T, T, T> operation)
        {
            Vector<T> minVector = this;

            Vector<T> maxVector = Vector2;

            if (Size >= Vector2.Size)
            {
                minVector = Vector2;
                maxVector = this;
            }

            Vector<T> resultVector = new Vector<T>(minVector.Size);


            for (int i = 0; i < minVector.Size; i++)
            {
                resultVector[i] = operation(minVector[i], maxVector[i]);
            }

            return resultVector;

        }

        public double L1Norm()
        {
            double Norm = 0;

            for (int i = 0; i < Size; i++)
            {
                Norm += Math.Abs(this[i].Value);
            }

            return Norm;
        }

        public double Sum()
        {
            double Sum = 0;

            for (int i = 0; i < Size; i++)
            {
                Sum+= this[i].Value;
            }

            return Sum;
        }

        public static double DotProduct(Vector lhs, Vector rhs)
        {
            if (lhs.Size != rhs.Size)
            {
                throw new ArgumentException("Vectors must be same size to perform DotProduct");
            }

            return lhs.DeepClone().Zip(rhs.DeepClone(), (a, b) => a * b).Sum();
        }
    }





    public class Vector : Vector<Element>
    {
        public Vector(int size) : base(size){}

        public Vector(IList<Element> elements) : base(elements) { }

        public static explicit operator Vector(Matrix matrix)
        {
            if (matrix.Rows > 1) throw new FormatException();

            var vector = new Vector(matrix.Columns);

            for (int i = 0; i < matrix.Columns; i++)
            {
                vector[i] = matrix[0, i];
            }

            return vector;
        }

        public static Vector Identity(int dimension)
        {
            var identity=new Vector(dimension);

            for(int i=0; i < dimension; i++)
            {
                identity[i].Value = 1;
            }

            return identity;
        }

        public static Vector operator / (Vector lhs, double rhs)
        {
            var result = new Vector(lhs.Size);

            for(int i = 0; i < lhs.Size; i++)
            {
                result[i].Value = lhs[i].Value / rhs;
            }

            return result;

        }

        public static Vector operator * (Vector lhs, double rhs)
        {
            var result = new Vector(lhs.Size);

            for (int i = 0; i < lhs.Size; i++)
            {
                result[i].Value = lhs[i].Value * rhs;
            }

            return result;

        }

        public static Vector operator +(Vector lhs, Vector rhs)
        {
            var result = new Vector(lhs.Size);

            return (Vector)lhs.Zip(rhs, (a, b) =>
            {
                return a + b;
            });
        }

       
    }
}
