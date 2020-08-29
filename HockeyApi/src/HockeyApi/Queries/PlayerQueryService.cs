using HockeyApi.Common;
using HockeyApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Queries
{
    public class PlayerQueryService : IPlayerQueryService
    {

		private readonly IDb _db;

		public PlayerQueryService(IDb db)
		{
			_db = db;
		}

		public IEnumerable<PlayerModel> Search(string q)
		{
			var players = new HashSet<PlayerModel>();

			using (var conn = _db.CreateConnection())
			using (var cmd = conn.CreateCommand())
			{

				cmd.CreateParameter(q, "q");
	
				cmd.CommandText = @"
                    SELECT TOP 10
						first_name,
						last_name,
						player_id
					FROM player
					WHERE first_name LIKE '%'+@q+'%' OR last_name LIKE '%'+@q+ '%'";

				using (var rd = cmd.ExecuteReader())
				{
					while (rd.Read())
					{
						players.Add(
							new PlayerModel(
								rd.GetString(0),
								rd.GetString(1),
								rd.GetInt32(2)
								));
					}
				}
			}

			return players;
		}

		public PlayerStatusModel GetPlayerStatus(int playerId, IDbConnection dbConnection = null)
		{
			var playerStatusModel = new PlayerStatusModel();
			using (var cmd = dbConnection.CreateCommand())
			{
				cmd.CreateParameter(playerId, "PlayerId");

				cmd.CommandText = @"
						SELECT TOP 1 
							team_code,
							roster_transaction_type_id
						FROM
							roster_transaction rt
						WHERE
							rt.player_id = @PlayerId
						ORDER BY rt.effective_date DESC";
				using (var rd = cmd.ExecuteReader())
				{
					while (rd.Read())
					{
						playerStatusModel.TeamCode = rd.GetString(0);
						playerStatusModel.TransactionTypeId = rd.GetInt32(1);
					}
				}
			}

			return playerStatusModel;
		}

		public PlayerTransactionsModel GetPlayerTransactions(int player_id)
		{
			var playerTransactions = new PlayerTransactionsModel();

			using (var conn = _db.CreateConnection())
			using (var cmd = conn.CreateCommand())
			{
				var playersSearchParam = cmd.CreateParameter();
				playersSearchParam.Value = player_id;
				playersSearchParam.ParameterName = "player_id";
				cmd.Parameters.Add(playersSearchParam);

				cmd.CommandText = @"
                    SELECT TOP 10
						p.first_name,
						p.last_name,
						p.player_id,
						t.team_name,
						rtt.label,
						rt.effective_date
					FROM 
						player p left join roster_transaction rt ON p.player_id = rt.player_id
						left join team t on rt.team_code = t.team_code
						left join roster_transaction_type rtt ON rt.roster_transaction_type_id = rtt.roster_transaction_type_id
					WHERE
						p.player_id = @player_id
					ORDER BY
						effective_date DESC";

				using (var rd = cmd.ExecuteReader())
				{
					while (rd.Read())
					{
						var transaction = new {FirstName = rd.GetString(0), LastName = rd.GetString(1), PlayerId= rd.GetInt32(2), TeamName = rd.GetString(3), Label = rd.GetString(4), EffectiveDate = rd.GetDateTime(5) };
						if(playerTransactions.Details == null)
						{
							playerTransactions.Details = new PlayerModel(transaction.FirstName, transaction.LastName, transaction.PlayerId);
						}
						playerTransactions.Transactions.Add(new TransactionModel(transaction.TeamName, transaction.Label, transaction.EffectiveDate));
					}
				}
			}

			return playerTransactions;
		}

	}
}
