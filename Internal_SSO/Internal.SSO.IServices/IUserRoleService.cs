using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.IServices
{
    public interface IUserRoleService
    {
        IEnumerable<UserRoleVM> GetAll(string userId);
        void Update(string userId, List<UserRole> entities);
    }
}
