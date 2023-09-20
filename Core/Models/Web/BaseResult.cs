namespace TYP.Angular.Core.Models.BaseResult
{
    //BaseResult classes- allows the returning of a value with an error message if necessary
    public class BaseResult
    {
        public BaseResult()
        {
        }
        public BaseResult(bool success, string errorMessage = "")
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsDuplicateCall { get; set; }
    }
    public class BaseResult<T> : BaseResult
    {
        public BaseResult()
        {
        }
        public BaseResult(T value) : base(true)
        {
            Result = value;
        }
        public BaseResult(bool success, string errorMessage = "") : base(success, errorMessage)
        {
            Success = success;
        }
        public T Result { get; set; }
    }


        
}
