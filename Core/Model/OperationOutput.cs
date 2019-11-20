using System;
namespace Core.Model
{
    public class OperationOutput<TOutput> : OperationResult
    {
        public TOutput Value;

        public static OperationOutput<TOutput> Success (TOutput value)
        {
            return new OperationOutput<TOutput>
            {
                Value = value,
                Status = true,
            };
        }

        new public static OperationOutput<TOutput> Failed(string message,string addtionalInformation = null)
        {
            return new OperationOutput<TOutput>
            {
                Status = false,
                Message = message,
                AdditionalInformation = addtionalInformation
            };
        }
    }
}
