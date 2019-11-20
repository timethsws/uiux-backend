using System;
namespace Core.Model
{
    public class OperationResult
    {
        public bool Status { get; set; }
        public String Message { get; set; }
        public string AdditionalInformation { get; set;}

        public static OperationResult Failed(String message,String additionalInformation = null)
        {
            return new OperationResult
            {
                Status =false,
                Message = message,
                AdditionalInformation = additionalInformation,
            };
        }

        public static OperationResult Success()
        {
            return new OperationResult
            {
                Status = true,
                Message = String.Empty
            };
        }
    }
}
