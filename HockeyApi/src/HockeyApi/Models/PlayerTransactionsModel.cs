using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Models
{
    public class PlayerTransactionsModel
    {
        public PlayerTransactionsModel()
        {
            Transactions = new List<TransactionModel>();
        }

        public PlayerModel Details { get; set; }
        public List<TransactionModel> Transactions { get; set; } 

    }
}
