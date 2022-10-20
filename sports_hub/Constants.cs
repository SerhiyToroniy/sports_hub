using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub
{
    public static class Constants
    {
        public static string SmtpServerAddress = "smtp.gmail.com";
        public static int SmtpServerPort = 465;
        public static string SmtpServerEmail = "sports.hub.project@gmail.com";
        public static string SmtpServerPassword = "SportsHub0208";
        public static string ConfirmEmailSubject = "Confirm your account";
        public static string EnConfirmEmailFilePath = "/Resources/Views/Account/email_confirmation.en.html";
        public static string UkConfirmEmailFilePath = "/Resources/Views/Account/email_confirmation.uk.html";
        public static string ResetPasswordEmailSubject = "Reset Password";
        public static string EnResetPasswordEmailFilePath = "/Resources/Views/Account/reset_email.en.html";
        public static string UkResetPasswordEmailFilePath = "/Resources/Views/Account/reset_email.uk.html";
        public static string EnglishDateTime = DateTime.Now.ToString("MMMM d, yyyy");
        public static string UkrainianDateTime = DateTime.Now.ToString("d MMMM, yyyy");
        public static string ShareButtonsURL = "https://www.google.com.ua";
        public static string EnCookieName = "c=en|uic=en";
        public static string UkCookieName = "c=uk|uic=uk";
        public static int MaxBreakDownArticles = 4;
        public static int minCommentsCount = 3;
        public static int maxCommentsCount = 20;
        public static int PageSizeForArticles = 3;
        public static int InitialPageNumber = 1;
        public static int ToSearch = 5;
    }
}
