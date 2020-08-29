using HockeyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Queries
{
	public interface ITeamService
	{
		IEnumerable<TeamModel> List();
	}
}
