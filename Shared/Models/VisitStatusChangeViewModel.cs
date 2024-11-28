using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class VisitStatusChangeViewModel
    {
        public VisitStatusChangeViewModel(int id, int visitStatusId)
        {
            Id = id;
            VisitStatusId = visitStatusId;
        }

        public int Id { get; set; }
        public int VisitStatusId { get; set; }
    }
}
