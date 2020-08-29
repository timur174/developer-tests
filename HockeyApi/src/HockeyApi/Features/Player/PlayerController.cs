using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HockeyApi.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HockeyApi.Features.Player
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        [HttpGet("Player")]
        public IActionResult Search(string q)
        {
            var players = _playerService.Search(q);
            if(players == null)
            {
                return NotFound();
            }
            return Ok(players);
        }
    }
}