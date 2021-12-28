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
    public class FormLogin : Forms
        {
        private ErrorProvider chyba;
        private Label text;
        private TextBox userBox;
        private TextBox passBox;

        Label passLbl1;

        public FormLogin(FormMain hlavne) : base(hlavne)
            {
            formular = new Form();

            formular.StartPosition = FormStartPosition.CenterScreen;
            formular.Size = new Size(350, 250);
            formular.FormBorderStyle = FormBorderStyle.FixedSingle;
            formular.Text = "Prihlasenie";

            Button acceptBtn = new Button();
            acceptBtn.Parent = formular;
            acceptBtn.Left = 250;
            acceptBtn.Top = 180;
            acceptBtn.Text = "Prihlasit";
            acceptBtn.TabIndex = 2;
            acceptBtn.Click += new System.EventHandler(PrihlasBtn);

            Button cancelBtn = new Button();
            cancelBtn.Parent = formular;
            cancelBtn.Left = 130;
            cancelBtn.Top = 180;
            cancelBtn.Text = "Zavriet";
            cancelBtn.TabIndex = 3;
            cancelBtn.Click += new System.EventHandler(hlavne.ZavriApp);

            userBox = new TextBox();
            userBox.Parent = formular;
            userBox.Left = 150;
            userBox.Top = 50;
            userBox.Text = "";
            userBox.TabIndex = 0;

            passBox = new TextBox();
            passBox.Parent = formular;
            passBox.Left = 150;
            passBox.Top = 80;
            passBox.Text = "";
            passBox.TabIndex = 1;

            Label userLbl = new Label();
            userLbl.Parent = formular;
            userLbl.Left = 110;
            userLbl.Top = 52;
            userLbl.Text = "Login:";

            Label passLbl = new Label();
            passLbl.Parent = formular;
            passLbl.Left = 110;
            passLbl.Top = 82;
            passLbl.Text = "Heslo:";

            text = new Label();
            text.Left = 130;
            text.Top = 130;
            text.Width = 170;
            text.Parent = formular;
            
            formular.ShowDialog(hlavne);    
            }


        public void PrihlasBtn(object sender, EventArgs e)
            { Prihlas();  }

        public void Zobraz(User u)
            {
            
            }

        private void Prihlas()
            {
            if (!hlavne.data.Pripojeny)
                {
                string output = hlavne.data.Pripojit();
                if (output != "")
                    {
                    chyba = new ErrorProvider();
                    text.Text = "Chyba pripojenia: "+output;
                    chyba.SetError(text, output);
                    }
                else 
                    {
                    text.Text = "";
                    if (chyba != null) chyba.Clear();
                    }
                }
            if (hlavne.data.Pripojeny)
                {
                ILoginable prihlaseny = hlavne.data.Prihlasit(userBox.Text, passBox.Text);
                if (prihlaseny == null)
                    { 
                    chyba = new ErrorProvider();
                    text.Text = "Chyba autentifikacie: zle heslo alebo meno";
                    chyba.SetError(text, "zle heslo alebo meno");
                    }
                else
                    {
                    text.Text = "";
                    if (chyba != null) chyba.Clear();
                    hlavne.Nacitaj(prihlaseny);
                    formular.Close();
                    }
                }
            }
        }
    }
