using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using TYP.Angular.Core.ExtensionMethods;
using TYP.Angular.Core.Models.LP;
using TYP.Angular.Core.Models.Simplex;

namespace TYP.Angular.Core.Models.MatrixModels
{

    public class Matrix<T> where T : Element, new()
    {
        private T[,] Values { get; set; }

        public T this[int x,int y]
        {
            get { return Values[x,y]; }
            set { Values[x,y] = (T) value.DeepClone(); }
        }

        public int Rows { get; set; }
        public int Columns { get; set; }

        public Matrix(int Rows, int Columns) 
        {
            Values=new T[Rows, Columns];
            this.Rows = Rows;
            this.Columns = Columns;

            for(int i=0; i<Rows; i++)
            {
                for(int j=0; j<Columns; j++)
                {
                    this[i, j] = new T();
                }
            }
        }

        public Matrix(List<List<T>> Elements)
        {
            this.Rows = Elements.Count;

            this.Columns = Elements.Max(i => i.Count);

            Values = new T[Rows, Columns];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] = Elements.ElementAtOrDefault(i)?.ElementAtOrDefault(j) ?? new T();
                        
                }
            }
        }
      
        public void InsertRange(int RowIndex,int RowIterations, int ColumnIndex, int ColumnIterations, Func<int,int, T> Predicate)
        {
            for (int i = RowIndex; i < RowIndex + RowIterations; i++)
            {
                for (int j = ColumnIndex; j < ColumnIndex + ColumnIterations; j++)
                {
                    this[i, j] = Predicate(i-RowIndex,j-ColumnIndex);
                }
            }
        }



        public Matrix<T> Transpose()
        {
           var Transposition = new Matrix<T>(Columns,Rows);

            for(int i=0; i < Transposition.Rows; i++)
            {
                for (int j = 0; j < Transposition.Columns; j++)
                {
                    Transposition[i,j] = this[j,i];
                }
            }



            return Transposition;
        }

        public Matrix<T> DeepClone() 
        {
            var newValues = new Matrix<T>(Rows, Columns);
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    newValues[i, j] = (T) Values[i, j].DeepClone();
                }
            }
            return newValues;
        }

        public T Min()
        {
            return this.ToList().Select(x => x.Min()).Min();
        }

        public T Max()
        {
            return this.ToList().Select(x => x.Max()).Max();
        }

        public T MinNonZero()
        {
            return this.ToList().Select(x => x.Where(i=>i.Value!=0).Min()).Min();
        }

        public T MaxNonZero()
        {
            return this.ToList().Select(x => x.Where(i => i.Value != 0).Max()).Max();
        }

        public IList<T> GetRow(int index, int start, int end)
        {
            //Return row for a given range
            return Enumerable.Range(start, end).Select(i => this[index, i]).ToList();
        }

        public IList<T> GetRow(int index)
        {
            //Return row for a given range
            return Enumerable.Range(0, Columns).Select(i => this[index, i]).ToList();
        }

        public Vector GetRowAsVector(int index)
        {
            var rowVector = new Vector(Columns);

            for(int i = 0; i < Columns; i++)
            {
                rowVector[i] = this[index,i];
            }

            return rowVector;
        }

        public (int, int)? Search(Func<T, bool> expression)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (expression(this[i,j]))
                    {
                        return (i,j);
                    }
                }
            }

            return null;
        }

        public IList<T> GetColumn(int index, int start, int end)
        {
            //Return column for a given range
            return Enumerable.Range(start, end).Select(i => Values[i, index]).ToList();
        }

        public IList<T> GetColumn(int index)
        {
            //Return row for a given range
            return Enumerable.Range(0, Rows).Select(i => this[i, index]).ToList();
        }

        public IList<IList<T>> ToList()
        {
            return Enumerable.Range(0, Columns).Select(i => GetColumn(i)).ToList();
        }

        public List<List<string>> ToStringList() 
        {
            return ToList().Select(i => i.Select(j => j.Value.ToString("0.###")).ToList()).ToList();
        }


        public static Matrix Identity(int dimension)
        {
            var identity=new Matrix(dimension, dimension);

            for(int i=0; i < dimension; i++)
            {
                for(int j=0; j < dimension; j++)
                {
                    if (i == j)
                    {
                        identity[i, j] = new T
                        {
                            Name = "Identity",
                            Value = 1,
                        };
                    } else
                    {
                        identity[i, j] = new T
                        {
                            Name = "Identity",
                            Value = 0,
                        };
                    }
                }
            }

            return identity;
        }

        

        
    }

    //Default BaseMatrix just has BaseMatrixElements as its elements
    public class Matrix : Matrix<Element>
    {

        public Matrix(int Rows, int Columns) : base(Rows, Columns){}

        public Matrix(List<List<Element>> Elements) : base(Elements) { }






        public static Matrix operator * (Matrix lhs, double rhs)
        {
            var NewMatrix=new Matrix(lhs.Rows, lhs.Columns);

            for(int i = 0; i < lhs.Rows; i++)
            {
                for(int j = 0; j < lhs.Columns; j++)
                {
                    NewMatrix[i, j] =lhs[i,j]* rhs;
                }
            }

            return NewMatrix;
        }





        

        public Matrix Transpose()
        {
            var Transposition = new Matrix(Columns, Rows);

            for (int i = 0; i < Transposition.Rows; i++)
            {
                for (int j = 0; j < Transposition.Columns; j++)
                {
                    Transposition[i, j] = this[j, i];
                }
            }


            return Transposition;
        }





    }
}
