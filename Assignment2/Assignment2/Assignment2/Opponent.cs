using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2
{
    public class Opponent
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email {  get; set; }
    }
}
