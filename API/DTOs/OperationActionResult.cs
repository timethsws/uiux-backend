namespace Web.DTOs
{
    using System;
    using Core.Model;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;

    /// <summary>
    /// Operation action result.
    /// </summary>
    public static class OperationActionResult
    {
        
        public static OperationActionResult<TValue> Failed<TValue>(string messageCode)
        {
            return new OperationActionResult<TValue>
            {
                MessageCode = messageCode
            };
        }

        public static OperationActionResult<TValue> Success<TValue>(TValue value, string messageCode = null)
        {
            return new OperationActionResult<TValue>(value)
            {
                Status = true,
                MessageCode = messageCode
            };
        }
    }

    public class OperationActionResult<TValue> : IConvertToActionResult
    {

        internal OperationActionResult()
        {
            Status = false;
        }

        public OperationActionResult(TValue value)
        {
            if (typeof(IActionResult).IsAssignableFrom(typeof(TValue)))
            {
                throw new ArgumentException("value");
            }

            this.Value = value;
        }

        public OperationActionResult(ActionResult result)
        {
            if (typeof(IActionResult).IsAssignableFrom(typeof(TValue)))
            {
                throw new ArgumentException("value");
            }

            this.Result = result ?? throw new ArgumentNullException(nameof(result));
        }

        public ActionResult Result { get; }

        public TValue Value { get; }

        public string MessageCode { get; set; }

        public bool Status { get; set; }

        public int TotalCount { get; set; }

        public static implicit operator OperationActionResult<TValue>(TValue value)
        {
            return new OperationActionResult<TValue>(value) { Status = true };
        }

        public static implicit operator OperationActionResult<TValue>(ActionResult result)
        {
            return new OperationActionResult<TValue>(result);
        }

        IActionResult IConvertToActionResult.Convert()
        {
            if (Result != null)
            {
                return Result;
            }

            if (typeof(OperationResult).IsAssignableFrom(typeof(TValue)))
            {
                return new ObjectResult(this.Value)
                {
                    DeclaredType = typeof(TValue)
                };
            }

            var response = new OperationOutput<TValue>
            {
                Status = this.Status,
                Message = this.MessageCode,
                Value = this.Value,
            };

            return new ObjectResult(response)
            {
                DeclaredType = typeof(OperationOutput<TValue>)
            };
        }
    }
}