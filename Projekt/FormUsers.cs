using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

using System.Xml;
using System.IO;

namespace Projekt
    {
    class FormUsers : Forms
        {
        ComboBox combo;
        ListBox stuBox;
        SaveFileDialog saveFile = new SaveFileDialog();

        TextBox textmeno;
        TextBox textpriez;
        TextBox textmail;
        TextBox textuser;
        TextBox textpass;

        Button submit;

        public FormUsers(FormMain hlavne) : base(hlavne)
            {
            formular = new Form();

            formular.StartPosition = FormStartPosition.CenterScreen;
            formular.Size = new Size(400, 300);
            formular.FormBorderStyle = FormBorderStyle.FixedSingle;
            formular.Text = "Informacie o uzivateloch";

            textuser = new TextBox();
            textuser.Parent = formular;
            textuser.Left = 180;
            textuser.Top = 50;
            textuser.Height = 10;
            textuser.Width = 80;
            textuser.Text = "";

            textpass = new TextBox();
            textpass.Parent = formular;
            textpass.Left = 300;
            textpass.Top = 50;
            textpass.Height = 10;
            textpass.Width = 80;
            textpass.Text = "";

            textmeno = new TextBox();
            textmeno.Parent = formular;
            textmeno.Left = 180;
            textmeno.Top = 75;
            textmeno.Height = 10;
            textmeno.Width = 80;
            textmeno.Text = "";

            textpriez = new TextBox();
            textpriez.Parent = formular;
            textpriez.Left = 300;
            textpriez.Top = 75;
            textpriez.Height = 10;
            textpriez.Width = 80;
            textpriez.Text = "";

            textmail = new TextBox();
            textmail.Parent = formular;
            textmail.Left = 180;
            textmail.Top = 100;
            textmail.Height = 10;
            textmail.Width = 80;
            textmail.Text = "";

            submit = new Button();
            submit.Parent = formular;
            submit.Left = 300;
            submit.Top = 100;
            submit.Height = 25;
            submit.Width = 80;
            submit.Text = "Ulozit";
            submit.Click += Potvrd;

            Button novy = new Button();
            novy.Parent = formular;
            novy.Left = 180;
            novy.Top = 200;
            novy.Height = 25;
            novy.Width = 80;
            novy.Text = "Novy";
            novy.Click += Novy;

            Button zmazat = new Button();
            zmazat.Parent = formular;
            zmazat.Left = 300;
            zmazat.Top = 200;
            zmazat.Height = 25;
            zmazat.Width = 80;
            zmazat.Text = "Zmazat";
            zmazat.Click += Zmazat;

            Button export = new Button();
            export.Parent = formular;
            export.Left = 300;
            export.Top = 225;
            export.Height = 25;
            export.Width = 80;
            export.Text = "Export";
            export.Click += Export;

            stuBox = new ListBox();
            stuBox.Parent = formular;
            stuBox.Left = 20;
            stuBox.Top = 50;
            stuBox.Height = 150;
            stuBox.Width = 150;
            stuBox.FormattingEnabled = false;
            stuBox.SelectedValueChanged += RefreshUser;

            combo = new ComboBox();
            combo.Parent = formular;
            combo.Left = 20;
            combo.Top = 15;
            combo.Height = 15;
            combo.SelectedValueChanged += RefreshList;
            combo.Items.Add("Admini");
            combo.Items.Add("Studenti");
            combo.Items.Add("Ucitelia");
            combo.SelectedIndex = 0;

            formular.ShowDialog(hlavne); 
            }

        private void Zmazat(object sender, EventArgs e)
            {
            if (stuBox.SelectedItem != null) hlavne.data.Zmazat((ILoginable)stuBox.SelectedItem);
            RefreshList(null,null);
            }

        private void Novy(object sender, EventArgs e)
            {
            stuBox.SelectedItem = null;
            textmeno.Text = "";
            textpriez.Text = "";
            textpass.Text = "";
            textmail.Text = "";
            textuser.Text = "";
            }

        private void Potvrd(object sender, EventArgs e)
            {
            if (stuBox.SelectedItem == null) 
                {
                if (combo.SelectedItem == "Admini") hlavne.data.Pridaj(new Admin(0,textuser.Text,textpass.Text,textmail.Text,textmeno.Text,textpriez.Text));
                else if (combo.SelectedItem == "Studenti") hlavne.data.Pridaj(new Student(0, textuser.Text, textpass.Text, textmail.Text, textmeno.Text, textpriez.Text));
                else if (combo.SelectedItem == "Ucitelia") hlavne.data.Pridaj(new Teacher(0, textuser.Text, textpass.Text, textmail.Text, textmeno.Text, textpriez.Text));
                }
            else
                {
                if (combo.SelectedItem == "Admini") hlavne.data.Uprav(new Admin(((ILoginable)stuBox.SelectedItem).Id, textuser.Text, textpass.Text, textmail.Text, textmeno.Text, textpriez.Text));
                else if (combo.SelectedItem == "Studenti") hlavne.data.Uprav(new Student(((ILoginable)combo.SelectedItem).Id, textuser.Text, textpass.Text, textmail.Text, textmeno.Text, textpriez.Text));
                else if (combo.SelectedItem == "Ucitelia") hlavne.data.Uprav(new Teacher(((ILoginable)combo.SelectedItem).Id, textuser.Text, textpass.Text, textmail.Text, textmeno.Text, textpriez.Text));
                }
            RefreshList(null,null);
            }

        private void RefreshList(object sender, EventArgs e)
            {
            stuBox.Items.Clear();
            if (combo.SelectedItem == null) return;
            List<ILoginable> zoznam = hlavne.data.Sql.ZoznamIlog;
            if (combo.SelectedItem == "Admini") stuBox.Items.AddRange((zoznam.FindAll(x => x.Typ() == "Admin")).ToArray());
            else if (combo.SelectedItem == "Studenti") stuBox.Items.AddRange((zoznam.FindAll(x => x.Typ() == "Student")).ToArray());
            else if (combo.SelectedItem == "Ucitelia") stuBox.Items.AddRange((zoznam.FindAll(x => x.Typ() == "Ucitel")).ToArray());
            if (stuBox.Items.Count > 0) stuBox.SelectedIndex = 0;
            }

        private void RefreshUser(object sender, EventArgs e)
            {
            if (stuBox.SelectedItem == null) return;
            ILoginable user = (ILoginable)stuBox.SelectedItem;

            textuser.Text = user.Username;
            textpass.Text = "";
            textmeno.Text = user.Meno;
            textpriez.Text = user.Priezvisko;
            textmail.Text = user.Email;
            }

        private void Export(object sender, EventArgs e)
            {
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                XmlDocument xmldoc = new XmlDocument();
                XmlElement xmlsystem = xmldoc.CreateElement("system");
                hlavne.data.Sql.Uzivatelia.ExportXML(ref  xmldoc, ref xmlsystem);
                xmldoc.AppendChild(xmlsystem);

                StringBuilder builder = new StringBuilder();
                using (StringWriter sWriter = new StringWriter(builder))
                using (XmlTextWriter xtWriter = new XmlTextWriter(saveFile.FileName, Encoding.UTF8)) //sWriter))
                    {
                    xtWriter.Formatting = Formatting.Indented;
                    xmldoc.WriteContentTo(xtWriter);
                    }
                }
            }
        }
    }
