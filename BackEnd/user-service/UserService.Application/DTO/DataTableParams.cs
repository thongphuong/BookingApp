using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Service
{
    public class DataTableParams
    {

        public string sEcho { get; set; } = string.Empty;
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iColumns { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; } = string.Empty;
        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; } = "asc";

    }

    public class DataTableResponse
    {
        public string sEcho { get; set; } = string.Empty;
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public IEnumerable<object> aaData { get; set; } = new List<object>();
    }
}
