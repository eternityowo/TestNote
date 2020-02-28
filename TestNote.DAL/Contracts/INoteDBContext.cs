using Microsoft.EntityFrameworkCore;
using System;

namespace TestNote.DAL.Contracts
{
    public interface INoteDBContext : IDisposable
    {
        DbContext DbContext { get; }
    }
}
