using HockeyApi.Common;
using HockeyApi.Models;
using HockeyApi.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Commands
{
    public class PlayerCommandService : IPlayerCommandService
    {
		private readonly IDb _db;
		private readonly ITeamQueryService _teamQueryService;
		private readonly IPlayerQueryService _playerQueryService;
		private const string TEAM_DOES_NOT_EXIST = "Error: Team with entered team_code does not exist";
		private const string TEAM_HAS_MORE_THAN_10_PLAYERS = "Error: Team has more than 10 players";
		private const string TEAM_HAS_LESS_THAN_4_PLAYERS = "Error: Team has less than 4 players";
		private const string UNABLE_TO_INSERT_A_RECORD = "Error: Unable to insert the table";
		private const string PLAYER_IS_INJURED_ALREADY = "Error: Player is already injured";
		private const string PLAYER_IS_NOT_INJURED = "Error: Player is not currently injured and cannot be healed";
		private const string PLAYER_HAS_NEVER_BEEN_SIGNED = "Error: Player has never been signed";


		private const string SUCCESSFULLY_ASSIGNED = "Success: Player assigned to a team successfully";
		private const string SUCCESSFULLY_INJURED = "Success: Player has been successfully injured";
		private const string SUCCESSFULLY_HEALED = "Success: Player has been successfully healed";
		private const string SUCCESSFULLY_TRADED = "Success: Player has been successfully traded";
		public PlayerCommandService
		(
			IDb db,
			ITeamQueryService teamQueryService,
			IPlayerQueryService playerQueryService 

		)
		{
			_db = db;
			_teamQueryService = teamQueryService;
			_playerQueryService = playerQueryService;
		}

		public ReturnModel AssignPlayer(PlayerAssignCommand playerAssignCommand)
		{
			var returnModel = new ReturnModel();
			var team_code = playerAssignCommand.teamCode;
			using (var conn = _db.CreateConnection())
			{
				// 1) Validate if team exist
				using (var cmd = conn.CreateCommand())
				{
					cmd.CreateParameter(team_code, "TeamCode");

					cmd.CommandText = @"
                    SELECT 
						team_code
					FROM 
						team
					WHERE 
						team_code = @TeamCode;";
					var isTeamExist = false;
					using (var rd = cmd.ExecuteReader())
					{
						while (rd.Read())
						{
							isTeamExist = true;
							break;
						}
					}

					if (!isTeamExist)
					{
						returnModel.IsSuccessfull = false;
						returnModel.Message = TEAM_DOES_NOT_EXIST;
						return returnModel;
					}
				}

				// 2) Validate if team has more than 10 active players
				var players = _teamQueryService.GetPlayersDetails(team_code, conn);
				var totalActivePlayersInTeam = players.Where(p => p.IsActive).Count();
				if(totalActivePlayersInTeam >= 12)
				{
					returnModel.IsSuccessfull = false;
					returnModel.Message = TEAM_HAS_MORE_THAN_10_PLAYERS;
					return returnModel;
				}

				// 3) Insert value
				using (var cmdInsert = conn.CreateCommand())
				{
					cmdInsert.CreateParameter(team_code, "TeamCode");
					cmdInsert.CreateParameter(playerAssignCommand.firstName, "FirstName");
					cmdInsert.CreateParameter(playerAssignCommand.lastName, "LastName");
					cmdInsert.CreateParameter(playerAssignCommand.effectiveDate, "EffectiveDate");

					cmdInsert.CommandText = @"
						INSERT INTO player(first_name, last_name)
						VALUES(@FirstName, @LastName)

						Declare @id int = (select SCOPE_IDENTITY())

						INSERT INTO roster_transaction(roster_transaction_type_id, player_id, team_code, effective_date)
						VALUES(1, @id, @TeamCode, @EffectiveDate)";

					var result = cmdInsert.ExecuteNonQuery();
					if(result != 2)
					{
						returnModel.IsSuccessfull = false;
						returnModel.Message = UNABLE_TO_INSERT_A_RECORD;
						return returnModel;
					}
				}
			}
			returnModel.IsSuccessfull = true;
			returnModel.Message = SUCCESSFULLY_ASSIGNED;
			return returnModel;
		}

		public ReturnModel InjurePlayer(PlayerInjuryHealthCommand playerInjuryCommand)
		{
			var returnModel = new ReturnModel();
			using (var conn = _db.CreateConnection())
			{
				// 1) Get team code and check player's last stataus
				var playerStatus = _playerQueryService.GetPlayerStatus(playerInjuryCommand.playerId, conn);
				if(playerStatus.TransactionTypeId == (int)PlayerStatusEnum.Injured)
				{
					returnModel.IsSuccessfull = false;
					returnModel.Message = PLAYER_IS_INJURED_ALREADY;
					return returnModel;
				}

				// 2) Validate if team has less than 4 players
				var players = _teamQueryService.GetPlayersDetails(playerStatus.TeamCode, conn);
				var totalActivePlayersInTeam = players.Where(p => p.IsActive).Count();
				if (totalActivePlayersInTeam < 5)
				{
					returnModel.IsSuccessfull = false;
					returnModel.Message = TEAM_HAS_LESS_THAN_4_PLAYERS;
					return returnModel;
				}

				// 3) Insert value
				using (var cmdInsert = conn.CreateCommand())
				{
					cmdInsert.CreateParameter(playerInjuryCommand.playerId, "PlayerId");
					cmdInsert.CreateParameter(playerStatus.TeamCode, "TeamCode");
					cmdInsert.CreateParameter(playerInjuryCommand.effectiveDate, "EffectiveDate");

					cmdInsert.CommandText = @"
						INSERT INTO roster_transaction(roster_transaction_type_id, player_id, team_code, effective_date)
						VALUES(2, @PlayerId, @TeamCode, @EffectiveDate)
						";

					var result = cmdInsert.ExecuteNonQuery();
					if (result != 1)
					{
						returnModel.IsSuccessfull = false;
						returnModel.Message = UNABLE_TO_INSERT_A_RECORD;
						return returnModel;
					}
				}
			}
			returnModel.IsSuccessfull = true;
			returnModel.Message = SUCCESSFULLY_INJURED;
			return returnModel;
		}


		public ReturnModel HealPlayer(PlayerInjuryHealthCommand playerHealthyCommand)
		{
			var returnModel = new ReturnModel();
			using (var conn = _db.CreateConnection())
			{
				// 1) Get team code and check player's last stataus
				var playerStatus = _playerQueryService.GetPlayerStatus(playerHealthyCommand.playerId, conn);
				if (playerStatus.TransactionTypeId != (int)PlayerStatusEnum.Injured)
				{
					returnModel.IsSuccessfull = false;
					returnModel.Message = PLAYER_IS_NOT_INJURED;
					return returnModel;
				}

				// 2) Validate if team has more than 10 players
				var players = _teamQueryService.GetPlayersDetails(playerStatus.TeamCode, conn);
				var totalActivePlayersInTeam = players.Where(p => p.IsActive).Count();
				if (totalActivePlayersInTeam >= 10)
				{
					returnModel.IsSuccessfull = false;
					returnModel.Message = TEAM_HAS_MORE_THAN_10_PLAYERS;
					return returnModel;
				}

				// 3) Insert value
				using (var cmdInsert = conn.CreateCommand())
				{
					cmdInsert.CreateParameter(playerHealthyCommand.playerId, "PlayerId");
					cmdInsert.CreateParameter(playerStatus.TeamCode, "TeamCode");
					cmdInsert.CreateParameter(playerHealthyCommand.effectiveDate, "EffectiveDate");

					cmdInsert.CommandText = @"
						INSERT INTO roster_transaction(roster_transaction_type_id, player_id, team_code, effective_date)
						VALUES(3, @PlayerId, @TeamCode, @EffectiveDate)
						";

					var result = cmdInsert.ExecuteNonQuery();
					if (result != 1)
					{
						returnModel.IsSuccessfull = false;
						returnModel.Message = UNABLE_TO_INSERT_A_RECORD;
						return returnModel;
					}
				}
			}
			returnModel.IsSuccessfull = true;
			returnModel.Message = SUCCESSFULLY_HEALED;
			return returnModel;
		}

		public ReturnModel TradePlayer(TradeCommand playerTradeCommand)
		{
			var returnModel = new ReturnModel();
			using (var conn = _db.CreateConnection())
			{
				// 1) Validate if a new team has more than 10 players
				var players = _teamQueryService.GetPlayersDetails(playerTradeCommand.teamCode, conn);
				var totalActivePlayersInTeam = players.Where(p => p.IsActive).Count();
				if (totalActivePlayersInTeam >= 10)
				{
					returnModel.IsSuccessfull = false;
					returnModel.Message = TEAM_HAS_MORE_THAN_10_PLAYERS;
					return returnModel;
				}

				// 2) Get team code and check player's last stataus
				var playerStatus = _playerQueryService.GetPlayerStatus(playerTradeCommand.playerId, conn);


				// 3) Validate if the old team has less than 4 players
				var playersInOldTeam = _teamQueryService.GetPlayersDetails(playerStatus.TeamCode, conn);
				var totalActivePlayersInOldTeam = players.Where(p => p.IsActive).Count();
				if (totalActivePlayersInOldTeam < 5)
				{
					returnModel.IsSuccessfull = false;
					returnModel.Message = TEAM_HAS_LESS_THAN_4_PLAYERS;
					return returnModel;
				}

				// 4) Validate if the player has been signed
				var isPlayerEverSigned = _playerQueryService.CheckPlayerSigned(playerTradeCommand.playerId, conn);
				if(!isPlayerEverSigned)
				{
					returnModel.IsSuccessfull = false;
					returnModel.Message = PLAYER_HAS_NEVER_BEEN_SIGNED;
					return returnModel;
				}

				// 5) Insert value
				using (var cmdInsert = conn.CreateCommand())
				{
					cmdInsert.CreateParameter(playerTradeCommand.playerId, "PlayerId");
					cmdInsert.CreateParameter(playerTradeCommand.teamCode, "TeamCode");
					cmdInsert.CreateParameter(playerTradeCommand.effectiveDate, "EffectiveDate");

					cmdInsert.CommandText = @"
						INSERT INTO roster_transaction(roster_transaction_type_id, player_id, team_code, effective_date)
						VALUES(4, @PlayerId, @TeamCode, @EffectiveDate)
						";

					var result = cmdInsert.ExecuteNonQuery();
					if (result != 1)
					{
						returnModel.IsSuccessfull = false;
						returnModel.Message = UNABLE_TO_INSERT_A_RECORD;
						return returnModel;
					}
				}
			}
			returnModel.IsSuccessfull = true;
			returnModel.Message = SUCCESSFULLY_TRADED;
			return returnModel;
		}

	}
}
