using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Common
{
    public class Paging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Paging"/> class.
        /// </summary>
        public Paging()
            : this(1, 10)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging" /> class.
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="pageSize">Number of elements per page.</param>
        public Paging(int page, int pageSize)
        {
            this.PageIndex = page > 0 ? page - 1 : 0;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging" /> class.
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="pageSize">Number of elements per page.</param>
        /// <param name="sort">The name of the property sorting should be applied to.</param>
        public Paging(int page, int pageSize, string sort)
            : this(page, pageSize)
        {
            this.SortColumn = sort;
        }


        /// <summary>
        /// Gets or sets the name of the property sorting should be applied to.
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        /// Gets or sets the current page index.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of elements per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "SortColumn: " + this.SortColumn + ", PageIndex: " + this.PageIndex + ", PageSize: " + this.PageSize;
        }
    }
}
