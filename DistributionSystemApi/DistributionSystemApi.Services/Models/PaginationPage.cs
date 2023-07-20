using System;

namespace DistributionSystemApi.DistributionSystemApi.Services.Models
{
    public class PaginationPage<T>
    {
        public List<T> Items { get; set; }

        public uint PageNumber { get; set; }

        public uint PageSize { get; set; }

        public uint TotalCount { get; set; }
    }
}
