using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MafiaApp   //vorher mit .Player
{
    public class PlayerItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public roles Role { get; set; }        // Rollen Datentyp einführen
        public bool Present { get; set; }
        public string Spouse { get; set; }
        public int Lives { get; set; }
        public bool Victim { get; set; }

    }
}
