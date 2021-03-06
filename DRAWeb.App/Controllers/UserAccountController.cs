﻿using DRAWeb.App.Utilities;
using DRAWeb.Core.Interface;
using DRAWeb.Logger;
using DRAWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using static DRAWeb.App.Utilities.EnumHelpers;

namespace DRAWeb.App.Controllers
{
    public class UserAccountController : BaseController
    {
        IUserBroker userService = null;
        public UserAccountController(IConfiguration _config,IUserBroker userBroker, ILoggerManager loggerManager)
        {
            logger = loggerManager;
            userService = userBroker;
            config = _config;
        }


        public IActionResult Login()
        {
            //logger.LogInfo("Hello, world!");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel user)
        {

            var validUser = await userService.ValidateUserCredentials(user);
            if (validUser.Content != null)
            {
                if (validUser.Content.UserActive == 1)
                {
                    SetUserSession(validUser.Content);
                    return RedirectToAction("Landing", "Home");
                }
                else
                {
                    var userModel = validUser.Content;
                    var activationURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/{"Registration"}/{"Activate"}?{"userID="}{userModel.UserID}";
                    var notificationSent = await NotificationHelper.SendRegisterNotification(userModel.UserEmail, userModel.UserName, activationURL, config);
                    if (!notificationSent)
                    {
                        logger.LogError("Sending notification failed for userID:" + user.UserID);
                    }
                    return RedirectToAction("Activate", "UserAccount");
                }
            }
            else
            {
                SetNotification(validUser.Message,NotificationType.Failure,"Failed");
                return View();
            }
        }

        public IActionResult Logout()
        {
            var user = GetUserSession();
            if (user != null)
            {
                DestroyUserSession();
            }
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ResetPassword user)
        {
            var message = await userService.UpdatePassword(user);
            if (message == "Updated")
            {
                var redirectURL = Url.Action("Login", "UserAccount");
                SetNotification("Password Updated Successfully!!",NotificationType.Success,"Success", redirectURL);
                return View();
            }
            else
            {
                SetNotification(message, NotificationType.Failure, "Failed");
                return View();
            }
        }

        public IActionResult Activate()
        {
            return View();
        }
    }
}