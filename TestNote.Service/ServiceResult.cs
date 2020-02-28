using System;
using System.Collections.Generic;
using System.Text;

namespace TestNote.Service
{
    public class ServiceResult : IServiceResult
    {
        public bool Success
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }

        public object ResultEntry { get; private set; }

        public ServiceResult()
        {
            Success = true;
        }

        public ServiceResult(string errorMessage)
        {
            if (errorMessage == null)
                throw new ArgumentNullException("errorMessage");
            ErrorMessage = errorMessage;
        }

        public ServiceResult(object resultEntry)
        {
            Success = true;
            ResultEntry = resultEntry;
        }
    }
}
