using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MafiaApp   //vorher mit .Player
{
    public class PlayerItem
    {
        // nur Beispielhaft hoffe es verschwinde ein paar Fehler
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        //public string Name { get; set; }
        public string Notes { get; set; }
        //public bool Done { get; set; }
    }
}
