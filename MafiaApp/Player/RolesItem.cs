using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaApp  //vorher mit Player
{
    public class RolesItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public roles Role { get; set; }
        public int Number { get; set; }
        public string Ability { get; set; }
        public int Lives { get; set; }
        public bool Active { get; set; }

    }
}
