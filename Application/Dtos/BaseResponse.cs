using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public  class BaseResponse<T>
    {
        public bool IsSuccessful { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public List<string> Errors { get; set; } = new();


        public static BaseResponse<T> Success(
       string message,
       T? data)
        {
            return new BaseResponse<T>
            {
                IsSuccessful = true,
                Message = message,
                Data = data
            };
        }


        public static BaseResponse<T> Failure(
        string message,
        List<string>? errors = null)
        {
            return new BaseResponse<T>
            {
                IsSuccessful = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

    }
}
