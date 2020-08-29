using HockeyApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Queries
{
	public interface ITeamQueryService
	{
		IEnumerable<TeamModel> List();
		IEnumerable<TeamPlayersModel> GetPlayers(string team_code);
		HashSet<TeamPlayersModel> GetPlayersDetails(string team_code, IDbConnection dbConnection = null);
	}
}
