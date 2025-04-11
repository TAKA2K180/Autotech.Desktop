using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.Helpers
{
    public class PagingHelper
    {
        public class PaginatedResponse<T>
        {
            public List<T> Items { get; set; }
            public int TotalCount { get; set; }
        }
    }
}
