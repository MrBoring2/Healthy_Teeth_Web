﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Role
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public required string Title { get; set; }
        public virtual List<Account>? Accounts { get; set; }
    }
}