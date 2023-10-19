namespace Frontend.Utils
{
    using Frontend.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public static class Permission
    {
        private static IHttpContextAccessor? context;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor;
        }

        public static bool CheckPermissions(string controller, string action, string submenu = "")
        {
            var currentUser = context?.HttpContext?.Session.Get<CurrentUserModel>("UserInfo") ?? new CurrentUserModel();
            if (currentUser == null)
            {
                return false;
            }
            var list_permission = currentUser.roleDetailDTOs;
            var check = list_permission.Any(el => el.Controller == controller && (string.IsNullOrEmpty(action) || el.Action == action) && (string.IsNullOrEmpty(submenu) || el.SubMenu == submenu));
            return check;
        }
    }
}
