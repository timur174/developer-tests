using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Models
{
    public class PlayerAssignCommand
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string TeamCode { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
