using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment2
{
    public class Game_Database
    {
        readonly SQLiteConnection database;

        public Game_Database(string dbPath)
        {
            database = new SQLiteConnection(dbPath);
            database.CreateTable<Opponent>();
            database.CreateTable<Match>();
            database.CreateTable<Game>();

            // Insert default games if the Games table is empty
            if (database.Table<Game>().Count() == 0)
            {
                database.Insert(new Game { Name = "Chess", Description = "Simple grid game", Rating = 9.5 });
                database.Insert(new Game { Name = "Checkers", Description = "Simpler grid game", Rating = 5 });
                database.Insert(new Game { Name = "Dominoes", Description = "Blocks game", Rating = 6.75 });
            }
        }

        // Opponent Methods
        public List<Opponent> GetOpponents()
        {
            return database.Table<Opponent>().ToList();
        }

        public Opponent GetOpponent(int id)
        {
            return database.Table<Opponent>().FirstOrDefault(o => o.Id == id);
        }

        public int SaveOpponent(Opponent opponent)
        {
            if (opponent.Id != 0)
            {
                return database.Update(opponent);
            }
            else
            {
                return database.Insert(opponent);
            }
        }

        public int DeleteOpponent(Opponent opponent)
        {
            // Delete all matches associated with this opponent
            database.Table<Match>().Delete(m => m.OpponentId == opponent.Id);
            // Delete the opponent
            return database.Delete(opponent);
        }

        // Match Methods
        public List<Match> GetMatchesForOpponent(int opponentId)
        {
            return database.Table<Match>().Where(m => m.OpponentId == opponentId).ToList();
        }

        public int SaveMatch(Match match)
        {
            if (match.Id != 0)
            {
                return database.Update(match);
            }
            else
            {
                return database.Insert(match);
            }
        }

        public int DeleteMatch(Match match)
        {
            return database.Delete(match);
        }

        // Game Methods
        public List<Game> GetGames()
        {
            return database.Table<Game>().ToList();
        }

        public Game GetGame(int id)
        {
            return database.Table<Game>().FirstOrDefault(g => g.Id == id);
        }

        public int GetMatchCountForGame(int gameId)
        {
            return database.Table<Match>().Count(m => m.GameId == gameId);
        }

        // Reset Database Method
        public void ResetDatabase()
        {
            database.DropTable<Opponent>();
            database.DropTable<Match>();
            database.DropTable<Game>();
            database.CreateTable<Opponent>();
            database.CreateTable<Match>();
            database.CreateTable<Game>();
            // Insert default games
            database.Insert(new Game { Name = "Chess", Description = "Simple grid game", Rating = 9.5 });
            database.Insert(new Game { Name = "Checkers", Description = "Simpler grid game", Rating = 5 });
            database.Insert(new Game { Name = "Dominoes", Description = "Blocks game", Rating = 6.75 });
        }
    }
}