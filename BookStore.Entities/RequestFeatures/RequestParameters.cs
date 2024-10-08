using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Entities.RequestFeatures
{
    public abstract class RequestParameters
    {
		const int maxPageSize = 50;

		// Auto-implemented property
        public int PageNumber { get; set; }

		// Full-property
		public int PageSize
        {
			get { return PageSize; }
			set { PageSize = value > maxPageSize ? maxPageSize : value; }
		}

	}
}
