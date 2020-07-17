using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace WebApplication1.Models
{
    public class ListOfemailsWithToken
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        String[,] MailWithDetails;

        public ListOfemailsWithToken()
        {
            dict.Add("test@gmail.com", "9245fe4a-d402-451c-b9ed-9c1a04247482");
            dict.Add("dsfdfdsf", "Two");
            dict.Add("sdfsdfdsfsdfsdf", "Three");
            MailWithDetails = new string[2, 3] {
                { "test@valuemomentum.com","9245fe4a-d402-451c-b9ed-9c1a04247482","30-06-2020 09:37:09"},
                 { "test@gmail.com","924666a-d402-451c-b9ed-9c1a04247482","30-06-2020 09:44:09"}
            };
        }



        public string VerificationMailAvilabeOrNot(string EmailId)
        {
            string activationCode = "";

            if (MailWithDetails.Length > 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (MailWithDetails[i, 0] == EmailId)
                    {
                        activationCode =  MailWithDetails[i, 1].ToString() + MailWithDetails[i, 2].ToString();
                        break;
                    }
                   
                }
            }
            else
            {
                activationCode = "";
            }

            return activationCode;
        }


        public bool verifyTimeConstraints(string activationCodeInput)
        {
            bool verified = false;

            if (MailWithDetails.Length > 0)
            {
                for (int i = 0; i < 2; i++)
                {

                    var crypt = new SHA256Managed();
                    string encrypted = String.Empty;
                    byte[] crypto = crypt.ComputeHash(System.Text.Encoding.ASCII.GetBytes(MailWithDetails[i, 1].ToString() + MailWithDetails[i, 2].ToString()));
                    foreach (byte theByte in crypto)
                    {
                        encrypted += theByte.ToString("x2");
                    }
                    if (encrypted + "VAM" == activationCodeInput)
                    {
                        var currentdateTimeInUtc = DateTime.UtcNow;
                        var requestedTime = DateTime.Parse(MailWithDetails[i, 2].ToString());
                        if ((currentdateTimeInUtc - requestedTime).TotalMinutes >  5)
                        {
                            verified = true;
                            break;
                        }
                    }
                }
            }
            return verified;

        }
    }
}