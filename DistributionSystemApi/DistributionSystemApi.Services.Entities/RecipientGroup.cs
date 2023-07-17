﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributionSystemApi.Data.Entities
{
    public class RecipientGroup
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ICollection<Recipient>? Recipients { get; set; }
    }

}
