using AutoMapper;
using MiddleOffice.Entities.Models.Sso;
using MiddleOffice.Entities.ViewModels.Sso;
using MiddleOffice.Web.Bindings.Sso.Permissions;
using MiddleOffice.Web.Bindings.Sso.Projects;
using MiddleOffice.Web.Bindings.Sso.Users;
using MiddleOffice.Web.Bindings.Sso.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiddleOffice.Web.Bindings.Sso.UserRoles;

namespace MiddleOffice.Web
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {

            CreateMap<Project, EditProjectBinding>();

            CreateMap<Permission, EditPermissionBinding>();

            CreateMap<User, EditUserBinding>();

            CreateMap<Role, EditRoleBinding>();

            CreateMap<RolePermissionVM, EditRolePermissionBinding>();

            CreateMap<Permission, CreateRolePermissionBinding>()
              .ForMember(dest => dest.Checked, act => act.MapFrom(x => false))
              .ForMember(dest => dest.PermissionId, act => act.MapFrom(x => x.Id))
              .ForMember(dest => dest.PermissionCandidateKey, act => act.MapFrom(x => x.CandidateKey))
              .ForMember(dest => dest.PermissionName, act => act.MapFrom(x => x.Name))
              .ForMember(dest => dest.PermissionDescription, act => act.MapFrom(x => x.Description));

            CreateMap<UserRoleVM, UserRoleBinding>();
        }
    }
}
