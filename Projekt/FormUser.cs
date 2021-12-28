using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Projekt
    {
    class FormUser : Forms
        {
        User u;
        public FormUser(FormMain hlavne, User user):base(hlavne)
            {
            formular = new Form();
            u = user;

            formular.StartPosition = FormStartPosition.CenterScreen;
            formular.Size = new Size(350, 250);
            formular.FormBorderStyle = FormBorderStyle.FixedSingle;
            formular.Text = "Informacie o predemte";

            //if (u == null) u = new User("chyba","");

            Label menoLbl = new Label();
            menoLbl.Parent = formular;
            menoLbl.Left = 20;
            menoLbl.Top = 20;
            menoLbl.Text = u.Meno+" "+u.Priezvisko;

            Label idLbl = new Label();
            idLbl.Parent = formular;
            idLbl.Left = 20;
            idLbl.Top = 50;
            idLbl.Text = "ID: " + u.Id;

            Label userLbl = new Label();
            userLbl.Parent = formular;
            userLbl.Left = 175;
            userLbl.Top = 15;
            userLbl.Height = 15;
            userLbl.Text = "Zapisane predmety:";

            ListBox uciBox = new ListBox();
            uciBox.Parent = formular;
            uciBox.Left = 175;
            uciBox.Top = 30;
            uciBox.Height = 100;
            uciBox.Width = 150;

            foreach (PredmetStud pr in u.Predmety)
                { uciBox.Items.Add(pr.Predmet); }

            formular.ShowDialog(hlavne); 
            }
        }
    }
