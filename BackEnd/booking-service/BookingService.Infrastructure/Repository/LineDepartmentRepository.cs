using BookingService.Domain;
using BookingService.Service;
using BookingService.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Infrastructure
{
    public class LineDepartmentRepository : GenericRepository<LineDepartment>, ILineDepartmentRepository
    {
        public LineRepository Line;
        public LineDepartmentRepository(BookingDbContext context) : base(context)
        {
            Line = new LineRepository(context);
        }

        public async Task<List<SelectResponseDTO>> SelectLineDepartment(string query, Guid line)
        {
            //return await FindByCondition(p => p.LineReference == line && p.Status == (int)Domain.Enum.Status.Active && (string.IsNullOrEmpty(query) || p.Name.Contains(query))).Select(p => new SelectResponseDTO
            //{
            //    Key = p.ReferenceId.ToString(),
            //    Value = p.Code + '-' + p.Name
            //}).OrderBy(p => p.Value).ToListAsync();
            return await (from a in FindByCondition(p => (line == Guid.Empty || p.LineReference == line) && p.Status == (int)Domain.Enum.Status.Active && (string.IsNullOrEmpty(query) || p.Name.Contains(query) || p.Code.Contains(query)))
                          join b in Line.FindAll() on a.LineReference equals b.ReferenceId
                          select new SelectResponseDTO
                          {
                              Key = a.ReferenceId.ToString(),
                              Value = a.Code + " - " + a.Name,
                              listAttr = new List<SelectAttr>() { new SelectAttr
                              {
                                  Key ="line_value",
                                  Value = b.Code + " - " + b.Name
                              },new SelectAttr {
                                  Key ="line_reference",
                                  Value = b.ReferenceId.ToString()
                              },
                              new SelectAttr {
                                  Key ="priority",
                                  Value = a.Priority.ToString()
                              }}
                          }).ToListAsync();
        }
        public async Task<List<LineDepartment>> GetByConditionDepartmentTask(Expression<Func<LineDepartment, bool>> expression)
        {
            return await FindByCondition(expression).ToListAsync();
        }
    }
}
