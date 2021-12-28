using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Projekt
    {
    public class Data
        {
        private Databaza sql;
        public Databaza Sql { get { return sql; } }

        public bool Pripojeny { get { return sql.Pripojeny; } }

        public Data()
            { sql = new Databaza(); }

        public string Pripojit()
            {
            sql.Nastav("localhost", "root", "123", "infosystem");
            return sql.Pripoj();
            }

        public ILoginable Prihlasit(string user, string pass)
            { 
            if (sql == null) Pripojit();

            Users zoznam = sql.Uzivatelia;
            ILoginable hladany = null;

            byte[] encodedPassword = new UTF8Encoding().GetBytes(pass);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            pass = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            IList<ILoginable> novy = sql.ZoznamIlog;
            foreach (User u in sql.Uzivatelia.Zoznam)
                { novy.Add(u); }

            foreach (ILoginable i in novy)
                {
                if (i.Check(user, pass)) 
                    {
                    hladany = i;
                    break;
                    }
                }
            return hladany;
            }

        public void Pridaj(ILoginable user)
            {
            if (sql == null) Pripojit();

            sql.Pridaj(user);
            }

        public void Zmazat(ILoginable user)
            {
            if (sql == null) Pripojit();

            sql.Zmazat(user);
            }

        public void Uprav(ILoginable user)
            {
            if (sql == null) Pripojit();
            if (user == null) return;

            sql.Upravit(user);
            }

        public void Pridaj(Predmet pr)
            {
            if (sql == null) Pripojit();
            if (pr == null) return;

            sql.Pridaj(pr);
            }

        public void Zmazat(Predmet pr)
            {
            if (sql == null) Pripojit();

            sql.Zmazat(pr);
            }

        public void Uprav(Predmet pr)
            {
            if (sql == null) Pripojit();
            if (pr == null) return;

            sql.Upravit(pr);
            }

        public List<PredmetStud> PredmetyUzivatela(User user, bool studovane)
            { return user.Predmety; }
        }
    }