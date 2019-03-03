using TableGenerator.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableGenerator
{
    public class Generator
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region --- Singleton ---

        private static Generator instance;

        public static Generator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Generator();
                }
                return instance;
            }
        }

        private Generator()
        {
        }

        #endregion

        private string GetWhere(List<string> whereList)
        {
            if (whereList.Count > 0)
            {
                return string.Format("WHERE {0}", string.Join(" AND ", whereList));
            }
            return null;
        }

        private int GetTotalCount(string connectionString, TableModel model, List<string> whereList)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT COUNT(*) FROM {0} tt {1} {2}", model.TableName, model.JoinTable, GetWhere(whereList));
                connection.Open();
                int totalCount = (int)command.ExecuteScalar();
                return totalCount;
            }
        }

        private void FillPages(string connectionString, TableModel model, List<string> whereList)
        {
            int totalCount = GetTotalCount(connectionString, model, whereList);
            int totalPages = (int)Math.Ceiling((double)totalCount / (double)model.PageSize);
            int maxPage = (totalPages > 0 ? totalPages - 1 : 0);
            if (model.PageIndex > maxPage)
            {
                model.PageIndex = maxPage;
            }
            model.FirstPageIndex = (model.PageIndex == 0 ? null : (int?)0);
            model.LastPageIndex = (model.PageIndex == maxPage ? null : (int?)maxPage);
            int prevIndex = model.PageIndex - 1;
            model.PrevPageIndex = (prevIndex >= 0 && prevIndex != model.PageIndex ? (int?)prevIndex : null);
            int nextIndex = model.PageIndex + 1;
            model.NextPageIndex = (nextIndex <= maxPage && nextIndex != model.PageIndex ? (int?)nextIndex : null);

            model.PageList = new List<PageModel>();
            for (int index = model.PageIndex - 2; index < model.PageIndex + 3; index++)
            {
                var pageModel = new PageModel() { PageIndex = index, Title = (index + 1).ToString(), IsCurrent = index == model.PageIndex };
                if (index >= 0 && index <= maxPage)
                {
                    model.PageList.Add(pageModel);
                }
            }
        }

        private string GetColumnQuery(TableModel model)
        {
            var columnQueryList = new List<string>();
            columnQueryList.Add("tt.[Id]");
            foreach (var column in model.ColumnList)
            {
                if (column.IsValueColumn())
                {
                    columnQueryList.Add(column.ColumnName);
                }
                else
                {
                    columnQueryList.Add("0");
                }
            }
            return string.Join(", ", columnQueryList);
        }

        private string GetIdent(SqlDataReader reader)
        {
            var fieldTypeId = reader.GetFieldType(0);
            if (fieldTypeId == typeof(int))
            {
                return reader.GetInt32(0).ToString();
            }
            if (fieldTypeId == typeof(string))
            {
                return reader.GetString(0);
            }
            throw new InvalidCastException("[Id] field is of unsupported type");
        }

        public void FillTable(string connectionString, TableModel model)
        {
            var whereList = new List<string>();
            if (!string.IsNullOrWhiteSpace(model.DefaultWhere))
            {
                whereList.Add(model.DefaultWhere);
            }
            if (model.IsDeleteTable)
            {
                whereList.Add("tt.[IsDeleted] = 0");
            }
            FillPages(connectionString, model, whereList);
            model.RowList = new List<RowModel>();
            using (var connection = new SqlConnection(connectionString))
            {
                int skipCount = model.PageSize * model.PageIndex;
                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT {0} FROM {1} tt {2} {3} ORDER BY {4} OFFSET {5} ROWS FETCH NEXT {6} ROWS ONLY",
                    GetColumnQuery(model),
                    model.TableName,
                    model.JoinTable,
                    GetWhere(whereList),
                    model.DefaultSort,
                    skipCount,
                    model.PageSize);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ident = GetIdent(reader);
                        var rowModel = new RowModel()
                        {
                            Ident = ident,
                            ValueList = new List<ValueModel>(),
                        };
                        for (int index = 0; index < model.ColumnList.Count; index++)
                        {
                            if (model.ColumnList[index].ColumnType == ColumnTypeEnum.String)
                            {
                                var valueModel = new ValueModel()
                                {
                                    StringValue = reader.IsDBNull(1 + index) ? null : reader.GetString(1 + index)
                                };
                                rowModel.ValueList.Add(valueModel);
                            }
                            else if (model.ColumnList[index].ColumnType == ColumnTypeEnum.Integer)
                            {
                                var valueModel = new ValueModel()
                                {
                                    IntValue = reader.IsDBNull(1 + index) ? (int?)null : reader.GetInt32(1 + index)
                                };
                                rowModel.ValueList.Add(valueModel);
                            }
                            else if (model.ColumnList[index].ColumnType == ColumnTypeEnum.Boolean)
                            {
                                var boolValue = reader.IsDBNull(1 + index) ? (bool?)null : reader.GetBoolean(1 + index);
                                var valueModel = new ValueModel()
                                {
                                    StringValue = model.ColumnList[index].DecoderBoolToString(boolValue)
                                };
                                rowModel.ValueList.Add(valueModel);
                            }
                            else if (model.ColumnList[index].ColumnType == ColumnTypeEnum.Date)
                            {
                                var dateTimeValue = reader.IsDBNull(1 + index) ? (DateTime?)null : reader.GetDateTime(1 + index);
                                var valueModel = new ValueModel()
                                {
                                    StringValue = model.ColumnList[index].DecoderDateToString(dateTimeValue)
                                };
                                rowModel.ValueList.Add(valueModel);
                            }
                            else if (model.ColumnList[index].ColumnType == ColumnTypeEnum.DecodeIntToString)
                            {
                                var intValue = reader.IsDBNull(1 + index) ? (int?)null : reader.GetInt32(1 + index);
                                var valueModel = new ValueModel()
                                {
                                    StringValue = model.ColumnList[index].DecoderIntToString(intValue)
                                };
                                rowModel.ValueList.Add(valueModel);
                            }
                            else if (model.ColumnList[index].ColumnType == ColumnTypeEnum.DecodeStringToString)
                            {
                                var stringValue = reader.IsDBNull(1 + index) ? null : reader.GetString(1 + index);
                                var valueModel = new ValueModel()
                                {
                                    StringValue = model.ColumnList[index].DecoderStringToString(stringValue)
                                };
                                rowModel.ValueList.Add(valueModel);
                            }
                            else
                            {
                                rowModel.ValueList.Add(null);
                            }
                        }
                        model.RowList.Add(rowModel);
                    }
                }
            }
        }
    }
}
