﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Service.Interface
{
    public interface ICurrentUserService
    {
        public UserDTO User { get; }
    }
}
