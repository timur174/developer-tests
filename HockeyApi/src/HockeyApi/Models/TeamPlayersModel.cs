using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Models
{
    public class TeamPlayersModel
    {
        public TeamPlayersModel(string firstName, string lastName, string teamName, string currentPlayerStatus, int isActiveFlag)
        {
            FirstName = firstName;
            LastName = lastName;
            TeamName = teamName;
            CurrentPlayerStatus = currentPlayerStatus;
            IsActiveFlag = isActiveFlag;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamName { get; set; }
        public string CurrentPlayerStatus { get; set; }
        public bool IsActive { get { return IsActiveFlag == 1; } }
        private int IsActiveFlag { get; set; }
    }
}
