using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TableGenerator.Properties;

namespace TableGenerator.Models
{
    public class TableModel
    {
        public string TableName { get; set; }
        public string DefaultWhere { get; set; }
        public string JoinTable { get; set; }
        public string DefaultSort { get; set; }
        public string ControllerName { get; set; }
        public string Title { get; set; }
        public bool IsDeleteTable { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public string ButtonCreateText { get; set; }
        public int? ButtonCreateId { get; set; }
        public string BackController { get; set; }
        public int? BackId { get; set; }
        public bool IsEditable { get; set; }

        public List<ColumnModel> ColumnList { get; set; }
        public List<RowModel> RowList { get; set; }

        public int? FirstPageIndex { get; set; }
        public int? LastPageIndex { get; set; }
        public int? PrevPageIndex { get; set; }
        public int? NextPageIndex { get; set; }
        public List<PageModel> PageList { get; set; }

        public TableModel()
        {
            this.ButtonCreateText = Resources.BUTTON_CREATE;
            this.IsEditable = true;
            this.PageSize = Settings.Default.PAGE_SIZE;
            this.DefaultSort = "tt.[Id]";
        }
    }
}