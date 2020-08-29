using HockeyApi.Models;
using System.Collections.Generic;

namespace HockeyApi.Queries
{
    public interface IPlayerQueryService
    {
        IEnumerable<PlayerModel> Search(string q);
        PlayerTransactionsModel GetPlayerTransactions(int player_id);
    }
}