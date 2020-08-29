using HockeyApi.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HockeyApi.Features {
	public class TeamController : Controller {
		private readonly ITeamService _service;

		public TeamController(ITeamService service) {
			_service = service;
		}

		public IActionResult Index() => 
			Json(_service.List());
	}
}
