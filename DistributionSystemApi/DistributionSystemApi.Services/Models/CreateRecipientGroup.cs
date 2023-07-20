using DistributionSystemApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributionSystemApi.Services.Models
{
    public class CreateRecipientGroup : BaseEntity
    {
        public string Title { get; set; }

        public List<Guid>? RecipientIds { get; set; }
    }
}
