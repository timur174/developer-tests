using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Models
{
    public class TradeCommand : PlayerInjuryHealthCommand
    {
        public string teamCode { get; set; }
    }
}
