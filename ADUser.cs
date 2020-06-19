using NLog;
using System;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace OutlookSignature
{
    class ADUser : UserInt 
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public UserObj get() 
        {
            // enter AD settings  
            PrincipalContext AD = new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings.Get("AD"));
            UserPrincipal u = new UserPrincipal(AD);
            UserObj user = new UserObj();
            logger.Info("AD: {0}", ConfigurationManager.AppSettings.Get("AD"));

            // search for user
            u.SamAccountName = Environment.UserName;
            PrincipalSearcher search = new PrincipalSearcher(u);
            UserPrincipal result = (UserPrincipal)search.FindOne();
            search.Dispose();
            // show some details  
            logger.Info("Display Name: {0}", result.DisplayName);
            user.DisplayName = result.DisplayName;
            logger.Info("Email: {0}", result.EmailAddress);
            user.Email = result.EmailAddress;

            string phone = result.VoiceTelephoneNumber;
            if (phone != null)
            {
                logger.Info("Phone Number : {0}", phone);
                try
                {
                    user.Phone = result.VoiceTelephoneNumber;
                }
                catch (FormatException e)
                {
                    logger.Error("Phone Number FormatException: {0}", phone);
                    throw new FormatException(e.Message);
                }
            }            

            DirectoryEntry d = (DirectoryEntry)result.GetUnderlyingObject();
            string title = d.Properties["Title"]?.Value?.ToString();
            if (title != null)
            {
                logger.Info("Title : {0}", title);
                user.Title = title;
            }

            string mobile = d.Properties["mobile"]?.Value?.ToString();
            if (mobile != null)
            {
                logger.Info("Mobile : {0}", mobile);
                user.Mobile = mobile;
            }
            
            return user;
        }
    }
}
