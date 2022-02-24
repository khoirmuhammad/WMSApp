using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.CustomModels
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }
    public class SortingModel
    {
        private string UpSortIcon = "fa fa-arrow-up";
        private string DownSortIcon = "fa fa-arrow-down";
        public SortOrder SortedOrder { get; set; }
        public string SortedProperty { get; set; }
        public string SortExpression { get; private set; }

        private List<SortableColumn> SortableColumns = new List<SortableColumn>();

        public void SetColumn(string columnName, bool isDefaultColumn = false)
        {
            SortableColumn tmp = this.SortableColumns.Where(s => s.ColumnName.ToLower() == columnName.ToLower()).SingleOrDefault();

            if (tmp == null)
            {
                SortableColumns.Add(new SortableColumn { ColumnName = columnName });
            }

            if (isDefaultColumn || SortableColumns.Count.Equals(1))
            {
                SortedProperty = columnName;
                SortedOrder = SortOrder.Ascending;
            }
        }

        public SortableColumn GetColumn(string columnName)
        {
            SortableColumn tmp = this.SortableColumns.Where(s => s.ColumnName.ToLower() == columnName.ToLower()).SingleOrDefault();

            if (tmp == null)
            {
                SortableColumns.Add(new SortableColumn { ColumnName = columnName });
            }

            return tmp;
        }

        public void SortingParam(string sortExpression)
        {
            if (string.IsNullOrEmpty(sortExpression))
            {
                sortExpression = this.SortedProperty;
            }

            sortExpression = sortExpression.ToLower();
            SortExpression = sortExpression;

            foreach (SortableColumn sortableColumn in this.SortableColumns)
            {
                sortableColumn.SortIcon = string.Empty;
                sortableColumn.SortExpression = sortableColumn.ColumnName;

                if (sortExpression.Equals(sortableColumn.ColumnName))
                {
                    this.SortedOrder = SortOrder.Ascending;
                    this.SortedProperty = sortableColumn.ColumnName;

                    sortableColumn.SortExpression = $"{sortableColumn.ColumnName}_desc";
                    sortableColumn.SortIcon = DownSortIcon;
                }

                if (sortExpression.Equals($"{sortableColumn.ColumnName}_desc"))
                {
                    this.SortedOrder = SortOrder.Descending;
                    this.SortedProperty = sortableColumn.ColumnName;

                    sortableColumn.SortExpression = sortableColumn.ColumnName;
                    sortableColumn.SortIcon = UpSortIcon;
                }

            }

        }
    }

    public class SortableColumn
    {
        public string ColumnName { get; set; }
        public string SortExpression { get; set; }
        public string SortIcon { get; set; }
    }
}
