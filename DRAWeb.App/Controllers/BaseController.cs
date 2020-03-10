using DRAWeb.Logger;
using DRAWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static DRAWeb.App.Utilities.EnumHelpers;

namespace DRAWeb.App.Controllers
{
    public class BaseController : Controller
    {
        public ILoggerManager logger;
        public IConfiguration config;
        public BaseController()
        {

        }

        public void SetUserSession(UserModel user)
        {
            const string sessionKey = "UserSession";
            DestroyUserSession();
            TempData.Add(sessionKey, JsonConvert.SerializeObject(user));
            TempData.Keep();
            //var serialisedData = JsonConvert.SerializeObject(user);
            //HttpContext.Session.SetString(sessionKey, serialisedData);
        }

        public UserModel GetUserSession()
        {
            const string sessionKey = "UserSession";
            var value = (string)TempData.Peek(sessionKey);
            //var value = HttpContext.Session.GetString(sessionKey);
            if (value != null)
                return JsonConvert.DeserializeObject<UserModel>(value);
            return null;
        }

        public void DestroyUserSession()
        {
            const string sessionKey = "UserSession";
            if (TempData.Keys.Contains("UserSession"))
                TempData.Remove(sessionKey);
            //HttpContext.Session.Remove(sessionKey);
        }

        public void SetNotification(string message, NotificationType type = NotificationType.Success, string title = "Success", string redirectURL = "")
        {
            TempData["Notification"] = JsonConvert.SerializeObject(new { Message = message, Type = type, Title = title, Redirection = redirectURL });
        }
    }
}