using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt
    {
    public class Admin : ILoginable
        {
        private int id;
        public int Id { get { return id; } }

        private string meno;
        public string Meno { get { return meno; } }

        private string priezvisko;
        public string Priezvisko { get { return priezvisko; } }

        private string username;
        public string Username { get { return username; } }

        private string email;
        public string Email { get { return email; } }

        private string password;
        public string Password { get { return password; } }

        public string Typ()
            { return "Admin"; }

        public Admin(int id, string user, string pw, string email, string meno, string priezvisko)
            {
            this.id = id;
            username = user;
            password = pw;
            this.email = email;
            this.meno = meno;
            this.priezvisko = priezvisko;
            }

        public bool Check(string username, string heslo)
            {
            if (username == this.username && heslo == this.password) return true;
            else return false;
            }

        public override string ToString()
            {
 	        return this.meno + " " + this.priezvisko;
            }
        }
    }