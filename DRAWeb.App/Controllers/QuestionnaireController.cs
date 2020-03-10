﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRAWeb.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static DRAWeb.App.Utilities.EnumHelpers;

namespace DRAWeb.App.Controllers
{
    public class QuestionnaireController : BaseController
    {

        public QuestionnaireController(IConfiguration _config, ILoggerManager loggerManager)
        {
            logger = loggerManager;
            config = _config;
        }
        public IActionResult Index()
        {
            
            var eligibleMonths = config.GetValue<int>("AppSettings:TestEligibleAfter");
            var user = GetUserSession();
            if (user != null)
            {
                if (user.IsTestTaken)
                {
                    if (DateTime.Compare(user.LastTestTakenOn.AddMonths(eligibleMonths), DateTime.Now) < 0)
                    {
                        return View();
                    }
                    else
                    {
                        SetNotification("Last test taken on " + user.LastTestTakenOn.ToShortDateString() + " should be atleast " + eligibleMonths + " months", NotificationType.Warning, "Warning", Url.Action("Landing", "Home"));
                        return View();
                    }
                }
                else
                {
                    return View();
                }

            }
            else
            {
                return RedirectToAction("Login", "UserAccount");
            }
        }
    }
}