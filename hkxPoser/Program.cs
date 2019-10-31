using System;
using System.IO;
using System.Windows.Forms;

namespace hkxPoser
{
    static class Program
    {
        

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            

            Settings settings = Settings.Load(Path.Combine(Application.StartupPath, @"config.xml"));

            string loadfilePath = "";
            if (args.Length != 0)
                loadfilePath = args[0];

            
            if(settings.SingleLaunch) {
                string mtxName = Application.ProductName;

            }

            MainWindow mainWindow = new MainWindow(settings, loadfilePath);
            BoneController boneController = new BoneController();

            boneController.TopLevel = false;
            boneController.Location = new System.Drawing.Point(0, 26);
            mainWindow.Controls.Add(boneController);
            boneController.BringToFront();
            boneController.viewer = mainWindow._viewer;

            mainWindow.Show();
            boneController.Show();

            Application.Run(mainWindow);
        }
    }
}
