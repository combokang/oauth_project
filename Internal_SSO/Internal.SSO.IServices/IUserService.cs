using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Internal.SSO.IServices
{
    public interface IUserService
    {
        IEnumerable<UserListVM> GetAll(string id, int pageIndex, int pageSize);
        bool? Validate(string id, string password);
        bool CheckPermission(string id, string clientId, string permissionKey);
        void Unlock(string id);
        void Add(User entity);
        User Get(string id);
        bool Exists(string id);
        void Delete(string id);
        void Update(User entity);
        void Export(string id, Stream outStream);
    }
}
