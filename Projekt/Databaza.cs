using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography;

namespace Projekt
    {
    public class Databaza
        {
        private Predmety_vztahy predmety_vztahy;

        private List<ILoginable> vsetko = new List<ILoginable>();
        public List<ILoginable> ZoznamIlog 
            { 
            get 
                { 
                nacitajVse();
                return vsetko;
                }
            }

        private Users uzivatelia;
        public Users Uzivatelia
            {
            get 
                {
                if (uzivatelia == null) nacitajUsers(); 
                return uzivatelia;
                }
            }

        private void nacitajVse()
            {
            vsetko.Clear();
            nacitajUsers();
            nacitajPredmety();
            nacitajPredmetyVztahy();
            }

        private void nacitajUsers()
            {
            MySqlCommand prikaz = new MySqlCommand("SELECT * FROM users", pripojenie);
            MySqlDataReader vystup = prikaz.ExecuteReader();

            if (uzivatelia != null) uzivatelia.Zoznam.Clear();
            else uzivatelia = new Users();
            while (vystup.Read())
                {
                if (vystup["typ"] + "" == "stu")
                    uzivatelia.Pridaj(new Student((int)vystup["id"], vystup["login"] + "", vystup["password"] + "", vystup["email"] + "", vystup["meno"] + "", vystup["prijmeni"] + ""));
                else if (vystup["typ"] + "" == "uci")
                    uzivatelia.Pridaj(new Teacher((int)vystup["id"], vystup["login"] + "", vystup["password"] + "", vystup["email"] + "", vystup["meno"] + "", vystup["prijmeni"] + ""));
                else if (vystup["typ"] + "" == "adm")
                    { vsetko.Add(new Admin((int)vystup["id"], vystup["login"] + "", vystup["password"] + "", vystup["email"] + "", vystup["meno"] + "", vystup["prijmeni"] + "")); }
                }
            vsetko.AddRange(uzivatelia.Zoznam.ToArray());
            vystup.Close();
            }

        private Predmety predmety;
        public Predmety Predmety
            {
            get
                {
                if (predmety == null) nacitajPredmety(); 
                return predmety;
                }
            }

        private void nacitajPredmety()
            {
            MySqlCommand prikaz = new MySqlCommand("SELECT * FROM predmety", pripojenie);
            MySqlDataReader vystup = prikaz.ExecuteReader();

            predmety = new Predmety();
            while (vystup.Read())
                { predmety.Pridaj(new Predmet((int)vystup["id"], vystup["nazov"] + "", vystup["skratka"] + "", (int)vystup["kredit"])); }
            vystup.Close();
            }

        private void nacitajPredmetyVztahy()
            {
            MySqlCommand prikaz = new MySqlCommand("SELECT * FROM predmety_vztahy", pripojenie);
            MySqlDataReader vystup = prikaz.ExecuteReader();

            predmety_vztahy = new Predmety_vztahy();
            while (vystup.Read())
                { predmety_vztahy.Pridaj(new Predmet_vztah((int)vystup["id"], (int)vystup["predmet"], (int)vystup["user"], vystup["typ"] + "")); }
            vystup.Close();

            foreach(Predmet_vztah pv in predmety_vztahy.Zoznam)
                {
                Predmet pr = predmety.Zoznam.Find(x => pv.Predmet == x.Id);
                uzivatelia.PridajPredmet(pv.User, pr, pv.Typ); 
                }
            }

        private MySqlConnection pripojenie;
        public bool Pripojeny 
            { 
            get 
                { 
                if (pripojenie == null) return false;
                return pripojenie.State == ConnectionState.Open; 
                } 
            }

        private string server, login, password, db;

        public Databaza(string server, string login, string password, string db)
            { Nastav(server, login, password, db); }

        public Databaza() { }

        public void Nastav(string server, string login, string password, string db)
            {
            this.server = server;
            this.login = login;
            this.password = password;
            this.db = db;

            pripojenie = new MySqlConnection("SERVER=" + server + ";DATABASE=" + db + "; UID=" + login + ";PASSWORD=" + password + ";");
            }

        public void UpravUser(User u)
            {
            MySqlCommand prikaz = new MySqlCommand("UPDATE users SET `login`='" + u.Username + "', `meno`='" + u.Meno + "',`prijmeni`='" + u.Priezvisko + "', `password`='" + u.Password + "', `email`='" + u.Email + "' WHERE `id`='"+u.Id+"';", pripojenie);
            prikaz.ExecuteNonQuery();

            nacitajVse();
            }

        public void UpravVztah(User u, PredmetStud pr)
            {
            int id = predmety_vztahy.Zoznam.Find(x => (x.User == u.Id && x.Predmet == pr.Predmet.Id)).Id;
            string typ = u.Najdi(pr.Predmet).Znamka;
            MySqlCommand prikaz = new MySqlCommand("UPDATE predmety_vztahy SET `predmet`='"+pr.Predmet.Id+"' , `user`='"+u.Id+"', `typ`='"+typ+"' WHERE `id`='"+id+"';", pripojenie);
            prikaz.ExecuteNonQuery();

            nacitajVse();
            }

        public void PridatVztah(User u, PredmetStud pr)
            {
            string typ = u.Najdi(pr.Predmet).Znamka;
            MySqlCommand prikaz = new MySqlCommand("INSERT INTO predmety_vztahy (`predmet`, `user`, `typ`) VALUES ('" + pr.Predmet.Id + "', '" + u.Id + "', '" + pr.Znamka + "');", pripojenie);
            prikaz.ExecuteNonQuery();

            nacitajVse();
            }

        public string Pripoj()
            {
            this.Odpoj();
            try
                {
                pripojenie.Open();
                nacitajVse();
                }
            catch (MySqlException e)
                {
                switch (e.Number)
                    {
                    case 0: return "Server is offline";
                    case 1045: return "Access denied";
                    default: return "---";
                    }
                }
            return "";
            }

        public void Pridaj(ILoginable user)
            {
            string typ;

            if (user.Typ() == "Student") typ = "stu";
            else if (user.Typ() == "Ucitel") typ = "uci";
            else typ = "adm"; 

            MySqlCommand prikaz = new MySqlCommand("INSERT INTO users (`login`, `meno`, `prijmeni`, `password`, `email`, `typ`) VALUES ('" + user.Username + "', '" + user.Meno + "', '" + user.Priezvisko + "', '" + user.Password + "', '" + user.Email + "', '" + typ + "');", pripojenie);
            prikaz.ExecuteNonQuery();
            nacitajVse();
            }

        public void Pridaj(Predmet pr)
            {
            MySqlCommand prikaz = new MySqlCommand("INSERT INTO predmety (`nazov`, `skratka`, `kredit`) VALUES ('" + pr.Nazov + "', '" + pr.Skratka + "', '" + pr.Kredit + "');", pripojenie);
            prikaz.ExecuteNonQuery();
            nacitajVse();
            }

        public void Zmazat(ILoginable user)
            {
            MySqlCommand prikaz = new MySqlCommand("DELETE FROM `users` WHERE `id`='"+ user.Id +"';", pripojenie);
            prikaz.ExecuteNonQuery();
            nacitajVse();
            }

        public void Upravit(ILoginable user)
            {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(user.Password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string heslo = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            string typ;

            if (user.Typ() == "Student") typ = "stu";
            else if (user.Typ() == "Ucitel") typ = "uci";
            else typ = "adm";

            MySqlCommand prikaz = new MySqlCommand("UPDATE `users` SET `login`='" + user.Username + "', `meno`='" + user.Meno + "', `prijmeni`='" + user.Priezvisko + "', `password`='" + heslo + "', `email`='" + user.Email + "', `typ`='" + typ + "' WHERE `id`='" + user.Id + "';", pripojenie);
            prikaz.ExecuteNonQuery();
            nacitajVse();
            }

        public void Zmazat(Predmet pr)
            {
            MySqlCommand prikaz = new MySqlCommand("DELETE FROM `predmety` WHERE `id`='" + pr.Id + "';", pripojenie);
            prikaz.ExecuteNonQuery();
            nacitajVse();
            }

        public void Upravit(Predmet pr)
            {
            MySqlCommand prikaz = new MySqlCommand("UPDATE `predmety` SET `nazov`='" + pr.Nazov + "', `skratka`='" + pr.Skratka + "', `kredit`='" + pr.Kredit + "' WHERE `id`='" + pr.Id + "';", pripojenie);
            prikaz.ExecuteNonQuery();
            nacitajVse();
            }

        private void Odpoj()
            {
            try
                { pripojenie.Close(); }
            catch
                { throw; }
            }
        }
    }
