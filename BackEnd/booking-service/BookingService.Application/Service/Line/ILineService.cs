using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public interface ILineService
    {
        public Task<ResponseMessage<List<SelectResponseDTO>>> DropDownLine(string query);
        public Task<ResponseMessage<List<SelectResponseDTO>>> DropDownLineDepartment(string query, Guid line);

        public Task<List<Line>> GetListLine(List<string?> lst_qurey);

        public Task<List<Line>> GetListLineEntity();
        public Task<List<LineDepartment>> GetListDepartment(List<string?> lst_qurey);
        Task<ResponseMessage<LineDownloadDTO>> DownloadLine(List<LineDownloadDTO> lst_param, List<LineDownloadDTO> lst_param_new, List<Line> entitys);
        Task<ResponseMessage<DepartmentDownloadDTO>> DownloadLineDepartment(List<Line> line_entitys, List<DepartmentDownloadDTO> lst_param, List<DepartmentDownloadDTO> lst_param_new, List<LineDepartment> entitys);
    }
}
