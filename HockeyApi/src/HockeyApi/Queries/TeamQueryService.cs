using System;
using System.Collections.Generic;
using System.Data;
using HockeyApi.Common;
using HockeyApi.Models;

namespace HockeyApi.Queries {


	public class TeamQueryService : ITeamQueryService {
		private readonly IDb _db;

		public TeamQueryService(IDb db) {
			_db = db;
		}

		public IEnumerable<TeamModel> List() {
			var teams = new HashSet<TeamModel>();

			using (var conn = _db.CreateConnection())
			using (var cmd = conn.CreateCommand()) {
				cmd.CommandText = @"
                    SELECT      team_code
                              , team_name
                    FROM        team";

				using (var rd = cmd.ExecuteReader()) {
					while (rd.Read()) {
						teams.Add(
							new TeamModel(
								rd.GetString(0),
								rd.GetString(1)));
					}
				}
			}

			return teams;
		}

		public IEnumerable<TeamPlayersModel> GetPlayers(string team_code, IDbConnection dbConnection = null)
		{
			var teamPlayers = new HashSet<TeamPlayersModel>();

			using (var conn = dbConnection ?? _db.CreateConnection())
			{
				using (var cmd = conn.CreateCommand())
				{
					var teamCodeParam = cmd.CreateParameter();
					teamCodeParam.Value = team_code;
					teamCodeParam.ParameterName = "team_code";
					cmd.Parameters.Add(teamCodeParam);
					cmd.CommandText = @"
					SELECT
						first_name,
						last_name,
						team_name,
						rt.label as CurrentStatus,
						IsActive
					FROM player p left join 
					(
					SELECT 
						t1.player_id, 
						rtt.label, 
						t2.maxEffectiveDate, 
						t1.team_code,
					CASE WHEN t1.roster_transaction_type_id = 2 THEN 0 ELSE 1 END AS IsActive
					FROM
					(
						SELECT 
							Player_id, 
							roster_transaction_type_id, 
							effective_date,
							team_code
						FROM 
							roster_transaction rt 
					) t1 inner join
					(
						SELECT 
							Player_id, 
							Max(effective_date) AS maxEffectiveDate 
						FROM 
							roster_transaction rt 
						GROUP BY 
							player_id
					) t2 ON t1.player_id = t2.player_id and t1.effective_date = t2.maxEffectiveDate
					left join roster_transaction_type rtt ON t1.roster_transaction_type_id = rtt.roster_transaction_type_id
					) rt
					ON p.player_id = rt.player_id
					left join team t ON rt.team_code = t.team_code
					where rt.team_code = @team_code";

					using (var rd = cmd.ExecuteReader())
					{
						while (rd.Read())
						{
							teamPlayers.Add(
								new TeamPlayersModel(
									rd.GetString(0),
									rd.GetString(1),
									rd.GetString(2),
									rd.GetString(3),
									rd.GetInt32(4)));
						}
					}
				}
			}

			return teamPlayers;
		}
	}
}
