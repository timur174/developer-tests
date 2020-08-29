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

        [HttpGet("Player/{player_id}")]
        public IActionResult Player(int player_id)
        {
            var playertransactions = _playerService.GetPlayerTransactions(player_id);
            if (playertransactions.Transactions.Count == 0)
            {
                return NotFound();
            }
            return Ok(playertransactions);
        }
    }
}