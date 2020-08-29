using HockeyApi.Common;
using HockeyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Queries
{
    public class PlayerService : IPlayerService
    {

		private readonly IDb _db;

		public PlayerService(IDb db)
		{
			_db = db;
		}

		public IEnumerable<PlayerModel> Search(string q)
		{
			var players = new HashSet<PlayerModel>();

			using (var conn = _db.CreateConnection())
			using (var cmd = conn.CreateCommand())
			{
				var playersSearchParam = cmd.CreateParameter();
				playersSearchParam.Value = q;
				playersSearchParam.ParameterName = "q";
				cmd.Parameters.Add(playersSearchParam);

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

	}
}
