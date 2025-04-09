using System;
using System.Data.SQLite;

namespace Project_1
{
    class Program
    {
        private readonly static string connectionString = @"Data Source=D:\Progs\DatabaseForVojta\database.db; Version=3;";
        private static List<User> Users = new List<User>();
        static void Main(string[] args)
        {
            Console.Clear();
            OnStart();
            GetFromDB();
            int idCounter = (Users.Count > 0) ? Users[Users.Count - 1].Id + 1 : 0;
            string? op;
            while (true)
            {
                Console.WriteLine("Enter Action [A - Add User, S - See all Users, D - Delete User, Q - Quit]:");
                op = Console.ReadLine().ToUpper();
                switch (op)
                {
                    case "S":
                    DisplayAllUsers();
                    break;
                    case "Q":
                    return;
                    case "A":
                    Console.WriteLine("Enter Username:");
                    string user = Console.ReadLine();
                    Console.WriteLine("Enter Password:");
                    string password = Console.ReadLine();
                    Console.WriteLine("Repeat Password:");
                    string passwordCheck = Console.ReadLine();
                    if (password.Equals(passwordCheck))
                    {
                        AddToDB(new User(idCounter, user, password));
                        idCounter++;
                    } 
                    else
                    {
                        Console.WriteLine("Passwords don't match.");
                    }
                    break;
                    case "D":
                    int id;
                    DisplayAllUsers();
                    Console.WriteLine("Enter ID.");
                    if (int.TryParse(Console.ReadLine(), out id))
                    {
                        DeleteFromDB(id);
                    } 
                    else
                    {
                        Console.WriteLine("Incorrect Value");
                    }
                    break;
                    default:
                    Console.WriteLine("Incorrect Action.");
                    break;
                }
            }
        }
        #region Methods
        private static void OnStart()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open(); 
                string commandString = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, Password TEXT NOT NULL, Username TEXT NOT NULL)";
                SQLiteCommand command = new SQLiteCommand(commandString, connection);
                command.ExecuteNonQuery();
            }
        }
        private static void GetFromDB()
        {
            Users.Clear();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open(); 
                string commandString = "SELECT * FROM Users";
                SQLiteCommand command = new SQLiteCommand(commandString, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string password = reader.GetString(1);
                        string username = reader.GetString(2);
                        Users.Add(new User(id, username, password));
                    }
                }
            }
            Users.Sort();
        }
        private static void AddToDB(User user)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string commandString = $"INSERT INTO Users (Id, Password, Username) VALUES ({user.Id}, '{user.Password}', '{user.Username}')";
                SQLiteCommand command = new SQLiteCommand(commandString, connection);
                command.ExecuteNonQuery();
            }
            GetFromDB();
        }
        private static void DeleteFromDB(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string commandString = $"DELETE FROM Users WHERE Id={id}";
                SQLiteCommand command = new SQLiteCommand(commandString, connection);
                command.ExecuteNonQuery();
            }
            GetFromDB();
        }
        private static void DisplayAllUsers()
        {
            foreach (User user in Users)
            {
                Console.WriteLine("* " + user.ToString());
            }
        }
        #endregion
    }
}