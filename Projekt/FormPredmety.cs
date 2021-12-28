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
    class FormPredmety : Forms
        {
        ListBox stuBox;

        TextBox textnazov;
        TextBox textskratka;
        TextBox textkredit;

        Button submit;

        SaveFileDialog saveFile = new SaveFileDialog();

        public FormPredmety(FormMain hlavne) : base(hlavne)
            {
            formular = new Form();

            formular.StartPosition = FormStartPosition.CenterScreen;
            formular.Size = new Size(400, 300);
            formular.FormBorderStyle = FormBorderStyle.FixedSingle;
            formular.Text = "Informacie o predmetoch";

            textnazov = new TextBox();
            textnazov.Parent = formular;
            textnazov.Left = 180;
            textnazov.Top = 50;
            textnazov.Height = 10;
            textnazov.Width = 80;
            textnazov.Text = "";

            textskratka = new TextBox();
            textskratka.Parent = formular;
            textskratka.Left = 300;
            textskratka.Top = 50;
            textskratka.Height = 10;
            textskratka.Width = 80;
            textskratka.Text = "";

            textkredit = new TextBox();
            textkredit.Parent = formular;
            textkredit.Left = 180;
            textkredit.Top = 75;
            textkredit.Height = 10;
            textkredit.Width = 80;
            textkredit.Text = "";

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
            export.Top = 220;
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
            stuBox.SelectedValueChanged += RefreshPredmet;
            RefreshList(null,null);

            formular.ShowDialog(hlavne);
            }

        private void Zmazat(object sender, EventArgs e)
            {
            if (stuBox.SelectedItem != null ) hlavne.data.Zmazat((Predmet)stuBox.SelectedItem);
            RefreshList(null, null);
            }

        private void Novy(object sender, EventArgs e)
            {
            stuBox.SelectedItem = null;
            textskratka.Text = "";
            textnazov.Text = "";
            textkredit.Text = "";
            }

        private void Potvrd(object sender, EventArgs e)
            {
            if (stuBox.SelectedItem == null)
                {
                int kredit;
                Int32.TryParse(textkredit.Text, out kredit);
                hlavne.data.Pridaj(new Predmet(0, textskratka.Text, textnazov.Text, kredit)); 
                RefreshList(null, null);
                }
            else
                {
                int kredit;
                Int32.TryParse(textkredit.Text, out kredit);
                hlavne.data.Uprav(new Predmet(((Predmet)stuBox.SelectedItem).Id, textnazov.Text, textskratka.Text, kredit)); 
                RefreshList(null, null);
                }
            }

        private void RefreshList(object sender, EventArgs e)
            {
            stuBox.Items.Clear();
            stuBox.Items.AddRange((hlavne.data.Sql.Predmety.Zoznam).ToArray());
            if (stuBox.Items.Count > 0) stuBox.SelectedIndex = 0;
            }

        private void RefreshPredmet(object sender, EventArgs e)
            {
            if (stuBox.SelectedItem == null) return;
            Predmet pr = (Predmet)stuBox.SelectedItem;

            textnazov.Text = pr.Nazov;
            textskratka.Text = pr.Skratka;
            textkredit.Text = pr.Kredit.ToString();
            }

        private void Export(object sender, EventArgs e)
            {
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                XmlDocument xmldoc = new XmlDocument();
                XmlElement xmlsystem = xmldoc.CreateElement("system");
                hlavne.data.Sql.Predmety.ExportXML(ref  xmldoc, ref xmlsystem);
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
