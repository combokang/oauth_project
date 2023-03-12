using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Internal.SSO.IServices
{
    public interface IProjectService
    {
        IEnumerable<ProjectListVM> GetAll(string name, int pageIndex, int pageSize);
        IEnumerable<Project> GetAll();
        IEnumerable<ProjectInfoListVM> GetInfoAll();
        void Add(Project entity);
        void Update(Project entity);
        void Delete(Guid id);
        Project Get(Guid id);
        Project GetByClientId(string clientId);
        void Export(string name, Stream outStream);
    }
}
