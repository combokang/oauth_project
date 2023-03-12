using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.IServices
{
    public interface IRoleService
    {
        IEnumerable<RoleListVM> GetAll(Guid projectId, int pageIndex, int pageSize);
        void Add(Role entity, List<RolePermission> rolePermissions);
        void Update(Role entity, List<RolePermission> rolePermissions);
        void Delete(Guid id);
        Role Get(Guid id);
    }
}
