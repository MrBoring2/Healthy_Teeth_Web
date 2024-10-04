﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ServiceToVisit
    {
        public int VisitId {  get; set; }
        public virtual Visit? Visit { get; set; }
        public int ServiceId {  get; set; }
        public virtual Service? Service { get; set; }
        public required int Count {  get; set; }
      
       
    }
}
