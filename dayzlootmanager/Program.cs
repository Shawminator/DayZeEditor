using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeEditor
{
    static class Program
    {
        public static SplashForm mySplashForm;
        static MainForm myMainForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Show Splash Form
            mySplashForm = new SplashForm();
            if (mySplashForm != null)
            {
                Thread splashThread = new Thread(new ThreadStart(
                    () => { Application.Run(mySplashForm); }));
                splashThread.SetApartmentState(ApartmentState.STA);
                splashThread.Start();
            }
            //Create and Show Main Form
            myMainForm = new MainForm(mySplashForm);
            myMainForm.LoadCompleted += MainForm_LoadCompleted;
            Thread.Sleep(3500);
            Application.Run(myMainForm);
        }
        private static void MainForm_LoadCompleted(object sender, EventArgs e)
        {
            if (mySplashForm == null || mySplashForm.Disposing || mySplashForm.IsDisposed)
                return;
            mySplashForm.Invoke(new Action(() => { mySplashForm.Close(); }));
            mySplashForm.Dispose();
            mySplashForm = null;
            myMainForm.Activate(); 
        }
    }
}
