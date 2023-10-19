using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookingService.Service;
using BookingService.Service.Interface;

namespace BookingService.Infrastructure
{
    public class CurrentUserService : ICurrentUserService
    {
        public UserDTO User { get; set; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var claim = httpContextAccessor.HttpContext?.Items["User"] ?? new UserDTO();
            User = (UserDTO)claim;
        }

    }
}
