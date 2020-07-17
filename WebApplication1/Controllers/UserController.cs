using System;
using System.Collections.Generic;
using System.Linq;
using MailKit;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Cryptography;
using System.Web.DynamicData;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private object smtpdeliverymethod;

        public ActionResult ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ForgetPassword(string emailId)
        {

            //verify email id
            // generate link
            //send mail

            string message = "";
            bool status = false;
            ListOfemailsWithToken AllEmailDetails = new ListOfemailsWithToken();

            string KeyInfo = AllEmailDetails.VerificationMailAvilabeOrNot(emailId);

            if (KeyInfo != "" && KeyInfo != null)
            {
                SendVerificationLinkEmail(emailId, KeyInfo);
                message = "Reset password link has been sent to your email id.";
            }
            else
            {
                message = "not found mail";
            }
            ViewBag.Message = message;
            return View();

        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {

            var crypt = new SHA256Managed();
            string encrypted = String.Empty;
            byte[] crypto = crypt.ComputeHash(System.Text.Encoding.ASCII.GetBytes(activationCode));
            foreach (byte theByte in crypto)
            {
                encrypted += theByte.ToString("x2");
            }

            // salt solution need to add


            var verifyUrl = "/User/" + "ResetPassword" + "/" + encrypted+ "VAM";
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            string subject = "Reset Password";
            string body = "Hi,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";


            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("test@gmail.com", "test@gmail.com"));
            message.Subject = subject;
            message.To.Add(new MailboxAddress(emailID, emailID));

            message.Body = new TextPart("html")
            {
                Text = body
            };
            using (var client = new SmtpClient())
            {
                client.Connect("outlook.office365.com", 587, false);
                client.Authenticate("test@gmail.com", "password");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        public ActionResult ResetPassword(string id)
        {

           

            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            if (id != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {


            //var crypt = new System.Security.Cryptography.SHA256Managed();
            //var decrypted = new System.Text.StringBuilder();
            //byte[] crypto = crypt.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.ResetCode));
            //foreach (byte theByte in crypto)
            //{
            //    decrypted.Append(theByte.ToString("x2"));

            //}
            //byte[] b;
            //string decrypted;
            //b = Convert.FromBase64String(model.ResetCode);
            //decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);

            var message = "";
            ListOfemailsWithToken AllEmailDetails = new ListOfemailsWithToken();
            if (ModelState.IsValid && model.ResetCode != null && AllEmailDetails.verifyTimeConstraints(model.ResetCode))
            {

                //  update key as null
                //    update  password


                message = "New password updated successfully";

            }
            else
            {
                message = "Something invalid or time Constraints";
            }
            ViewBag.Message = message;
            return View(model);
        }
    }
}