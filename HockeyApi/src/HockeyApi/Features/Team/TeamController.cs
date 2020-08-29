using HockeyApi.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HockeyApi.Features {
	public class TeamController : Controller {
		private readonly ITeamQueryService _teamService;

		public TeamController(ITeamQueryService teamService) {
			_teamService = teamService;
		}

		public IActionResult Index() => 
			Json(_teamService.List());

		[HttpGet("team/{team_code}")]
		public IActionResult Players(string team_code)
		{
			var players = _teamService.GetPlayers(team_code);
			if(players == null)
			{
				return NotFound();
			}
			return Ok(players);
		}
	}
}
