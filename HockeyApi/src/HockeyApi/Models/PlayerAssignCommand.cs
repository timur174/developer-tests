using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Models
{
    public class PlayerAssignCommand
    {
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string teamCode { get; set; }
        public DateTime effectiveDate { get; set; }
    }
}
