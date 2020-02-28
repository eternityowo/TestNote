using System;
using System.Collections.Generic;
using System.Text;

namespace TestNote.Service
{
    public interface IServiceResult
    {
        bool Success { get; }
        string ErrorMessage { get; }
        object ResultEntry { get; }
    }
}
