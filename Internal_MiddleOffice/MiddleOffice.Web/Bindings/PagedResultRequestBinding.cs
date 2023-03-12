using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings
{
    public class PagedResultRequestBinding
    {
        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public List<Order> Order { get; set; }

        public Column[] Columns { get; set; }

        public Search Search { get; set; }

        public int PageIndex
        {
            get
            {
                return (Start / Length) + 1;
            }
        }

        public string SortColumn
        {
            get
            {
                if (Columns != null && Order != null && Order.Count > 0)
                {
                    return Columns[Order[0].Column].Data;
                }

                return string.Empty;
            }
        }

        public string SortDir
        {
            get
            {
                if (Columns != null && Order != null && Order.Count > 0)
                {
                    return Order.First().Dir.ToUpper() == DTOrderDir.DESC.ToString() ? DTOrderDir.DESC.ToString() : string.Empty;
                }

                return string.Empty;
            }
        }
    }

    public class Order
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class Column
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public Search Search { get; set; }
    }

    public class Search
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public enum DTOrderDir
    {
        ASC,
        DESC
    }
}
