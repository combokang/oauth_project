using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Internal.SSO.Entities.Enums.OAuths
{
    public enum LoginType
    {
        [Description("Active Directory(AD)")]
        AD = 0,

        [Description("Custom")]
        Custom = 1
    }
    public enum UserAccessLogAction
    {
        Login = 1,

        GetPermission = 2
    }
}
