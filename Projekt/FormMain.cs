using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt
    {
    public partial class FormMain : Form
        {
        public Data data = new Data();
        private FormLogin loginokno;
        private FormPredmet predokno;
        private FormUser uokno;
        private Forms okno;

        public User uzivatel = null;
        public Admin admin = null;

        public FormMain()
            { InitializeComponent(); }

        private void FormMain_Load(object sender, EventArgs e)
            {
            this.Show();
            loginokno = new FormLogin(this);
            }

        public void ZavriApp(object sender, EventArgs e)
            { Close(); }

        public void Nacitaj(ILoginable prihlaseny)
            {
            if (prihlaseny.Typ() != "Admin") uzivatel = (User)prihlaseny;
            else admin = (Admin)prihlaseny;

            if (uzivatel != null)
                {
                label1.Text = uzivatel.Meno + " " + uzivatel.Priezvisko ;
                label2.Text = uzivatel.Typ() + " ID: " + uzivatel.Id;
                label3.Text = "";
                label4.Text = "";

                if (uzivatel.Typ() == "Student")
                    {
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("Historia");
                    comboBox1.Items.Add("Aktivne");
                    comboBox1.SelectedIndex = 1;
                    }
                else if (uzivatel.Typ() == "Ucitel")
                    {
                    button1.Text = "Oznamkovat";
                    comboBox2.Items.Add("A");
                    comboBox2.Items.Add("B");
                    comboBox2.Items.Add("C");
                    comboBox2.Items.Add("D");
                    comboBox2.Items.Add("E");
                    comboBox2.Items.Add("F");
                    }
                Obnov();
                }
            else if (admin != null)
                {
                label1.Text = admin.Meno + " " + admin.Priezvisko;
                label2.Text = admin.Typ() + " ID: " + admin.Id;
                label3.Text = "";
                label4.Text = "";
                listBox1.Visible = false;
                comboBox1.Visible = false;
                comboBox2.Visible = false;
                button1.Visible = false;

                button2.Visible = true;
                stuBut.Visible = true;
                }
            }

        private void Obnov()
            {
            listBox1.Items.Clear();

            if (uzivatel.Typ() == "Student")
                {
                if (comboBox1.SelectedIndex == 0)
                    {
                    foreach (PredmetStud p in uzivatel.Predmety)
                        { if (p.Znamka != "O") listBox1.Items.Add(p); }
                    }
                else
                    {
                    foreach (PredmetStud p in uzivatel.Predmety)
                        { if (p.Znamka == "O") listBox1.Items.Add(p); }
                    }

                int kredity = 0;
                foreach (PredmetStud pr in uzivatel.Predmety)
                    { if (pr.Znamka != "O") kredity += pr.Predmet.Kredit; }
                label3.Text = "Pocet ziskanych kreditov: " + kredity;

                kredity = 0;
                foreach (PredmetStud pr in uzivatel.Predmety)
                    { if (pr.Znamka == "O") kredity += pr.Predmet.Kredit; }
                label4.Text = "Pocet prave zapisanych kreditov: " + kredity;

                comboBox2.Items.Clear();
                foreach (Predmet pr in data.Sql.Predmety.Zoznam)
                    { if (uzivatel.Najdi(pr) == null) comboBox2.Items.Add(pr); }
                }
            else if (uzivatel.Typ() == "Ucitel")
                {
                comboBox1.Items.Clear();
                foreach (PredmetStud pr in uzivatel.Predmety)
                    { comboBox1.Items.Add(pr); }
                if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
                }
            }

        public void OtvorOkno(User u)
            {
            if (uokno != null) uokno.Zavri();
            uokno = new FormUser(this, u);
            }

        private void zavrietToolStripMenuItem_Click(object sender, EventArgs e)
            { Application.Exit(); }

        private void listBox1_DoubleClick(object sender, EventArgs e)
            {
            PredmetStud pr = (PredmetStud)listBox1.SelectedItem;
            if (pr != null)
                { 
                if (predokno != null) predokno.Zavri();
                predokno = new FormPredmet(this,pr);
                }
            }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
            {
            if (uzivatel.Typ() == "Student")
                {
                listBox1.Items.Clear();
                if (comboBox1.SelectedIndex == 0)
                    {
                    foreach (PredmetStud p in uzivatel.Predmety)
                        { if (p.Znamka != "O") listBox1.Items.Add(p); }
                    }
                else
                    {
                    foreach (PredmetStud p in uzivatel.Predmety)
                        { if (p.Znamka == "O") listBox1.Items.Add(p); }
                    }
                }
            else if (comboBox1.SelectedItem != null && uzivatel.Typ() == "Ucitel")
                {
                listBox1.Items.Clear();
                foreach (User u in data.Sql.Uzivatelia.Najdi(((PredmetStud)comboBox1.SelectedItem).Predmet))
                        {
                        if (u.Typ() == "Student")
                            {
                            PredmetStud pr = u.Najdi(((PredmetStud)comboBox1.SelectedItem).Predmet);
                            if (pr != null)
                                { if (pr.Znamka == "O") listBox1.Items.Add(u); }
                            }
                        }
                }
            }

        private void button1_Click(object sender, EventArgs e)
            { 
            if (comboBox2.SelectedItem != null)
                {
                if (uzivatel.Typ() == "Student")
                    {
                    Predmet pr = (Predmet) comboBox2.SelectedItem;
                    PredmetStud prs = new PredmetStud("O", pr);
                    if (uzivatel.Najdi(pr) == null) uzivatel.Predmety.Add(prs);
                    data.Sql.PridatVztah(uzivatel, prs);
                    Obnov();
                    }
                else if (uzivatel.Typ() == "Ucitel" && listBox1.SelectedItem != null)
                    {
                    User u = (User)listBox1.SelectedItem;
                    Predmet pr = ((PredmetStud)comboBox1.SelectedItem).Predmet;
                    u.Najdi(pr).Znamka = (string)comboBox2.SelectedItem;
                    data.Sql.UpravVztah(u, (PredmetStud)comboBox1.SelectedItem);
                    Obnov();
                    }
                }
            }

        private void stuBut_Click(object sender, EventArgs e)
            {
            if (okno != null) okno.Zavri();
            okno = new FormUsers(this);
            }

        private void button2_Click(object sender, EventArgs e)
            {
            if (okno != null) okno.Zavri();
            okno = new FormPredmety(this);
            }
        }
    }



//pridavanie //done
//ohodnotenie predemtu ucitelom ziakovi //done
//ucim neucim ?
//admin //done
//xml 