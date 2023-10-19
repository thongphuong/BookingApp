﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service.Interface
{
    public interface IHistoryRepository : IGenericRepository<BookingService.Domain.History>
    {
        public IQueryable<BookingService.Domain.History> GetByCondition(Expression<Func<BookingService.Domain.History, bool>> expression);
    }
}
