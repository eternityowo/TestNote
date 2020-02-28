using System;
using System.Collections.Generic;
using System.Text;

namespace TestNote.Service.Contracts
{
    public interface IEntityGuidConverter
    {
        string ConvertToPrefixedGuid(Type entityType, Guid? guid);
        Guid ConvertToGuid(string prefixedGuid);
        bool TryConvertToGuid(string prefixedGuid, out Guid guid);
    }
}
