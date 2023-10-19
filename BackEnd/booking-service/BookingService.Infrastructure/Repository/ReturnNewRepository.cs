﻿using BookingService.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Infrastructure
{
    public class ReturnNewRepository : GenericRepository<Domain.Return>, IReturnNewRepository
    {
        public ReturnNewRepository(BookingDbContext context) : base(context)
        {

        }
    }
}
