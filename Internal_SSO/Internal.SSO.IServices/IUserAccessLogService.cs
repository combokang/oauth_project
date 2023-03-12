using Internal.SSO.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.IServices
{
    public interface IUserAccessLogService
    {
        void Add(UserAccessLog entity);
    }
}
