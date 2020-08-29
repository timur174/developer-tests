using System.Collections.Generic;
using HockeyApi.Common;

namespace HockeyApi.Features {
	public interface ITeamService {
		IEnumerable<TeamModel> List();
	}

	public class TeamService : ITeamService {
		private readonly IDb _db;

		public TeamService(IDb db) {
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
	}
}
