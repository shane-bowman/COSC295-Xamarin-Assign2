using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Assignment2
{
    public class Game
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
    }
}
