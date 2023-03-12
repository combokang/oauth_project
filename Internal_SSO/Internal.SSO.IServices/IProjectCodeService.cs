using Internal.SSO.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.IServices
{
    public interface IProjectCodeService
    {
        void Add(ProjectCode entity);
        ProjectCode GetByCode(string code);
    }
}
