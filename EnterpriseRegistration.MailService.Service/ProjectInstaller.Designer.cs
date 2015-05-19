﻿namespace EnterpriseRegistration.MailService.Service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            // 
            // serviceProcessInstaller1
            // 
            serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            serviceProcessInstaller1.Password = null;
            serviceProcessInstaller1.Username = null;
            serviceProcessInstaller1.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "Fetch mails from configured mailbox and store them in database.";
            this.serviceInstaller1.DisplayName = "Enterprise Registration Mail Processor";
            this.serviceInstaller1.ServiceName = "FetchMailService";
            this.serviceInstaller1.ServicesDependedOn = new string[] {
        "Server"};
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.serviceInstaller1.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            serviceProcessInstaller1,
            this.serviceInstaller1});

        }

        #endregion

        public System.ServiceProcess.ServiceInstaller serviceInstaller1;

    }
}