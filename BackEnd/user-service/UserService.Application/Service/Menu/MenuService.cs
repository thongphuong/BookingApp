using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserService.Service.DTO;
using UserService.Service.Interface;

namespace UserService.Service
{


    public class MenuService : IMenuService
    {

        private readonly IUnitOfWork _uom;

        public MenuService(IUnitOfWork uom)
        {
            _uom = uom;
        }
        public async Task<ResponseMessage<List<MenuDTO>>> GetMenu(int roleId)
        {
            try
            {
                var role = await _uom.Role.FirstOrDefault(p => p.Id == roleId && p.Status == (int)Domain.Enum.Status.Active);
                if (role is null)
                    return new ResponseMessage<List<MenuDTO>>("", HttpStatusCode.OK, new List<MenuDTO>());
                var roleDetail = await _uom.RoleDetail.GetRoleDetaisById(roleId);
                var roleMenu = roleDetail.Where(p => p.Status == (int)Domain.Enum.Status.Active).Select(p => p.Menu);
                var subMenus = roleDetail.Where(p => p.Status == (int)Domain.Enum.Status.Active).Select(p => p.SubMenu ?? "").ToList() ?? new List<string>();
                var menus = await _uom.Menu.GetRoleMenuOrderByAsync(p => roleMenu.Contains(p.Name), subMenus);
                List<MenuDTO> menuDto = new List<MenuDTO>();
                foreach (var menu in menus)
                {
                    menuDto.Add(new MenuDTO
                    {
                        Id = menu.Id,
                        Controller = menu.Controller ?? "",
                        Icon = menu.Icon ?? "",
                        Name = menu.Name ?? "",
                        Order = menu.Order ?? 0,
                        LstMenuSiteAttr = menu.MenuAttributes.Select(p => new MenuSiteAttr
                        {
                            Id = p.Id,
                            Action = p.Action ?? "",
                            Controller = p.Controller ?? "",
                            MenuId = p.MenuId ?? 0,
                            Name = p.Name ?? "",
                            Order = p.Order ?? 0
                        }).ToList()
                    });
                }
                return new ResponseMessage<List<MenuDTO>>("", HttpStatusCode.OK, menuDto);
            }
            catch
            {
                return new ResponseMessage<List<MenuDTO>>("", HttpStatusCode.BadRequest, new List<MenuDTO>());

            }
        }
    }
}
