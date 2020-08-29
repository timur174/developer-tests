using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Models
{
    public class PlayerTransactionsModel
    {
        public PlayerTransactionsModel(PlayerModel details, TransactionModel transactionModel)
        {
            Details = details;
            //if(Transactions)
            Transactions = new List<TransactionModel>();
        }

        public PlayerModel Details { get; set; }
        public List<TransactionModel> Transactions { get; set; } 

    }
}
