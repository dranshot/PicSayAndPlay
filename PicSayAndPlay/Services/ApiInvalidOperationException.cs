using System;

namespace PicSayAndPlay.Services
{
    public class ApiInvalidOperationException : Exception
    {
        public string AppExceptionMessage { get; set; }
        public string AppExceptionCode { get; set; }
        public ApiInvalidOperationExceptionType AppExceptionType { get; set; }
    }

    public enum ApiInvalidOperationExceptionType
    {
        Client = 1,
        ServerLogic = 2,
        ServerAuth = 3
    }
}