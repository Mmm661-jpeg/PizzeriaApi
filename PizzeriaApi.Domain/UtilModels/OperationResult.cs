using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.UtilModels
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public T? Data { get; set; }

        public OperationResult(bool isSuccess,T? data,string message)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
        }

        public static OperationResult<T> Success(T? data,string message="Operation Successfull")
        {
            return new OperationResult<T>(isSuccess: true, data, message);
        }

        public static OperationResult<T> Failure(T? data,string message = "Operation failed")
        {
            return new OperationResult<T>(isSuccess: false, data, message);
        }

    }
}
