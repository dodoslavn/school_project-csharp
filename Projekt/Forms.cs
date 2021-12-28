using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;



namespace Projekt
    {
    public class Forms
        {
        protected FormMain hlavne;

        protected Form formular;
        public Form Formular { get { return formular; } }

        public Forms(FormMain hlavne)
            { this.hlavne = hlavne; }

        public void Zavri()
            { formular.Close(); }

        }
    }
