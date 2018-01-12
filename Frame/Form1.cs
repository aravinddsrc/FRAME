using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Thread t = new Thread(new ThreadStart(Splash));
            t.Start();
            InitializeComponent();
            string str = string.Empty;
            for (int i = 0; i < 100000; i++)
            {

                str += i.ToString();
            }
            t.Abort();
        }
        void Splash()
        {
            SplashScreen.SplashForm frm = new SplashScreen.SplashForm();
            frm.AppName = "Demo";
            Application.Run(frm);
        }
    }
}
