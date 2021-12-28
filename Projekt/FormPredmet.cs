using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

namespace Projekt
    {
    public class FormPredmet : Forms
        {
        private PredmetStud predmet;
        private ListBox uciBox, stuBox;
        private ComboBox combo;

        public FormPredmet(FormMain hlavne, PredmetStud pr) : base(hlavne)
            {
            formular = new Form();

            predmet = pr;

            formular.StartPosition = FormStartPosition.CenterScreen;
            formular.Size = new Size(400, 300);
            formular.FormBorderStyle = FormBorderStyle.FixedSingle;
            formular.Text = "Informacie o predmete";

            Label menopopisLbl = new Label();
            menopopisLbl.Parent = formular;
            menopopisLbl.Left = 20;
            menopopisLbl.Top = 20;
            menopopisLbl.Height = 10;
            menopopisLbl.Width = 100;
            menopopisLbl.Text = "Nazov predemtu: ";

            Label menonazovLbl = new Label();
            menonazovLbl.Parent = formular;
            menonazovLbl.Left = 20;
            menonazovLbl.Top = 35;
            menopopisLbl.Height = 15;
            menonazovLbl.Width = 100;
            menonazovLbl.Text = pr.Predmet.Nazov;

            Label skratkapopisLbl = new Label();
            skratkapopisLbl.Parent = formular;
            skratkapopisLbl.Left = 20;
            skratkapopisLbl.Top = 60;
            skratkapopisLbl.Height = 15;
            skratkapopisLbl.Width = 100;
            skratkapopisLbl.Text = "Skratka predemtu: ";

            Label skratkanazovLbl = new Label();
            skratkanazovLbl.Parent = formular;
            skratkanazovLbl.Left = 20;
            skratkanazovLbl.Top = 75;
            skratkanazovLbl.Height = 15;
            skratkanazovLbl.Width = 100;
            skratkanazovLbl.Text = pr.Predmet.Skratka;

            Label kreditkapopisLbl = new Label();
            kreditkapopisLbl.Parent = formular;
            kreditkapopisLbl.Left = 20;
            kreditkapopisLbl.Top = 100;
            kreditkapopisLbl.Height = 15;
            kreditkapopisLbl.Width = 100;
            kreditkapopisLbl.Text = "Vaha predemtu: ";

            Label kreditanazovLbl = new Label();
            kreditanazovLbl.Parent = formular;
            kreditanazovLbl.Left = 20;
            kreditanazovLbl.Top = 115;
            kreditanazovLbl.Height = 15;
            kreditanazovLbl.Width = 100;
            kreditanazovLbl.Text = pr.Predmet.Kredit+" kreditov";

            Label uciLbl = new Label();
            uciLbl.Parent = formular;
            uciLbl.Left = 200;
            uciLbl.Top = 15;
            uciLbl.Height = 15;
            uciLbl.Width = 100;
            uciLbl.Text = "Vyucuje:";

            uciBox = new ListBox();
            uciBox.Parent = formular;
            uciBox.Left = 200;
            uciBox.Top = 30;
            uciBox.Height = 100;
            uciBox.Width = 150;
            uciBox.DoubleClick += uciBox_DoubleClick;

            combo = new ComboBox();
            combo.Parent = formular;
            combo.Left = 200;
            combo.Top = 135;
            combo.Height = 15;
            combo.SelectedValueChanged += combo_SelectedValueChanged;

            stuBox = new ListBox();
            stuBox.Parent = formular;
            stuBox.Left = 200;
            stuBox.Top = 160;
            stuBox.Height = 100;
            stuBox.Width = 150;
            stuBox.DoubleClick += stuBox_DoubleClick;

            combo.Items.Add("Studuju");
            combo.Items.Add("Vystudovali");
            combo.SelectedIndex = 0;

            Nacitaj();

            formular.ShowDialog(hlavne); 
            }

        void combo_SelectedValueChanged(object sender, EventArgs e)
            { Nacitaj();  }
        public void Nacitaj()
            {
            stuBox.Items.Clear();
            uciBox.Items.Clear();

            foreach (User u in hlavne.data.Sql.Uzivatelia.Najdi(predmet.Predmet))
                {
                if (u.Typ() == "Student" ) 
                    {
                    PredmetStud pr = u.Najdi(predmet.Predmet);
                    if (pr != null)
                        { 
                        if (pr.Znamka == "O" && combo.SelectedIndex == 0) stuBox.Items.Add(u);
                        else if (pr.Znamka != "O" && combo.SelectedIndex == 1) stuBox.Items.Add(u);
                        }
                    }
                    
                else uciBox.Items.Add(u);
                }
            }
        private void uciBox_DoubleClick(object sender, EventArgs e)
            { hlavne.OtvorOkno((User)uciBox.SelectedItem); }

        private void stuBox_DoubleClick(object sender, EventArgs e)
            { hlavne.OtvorOkno((User)stuBox.SelectedItem); }

        }
    }
