using DRAWeb.Core.Interface;
using DRAWeb.Logger;
using DRAWeb.Models;
using DRAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DRAWeb.Core.Broker
{
    public class RegistrationBroker : BrokerBase, IRegistrationBroker
    {
        public RegistrationBroker(IConfiguration _config, ILoggerManager loggerManager, IDRAAzureServiceProxy _proxy)
        {
            logger = loggerManager;
            config = _config;
            proxy = _proxy;
        }

        public async Task<ResponseMessage<UserModel>> RegisterUser(UserModel user)
        {
            return await Task.Run(() => proxy.RegisterUser(user));
        }

        public async Task<string> ActivateUser(int userID)
        {
            return await Task.Run(() => proxy.ActivateUser(userID));
        }
    }
}
