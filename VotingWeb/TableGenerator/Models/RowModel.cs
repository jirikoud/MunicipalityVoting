using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TableGenerator.Models
{
    public class RowModel
    {
        public string Ident { get; set; }
        public List<ValueModel> ValueList { get; set; }
    }
}