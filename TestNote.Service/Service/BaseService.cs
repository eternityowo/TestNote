using System;
using System.Collections.Generic;
using System.Text;
using TestNote.DAL.Contracts;
using TestNote.Service.Contracts;

namespace TestNote.Service.Service
{
    public class BaseService
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IEntityGuidConverter EntityGuidConverter;

        public BaseService(IUnitOfWork unitOfWork, IEntityGuidConverter entityGuidConverter)
        {
            UnitOfWork = unitOfWork;
            EntityGuidConverter = entityGuidConverter;
        }

        protected IServiceResult EntityNotFoundResult<T>(string field, string fieldName = "id")
        {
            return new ServiceResult(string.Format("{0} with {1} : '{2}' is not found.", typeof(T).Name, fieldName, field));
        }

        protected IServiceResult EntityCanNotBeDeletedResult<T>(string reason)
        {
            return new ServiceResult(string.Format("{0} can not be removed. Reason: {1}", typeof(T).Name, reason));
        }

        protected IServiceResult SuccessResult()
        {
            return new ServiceResult();
        }

        protected IServiceResult SuccessResult(object resultEntry)
        {
            return new ServiceResult(resultEntry);
        }
    }
}
