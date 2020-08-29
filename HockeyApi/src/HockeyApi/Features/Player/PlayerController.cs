using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HockeyApi.Commands;
using HockeyApi.Models;
using HockeyApi.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HockeyApi.Features.Player
{
    public class PlayerController : Controller
    {
        private readonly IPlayerQueryService _playerService;
        private readonly IPlayerCommandService _playerCommandService;
        private const string MODEL_IS_NOT_VALID = "Model is not valid";
        public PlayerController
        (
            IPlayerQueryService playerService,
            IPlayerCommandService playerCommandService
        )
        {
            _playerService = playerService;
            _playerCommandService = playerCommandService;
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

        [HttpPost("Player")]
        public IActionResult Create([FromBody]PlayerAssignCommand playerAssignModel)
        {
            var returnResult = new ReturnModel();
            if (ModelState.IsValid)
            {
                returnResult = _playerCommandService.AssignPlayer(playerAssignModel);
            }
            else
            {
                returnResult.IsSuccessfull = false;
                returnResult.Message = MODEL_IS_NOT_VALID;
            }
            return Ok(returnResult);
        }

        [HttpPost("player/{player_id}/injured")]
        public IActionResult Injure([FromBody]PlayerInjuryHealthCommand playerInjuryModel)
        {
            var returnResult = new ReturnModel();
            if (ModelState.IsValid)
            {
                returnResult = _playerCommandService.InjurePlayer(playerInjuryModel);
            }
            else
            {
                returnResult.IsSuccessfull = false;
                returnResult.Message = MODEL_IS_NOT_VALID;
            }
            return Ok(returnResult);
        }

        [HttpPost("player/{player_id}/healthy")]
        public IActionResult Heal([FromBody]PlayerInjuryHealthCommand playerHealthyModel)
        {
            var returnResult = new ReturnModel();
            if (ModelState.IsValid)
            {
                returnResult = _playerCommandService.HealPlayer(playerHealthyModel);
            }
            else
            {
                returnResult.IsSuccessfull = false;
                returnResult.Message = MODEL_IS_NOT_VALID;
            }
            return Ok(returnResult);
        }

        [HttpPost("player/{player_id}/trade")]
        public IActionResult Trade([FromBody]TradeCommand playerTradeModel)
        {
            var returnResult = new ReturnModel();
            if (ModelState.IsValid)
            {
                returnResult = _playerCommandService.TradePlayer(playerTradeModel);
            }
            else
            {
                returnResult.IsSuccessfull = false;
                returnResult.Message = MODEL_IS_NOT_VALID;
            }
            return Ok(returnResult);
        }
    }
}