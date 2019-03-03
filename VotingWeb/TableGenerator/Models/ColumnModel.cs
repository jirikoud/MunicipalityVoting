using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TableGenerator.Models
{
    public class ColumnModel
    {
        public string ColumnName { get; set; }
        public string Title { get; set; }
        public ColumnTypeEnum ColumnType { get; set; }
        public bool IsSortable { get; set; }
        public string ButtonTitle { get; set; }
        public string ButtonAction { get; set; }
        public string ButtonController { get; set; }

        public Func<int?, string> DecoderIntToString;
        public Func<string, string> DecoderStringToString;
        public Func<DateTime?, string> DecoderDateToString;
        public Func<bool?, string> DecoderBoolToString;

        public bool IsValueColumn()
        {
            return (ColumnType == ColumnTypeEnum.Integer
                || ColumnType == ColumnTypeEnum.String
                || ColumnType == ColumnTypeEnum.Boolean
                || ColumnType == ColumnTypeEnum.Date
                || ColumnType == ColumnTypeEnum.DecodeIntToString
                || ColumnType == ColumnTypeEnum.DecodeStringToString);
        }

        public bool IsStringColumn()
        {
            return (ColumnType == ColumnTypeEnum.String
                || ColumnType == ColumnTypeEnum.Boolean
                || ColumnType == ColumnTypeEnum.Date
                || ColumnType == ColumnTypeEnum.DecodeIntToString
                || ColumnType == ColumnTypeEnum.DecodeStringToString);
        }
    }
}