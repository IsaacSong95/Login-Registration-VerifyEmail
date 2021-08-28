using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Testing_login.Models;

namespace Testing_login.Controllers
{
    public class UserController : Controller
    {
        private object smtpDeliveryMethod;

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }


                #region Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified = false;

                #region Save to Database
                using (MydatabaseEntities dc = new MydatabaseEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();

                    SendVerificationLinkEmail(user.EmailID, user.ActivationCode);

                    ; }
                #endregion

            }
            else
            {
                message = "Invaild Request";
            }


            return View(user);
        }

        private void SendVerificationLinkEmail(string emailID, Guid activationCode)
        {
            throw new NotImplementedException();
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (MydatabaseEntities dc = new MydatabaseEntities())
            {
                var V = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return V != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("songlilun@live.com", "Isaac");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "***********";
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your account" +
             "sucessfully created.Please click on the below link to verify your account" +
             "<br/><br/><a href='" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "songlilun@live.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = smtpDeliveryMethod.Network,
                UseDefaultCredentals = false;
            Creadentails = new NetworkCredentail(fromEmail.Address, fromEmailPassword)
            };
    }

          using (var message = new MailMessage(fromEmail, toEmail))
          {
            Subject = subject,
            Body = body,
            IsBodyHtml = true
           
        }

    }
}