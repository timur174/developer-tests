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
		private const string TEAM_DOES_NOT_EXIST = "Error: Team with entered team_code does not exist";
		private const string TEAM_HAS_MORE_THAN_10_PLAYERS = "Error: Team has more than 10 players";
		private const string TEAM_HAS_MORE_LESS_THAN_4_PLAYERS = "Error: Team has less than 4 players";
		private const string UNABLE_TO_INSERT_A_RECORD = "Error: Unable to insert the table";

		private const string SUCCESSFULLY_ASSIGNED = "Success: Player assigned to a team successfully";
		public PlayerCommandService
		(
			IDb db,
			ITeamQueryService teamQueryService
		)
		{
			_db = db;
			_teamQueryService = teamQueryService;
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

		public ReturnModel InjurePlayer(PlayerInjuryCommand playerInjuryCommand)
		{
			var returnModel = new ReturnModel();
			var team_code = string.Empty;
			var roster_transaction_type_id = (int)PlayerStatus.Signed;
			using (var conn = _db.CreateConnection())
			{
				// 1) Get team code
				using (var cmd = conn.CreateCommand())
				{

					cmd.CreateParameter(playerInjuryCommand.playerId, "PlayerId");

					cmd.CommandText = @"
						SELECT TOP 1 
							team_code,
							roster_transaction_type_id
						FROM
							roster_transaction rt
						WHERE
							rt.player_id = @PlayerId
						ORDER BY rt.effective_date DESC";
					using (var rd = cmd.ExecuteReader())
					{
						while (rd.Read())
						{
							team_code = rd.GetString(0);
							roster_transaction_type_id = rd.GetInt32(1);
						}
					}

					if ((PlayerStatus)roster_transaction_type_id == PlayerStatus.Injured)
					{
						returnModel.IsSuccessfull = false;
						returnModel.Message = TEAM_DOES_NOT_EXIST;
						return returnModel;
					}
				}
				// 2) Validate if team has less than 4 players
				var players = _teamQueryService.GetPlayersDetails(team_code, conn);
				var totalActivePlayersInTeam = players.Where(p => p.IsActive).Count();
				if (totalActivePlayersInTeam < 5)
				{
					returnModel.IsSuccessfull = false;
					returnModel.Message = TEAM_HAS_MORE_LESS_THAN_4_PLAYERS;
					return returnModel;
				}

				// 3) Insert value
				using (var cmdInsert = conn.CreateCommand())
				{
					cmdInsert.CreateParameter(playerInjuryCommand.playerId, "PlayerId");
					cmdInsert.CreateParameter(playerInjuryCommand.effectiveDate, "EffectiveDate");

					cmdInsert.CommandText = @"
						DECLARE @TeamCode nvarchar(30);
						DECLARE @TransactionTypeId int;
						
						SELECT TOP 1
						  @TeamCode = rt.team_code,
						  @TransactionTypeId = rt.roster_transaction_type_id
						FROM
							roster_transaction rt
						WHERE
							player_id = @PlayerId
						ORDER BY
							effective_date DESC
						
						IF(@TransactionTypeId <> 2)
						BEGIN
							INSERT INTO roster_transaction(roster_transaction_type_id, player_id, team_code, effective_date)
							VALUES(2, @PlayerId, @TeamCode, @EffectiveDate)
						END
						";

					var result = cmdInsert.ExecuteNonQuery();
					if (result != 2)
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
		////TODO: refactor
		//private IDbDataParameter CreateParameter(IDbCommand command, object value, string name)
		//{
		//	var param = command.CreateParameter();
		//	param.Value = value;
		//	param.ParameterName = name;
		//	return param;
		//}

	}
}
