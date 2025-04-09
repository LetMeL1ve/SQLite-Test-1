using System;
using System.Data.SQLite;
// Class User program
namespace Project_1
{
    class User : IComparable
    {
        private int id;
        private string? password;
        private string? username;

        public User(int id, string? username, string? password)
        {
            Id = id;
            Password = password;
            Username = username;

        }
        public User() {}
        public int Id {get {return id;} private set {id = value;}}
        public string? Password {get {return password;} private set {password = value;}}
        public string? Username {get {return username;} private set {username = value;}}

        public override string ToString() => $"ID: {Id}, Username: {Username}, Password: {Password}";

        int IComparable.CompareTo(object? obj)
        {
            User? user = obj as User;
            if (!(user is null))
            return this.Id.CompareTo(user.Id);
            else
            throw new Exception("Object is not a car.");
        }
    }
}