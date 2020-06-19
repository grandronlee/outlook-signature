using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace OutlookSignature
{
    class Template
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private string path = "";
        private string name = "";

        public Template ()
        {
            path = ConfigurationManager.AppSettings.Get("TemplatePath");
            if (Directory.Exists(path))
            {
                // This path is a directory
                logger.Info("{0} is a valid directory.", path);
            }
            else
            {                
                throw new Exception(path + " not a valid directory.");
            }
            name = ConfigurationManager.AppSettings.Get("TemplateName"); 
        }

        public void update(UserObj user, string appDataDir)
        {
            if (appDataDir != null)
            {
                // copy htm rtf txt files to target dir
                string sourceDir = path + "\\" + name;                
                string[] fileEntries = Directory.GetFiles(sourceDir);
                foreach (string fileName in fileEntries)
                {
                    logger.Info("file found: {0}", fileName);                    
                    // Remove path from the file name.
                    string fName = fileName.Substring(sourceDir.Length + 1);
                    // copy files to target dir
                    File.Copy(Path.Combine(sourceDir, fName), Path.Combine(appDataDir, fName), true);
                }

                // copy name_files directory to target
                string subDir = appDataDir + "\\" + name + "_files";
                if (!Directory.Exists(subDir))
                {
                    logger.Info("Directory created: {0}", subDir);
                    Directory.CreateDirectory(subDir);
                }

                string sourceDir2 = path + "\\" + name + "\\" + name + "_files";
                string[] fileEntries2 = Directory.GetFiles(sourceDir2);
                foreach (string fileName in fileEntries2)
                {
                    logger.Info("file found: {0}", fileName);
                    // Remove path from the file name.
                    string fName = fileName.Substring(sourceDir2.Length + 1);
                    // copy files to target dir
                    File.Copy(Path.Combine(sourceDir2, fName), Path.Combine(subDir, fName), true);
                }

                //update user info
                string[] fileEntries3 = Directory.GetFiles(appDataDir, name+"*");
                foreach (string fileName in fileEntries3)
                {
                    logger.Info("file found: {0}", fileName);
                    string text = File.ReadAllText(fileName);
                    text = text.Replace("GetProp(displayName)", user.DisplayName);
                    text = text.Replace("GetProp(title)", user.Title);
                    text = text.Replace("GetProp(telephoneNumber)", user.Phone);
                    text = text.Replace("GetProp(mobile)", user.Mobile);
                    text = text.Replace("GetProp(Mail)", user.Email);
                    File.WriteAllText(fileName, text);
                    logger.Info("User {0} template udpated", user.DisplayName);
                }
            }
            else
            {
                throw new Exception("appDataDir is empty");
            }

        }
    }
}
