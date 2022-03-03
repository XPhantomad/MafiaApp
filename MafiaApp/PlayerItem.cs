using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MafiaApp   
{
    public class PlayerItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public bool Present { get; set; }
        public int SpouseId { get; set; }
        public double Lives { get; set; }

        public void TogglePresent()
        {
            Present = !Present;
        }
    }
}
