using HockeyApi.Models;
using System.Collections.Generic;
using System.Data;

namespace HockeyApi.Queries
{
    public interface IPlayerQueryService
    {
        IEnumerable<PlayerModel> Search(string q);
        PlayerTransactionsModel GetPlayerTransactions(int player_id);
        PlayerStatusModel GetPlayerStatus(int playerId, IDbConnection dbConnection = null);
    }
}