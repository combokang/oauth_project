using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.IServices
{
    public interface IRolePermissionService
    {
        IEnumerable<RolePermissionVM> GetAllByRoleId(Guid roleId);
    }
}
