using System;
using NLog;
using System.Configuration;

namespace OutlookSignature
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                UserObj user = GetUser();
                string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
                logger.Info("appDataDir : {0}", appDataDir);

                Template template = new Template();
                template.update(user, appDataDir);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
            finally
            {
                //System.Environment.Exit(0);
            }            
        }

        static UserObj GetUser()
        {
            UserObj user = null;
            String source = ConfigurationManager.AppSettings.Get("UserSource");
            if (source == "AD")
            {
                logger.Info("User Source is Active Directory");
                ADUser adUser = new ADUser();
                user = adUser.get();
            }
            else
            {
                logger.Info("User Source is Desktop");
            }
           
            return user;
        }
    }
}
