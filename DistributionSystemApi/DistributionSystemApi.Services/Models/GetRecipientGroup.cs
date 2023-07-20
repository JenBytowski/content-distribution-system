using DistributionSystemApi.Data;
using DistributionSystemApi.DistributionSystemApi.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributionSystemApi.Services.Models
{
    public class GetRecipientGroup : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public List<RecipientRecipientGroupModel>? Recipients { get; set; }
    }
}
