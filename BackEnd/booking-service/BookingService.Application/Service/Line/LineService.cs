using AutoMapper;
using BookingService.Domain;
using BookingService.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class LineService : ILineService
    {

        private readonly IUnitOfWork _uom;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        public LineService(IUnitOfWork uom, IMapper mapper, ICurrentUserService currentUser)
        {
            _uom = uom;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<ResponseMessage<List<SelectResponseDTO>>> DropDownLine(string query)
        {
            try
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.OK, await _uom.Line.SelectLine(query));
            }
            catch
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.BadRequest, new List<SelectResponseDTO>());
            }
        }

        public async Task<ResponseMessage<List<SelectResponseDTO>>> DropDownLineDepartment(string query, Guid l)
        {
            try
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.OK, await _uom.LineDepartment.SelectLineDepartment(query, l));
            }
            catch
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.BadRequest, new List<SelectResponseDTO>());
            }
        }
        public async Task<List<Line>> GetListLine(List<string?> lst_qurey)
        {
            try
            {
                Expression<Func<Line, bool>> query = (o => !string.IsNullOrEmpty(o.Code)
                && lst_qurey.Contains(o.Code));
                return await _uom.Line.GetByConditionLineTask(query);
            }
            catch (Exception)
            {
                return new List<Line>();
            }
        }
        public async Task<List<LineDepartment>> GetListDepartment(List<string?> lst_qurey)
        {
            try
            {
                Expression<Func<LineDepartment, bool>> query = (o => !string.IsNullOrEmpty(o.Code)
                && lst_qurey.Contains(o.Code));
                return await _uom.LineDepartment.GetByConditionDepartmentTask(query);
            }
            catch (Exception)
            {
                return new List<LineDepartment>();
            }
        }

        public async Task<ResponseMessage<LineDownloadDTO>> DownloadLine(List<LineDownloadDTO> lst_param, List<LineDownloadDTO> lst_param_new, List<Line> entitys)
        {

            try
            {
                var lst_line = new List<Line>();
                foreach (var item_param in lst_param_new)
                {
                    if (lst_line.Any(a => a.Code == item_param.Code))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(item_param.Code) || string.IsNullOrEmpty(item_param.Name))
                    {
                        return new ResponseMessage<LineDownloadDTO>("Code/Name is empty !!!", HttpStatusCode.BadRequest, new LineDownloadDTO());
                    }
                    var line = _mapper.Map<LineDownloadDTO, Line>(item_param);
                    line.CreatedAt = DateTime.Now;
                    //line.CreatedBy = _currentUser.User.Id;
                    line.Status = item_param.Status ?? (int)Domain.Enum.Status.Active;
                    line.ReferenceId = Guid.NewGuid();
                    lst_line.Add(line);
                }

                var lst_line_edit = new List<Line>();
                foreach (var item_param in entitys)
                {
                    var record_temp = lst_param.FirstOrDefault(a => a.Code == item_param.Code);
                    if (record_temp != null)
                    {
                        if (string.IsNullOrEmpty(record_temp.Code) || string.IsNullOrEmpty(record_temp.Name))
                        {
                            return new ResponseMessage<LineDownloadDTO>("Code/Name is empty !!!", HttpStatusCode.BadRequest, new LineDownloadDTO());
                        }
                        item_param.Name = record_temp.Name;
                        item_param.ModifiedAt = DateTime.Now;
                        //item_param.ModifiedBy = _currentUser.User.Id;
                        item_param.Status = record_temp.Status ?? (int)Domain.Enum.Status.Active; //(int)Domain.Enum.Status.Active;
                        lst_line_edit.Add(item_param);
                    }
                }

                await _uom.BeginTransactionAsync();
                _uom.Line.CreateRange(lst_line);
                _uom.Line.UpdateRange(lst_line_edit);
                await _uom.SaveAsync();
                await _uom.CommitAsync();

                return new ResponseMessage<LineDownloadDTO>("Download Success", HttpStatusCode.OK, new LineDownloadDTO());

            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                return new ResponseMessage<LineDownloadDTO>("Download Error", HttpStatusCode.BadRequest, new LineDownloadDTO());
            }
        }
        public async Task<ResponseMessage<DepartmentDownloadDTO>> DownloadLineDepartment(List<Line> line_entitys, List<DepartmentDownloadDTO> lst_param, List<DepartmentDownloadDTO> lst_param_new, List<LineDepartment> entitys)
        {

            try
            {
                var lst_linedepart = new List<LineDepartment>();
                foreach (var item_param in lst_param_new)
                {
                    if (string.IsNullOrEmpty(item_param.line_Code))
                    {
                        return new ResponseMessage<DepartmentDownloadDTO>("LineCode is empty !!!", HttpStatusCode.BadRequest, new DepartmentDownloadDTO());
                    }
                    if (string.IsNullOrEmpty(item_param.Code) || string.IsNullOrEmpty(item_param.Name))
                    {
                        return new ResponseMessage<DepartmentDownloadDTO>("Code/Name is empty !!!", HttpStatusCode.BadRequest, new DepartmentDownloadDTO());
                    }
                    var line_item = line_entitys.FirstOrDefault(x => x.Code == item_param.line_Code);
                    var line_depart = _mapper.Map<DepartmentDownloadDTO, LineDepartment>(item_param);

                    if (line_item != null)
                    {
                        line_depart.CreatedAt = DateTime.Now;
                        //line_depart.CreatedBy = _currentUser.User.Id;
                        line_depart.Status = item_param.Status ?? (int)Domain.Enum.Status.Active;
                        line_depart.LineReference = (Guid)line_item.ReferenceId;
                        line_depart.ReferenceId = Guid.NewGuid();
                        lst_linedepart.Add(line_depart);
                    }
                }

                var lst_line_edit = new List<LineDepartment>();
                foreach (var item_param in entitys)
                {

                    var record_temp = lst_param.FirstOrDefault(a => a.Code == item_param.Code);
                    if (record_temp != null)
                    {
                        if (string.IsNullOrEmpty(record_temp.line_Code))
                        {
                            return new ResponseMessage<DepartmentDownloadDTO>("LineCode is empty !!!", HttpStatusCode.BadRequest, new DepartmentDownloadDTO());
                        }
                        if (string.IsNullOrEmpty(record_temp.Code) || string.IsNullOrEmpty(record_temp.Name))
                        {
                            return new ResponseMessage<DepartmentDownloadDTO>("Code/Name is empty !!!", HttpStatusCode.BadRequest, new DepartmentDownloadDTO());
                        }
                        var line_item = line_entitys.FirstOrDefault(x => x.Code == record_temp.line_Code);
                        if (line_item != null)
                        {
                            item_param.Name = record_temp.Name;
                            item_param.ModifiedAt = DateTime.Now;
                            item_param.LineReference = (Guid)line_item.ReferenceId;
                            //item_param.ModifiedBy = _currentUser.User.Id;
                            item_param.Status = record_temp.Status ?? (int)Domain.Enum.Status.Active;//(int)Domain.Enum.Status.Active;
                            lst_line_edit.Add(item_param);
                        }
                    }
                }

                await _uom.BeginTransactionAsync();
                _uom.LineDepartment.CreateRange(lst_linedepart);
                _uom.LineDepartment.UpdateRange(lst_line_edit);
                await _uom.SaveAsync();
                await _uom.CommitAsync();

                return new ResponseMessage<DepartmentDownloadDTO>("Download Success", HttpStatusCode.OK, new DepartmentDownloadDTO());

            }
            catch (Exception)
            {
                await _uom.RollBackAsync();
                return new ResponseMessage<DepartmentDownloadDTO>("Download Error", HttpStatusCode.BadRequest, new DepartmentDownloadDTO());
            }
        }

        public async Task<List<Line>> GetListLineEntity()
        {
            try
            {
                Expression<Func<Line, bool>> query = (o => o.Status != (int)Domain.Enum.Status.Delete);
                return await _uom.Line.GetByConditionLineTask(query);
            }
            catch (Exception)
            {
                return new List<Line>();
            }
        }
    }
}
