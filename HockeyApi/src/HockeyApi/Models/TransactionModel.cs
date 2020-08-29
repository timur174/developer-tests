using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Models
{
    public class TransactionModel
    {
        public TransactionModel(string team, string transactionType, DateTime effectiveDate)
        {
            Team = team;
            TransactionType = transactionType;
            EffectiveDate = effectiveDate;
        }

        public string Team { get; set; }
        public string TransactionType { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
