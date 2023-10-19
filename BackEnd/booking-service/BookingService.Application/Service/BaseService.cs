using BookingService.Domain;
using BookingService.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class BaseService
    {
        public readonly IUnitOfWork _uom;
        public readonly ICurrentUserService _currentUser;
        protected BaseService(IUnitOfWork uom, ICurrentUserService currentUser)
        {
            _uom = uom;
            _currentUser = currentUser;
        }

        public async Task<bool> CheckPermissions(string controller, string action, string submenu = "")
        {
            try
            {
                int RoleID = _currentUser.User.RoleId ?? 0;
                var roleDetail = await _uom.Redis.GetAsync<List<RoleDetailDTO>>("ROLE_" + RoleID);
                var role = roleDetail.Where(x => x.RoleId == RoleID && x.Controller.ToLower() == controller.ToLower() && x.Action.ToLower() == action.ToLower() && (string.IsNullOrEmpty(submenu) || x.SubMenu == submenu)).FirstOrDefault();
                return role != null;
            }
            catch
            {
                return false;
            }
        }

        public async Task Log(string menu, string action, string msg, string status)
        {
            try
            {
                var log = new Log
                {
                    Menu = menu,
                    Action = action,
                    CreatedAt = DateTime.Now,
                    CreatedBy = _currentUser?.User?.Id.ToString() ?? "0",
                    CreatedName = _currentUser?.User?.UserName ?? "",
                    Msg = msg,
                    Status = status,
                };
                await _uom.Log.CreateAsync(log);
            }
            catch
            {

            }
        }
    }
}
