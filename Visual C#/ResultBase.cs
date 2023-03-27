using System;

namespace Namespace1
{
    public class ResultBase
    {
        public bool IsSuccessful { get; set; } = true;

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public virtual void Success(string message)
        {
            Message = message;

            IsSuccessful = true;
        }

        public virtual void Failure(Exception ex, string message = null)
        {
            Message = message;

            Exception = ex;

            IsSuccessful = false;
        }

        public virtual string GetErrorMessage()
        {
            string strException = null;

            if (Exception != null)
                strException = Exception.Message;

            return $"{strException} {Message}".Trim();
        }
    }
}
