using HockeyApi.Models;

namespace HockeyApi.Commands
{
    public interface IPlayerCommandService
    {
        ReturnModel AssignPlayer(PlayerAssignCommand playerAssignCommand);
        ReturnModel InjurePlayer(PlayerInjuryHealthCommand playerInjuryCommand);
        ReturnModel HealPlayer(PlayerInjuryHealthCommand playerHealthyCommand);
        ReturnModel TradePlayer(TradeCommand playerTradeCommand);
    }
}