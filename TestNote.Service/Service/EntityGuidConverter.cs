using System;
using TestNote.Service.Contracts;

namespace TestNote.Service.Service
{
    public class EntityGuidConverter : IEntityGuidConverter
    {
        public string ConvertToPrefixedGuid(Type entityType, Guid? guid)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");

            if (guid == null)
                return null;

            string prefix = entityType.Name.Substring(0, 2).ToLower();
            return String.Format("{0}_{1}", prefix, guid);
        }

        public Guid ConvertToGuid(string prefixedGuid)
        {
            if (string.IsNullOrWhiteSpace(prefixedGuid))
                return Guid.Empty;

            Guid guid;
            TryConvertToGuid(prefixedGuid, out guid);
            return guid;
        }

        public bool TryConvertToGuid(string prefixedGuid, out Guid guid)
        {
            try
            {
                string rawGuid = prefixedGuid.Substring(3);
                return Guid.TryParse(rawGuid, out guid);
            }
            catch (ArgumentOutOfRangeException)
            {
                guid = Guid.Empty;
                return false;
            }
        }
    }
}
