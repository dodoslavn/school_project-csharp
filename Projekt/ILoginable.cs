using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt
    {
    public interface ILoginable 
        {
        int Id { get; }
        string Meno { get; }
        string Priezvisko { get; }
        string Username { get; }
        string Email { get; }
        string Password { get; }

        bool Check(string meno, string heslo);
        string Typ();
        }
    }