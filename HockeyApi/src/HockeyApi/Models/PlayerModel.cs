using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Models
{
    public class PlayerModel
    {
        public PlayerModel(string firstName, string lastName, int id)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = id;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
    }
}
