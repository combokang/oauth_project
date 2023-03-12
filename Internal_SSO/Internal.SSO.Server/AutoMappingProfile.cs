using AutoMapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.Server.Bindings.Permissions;
using Internal.SSO.Server.Bindings.Projects;
using Internal.SSO.Server.Bindings.Roles;
using Internal.SSO.Server.Bindings.UserRoles;
using Internal.SSO.Server.Bindings.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            //Project
            CreateMap<CreateProjectBinding, Project>();
            CreateMap<EditProjectBinding, Project>();

            //Permission
            CreateMap<CreatePermissionBinding, Permission>();
            CreateMap<EditPermissionBinding, Permission>();

            //Role
            CreateMap<CreateRoleBinding, Role>();
            CreateMap<EditRoleBinding, Role>();

            //User
            CreateMap<CreateUserBinding, User>();
            CreateMap<EditUserBinding, User>();
            CreateMap<UnlockUserBinding, User>();

            //UserRole
            CreateMap<EditUserRoleBinding, UserRole>();
            CreateMap<UserRoleBinding, UserRole>();
        }
    }
}
