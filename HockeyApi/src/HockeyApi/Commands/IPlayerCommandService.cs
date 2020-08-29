using HockeyApi.Models;

namespace HockeyApi.Commands
{
    public interface IPlayerCommandService
    {
        ReturnModel AssignPlayer(PlayerAssignCommand playerAssignCommand);
        ReturnModel InjurePlayer(PlayerInjuryHealthCommand playerInjuryCommand);
    }
}