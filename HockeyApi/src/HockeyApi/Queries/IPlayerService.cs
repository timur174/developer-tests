﻿using HockeyApi.Models;
using System.Collections.Generic;

namespace HockeyApi.Queries
{
    public interface IPlayerService
    {
        IEnumerable<PlayerModel> Search(string q);
        PlayerTransactionsModel GetPlayerTransactions(int player_id);
    }
}