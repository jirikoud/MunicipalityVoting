using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableGenerator.Models
{
    public class PageModel
    {
        public string Title { get; set; }
        public int PageIndex { get; set; }
        public bool IsCurrent { get; set; }
    }
}
