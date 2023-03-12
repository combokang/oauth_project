using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.IServices
{
    public interface IPermissionService
    {
        IEnumerable<PermissionListVM> GetAll(Guid projectId, int pageIndex, int pageSize);
        IEnumerable<Permission> GetAllByProjectId(Guid projectId);
        void Add(Permission entity);
        void Update(Permission entity);
        void Delete(Guid id);
        Permission Get(Guid id);
    }
}
