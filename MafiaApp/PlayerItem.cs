﻿using SQLite;

namespace MafiaApp   
{
    public class PlayerItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public Roles Role { get; set; }
        public bool Present { get; set; }
        public string Spouse { get; set; }
        public bool Accomodates { get; set; }
        public int Lives { get; set; }
        //public List<string> Abiities { get; set; }

        public void TogglePresent()
        {
            Present = !Present;
        }
    }
}
