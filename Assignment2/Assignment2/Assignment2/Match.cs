using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Assignment2
{
    public class Match
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int OpponentId { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public int GameId { get; set; }
        public bool Win {  get; set; }
    }
}
