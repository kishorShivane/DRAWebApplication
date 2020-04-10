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
    public class UserBroker : BrokerBase, IUserBroker
    {
        public UserBroker(IConfiguration _config, ILoggerManager loggerManager, IDRAAzureServiceProxy _proxy)
        {
            logger = loggerManager;
            config = _config;
            proxy = _proxy;
        }

        public async Task<ResponseMessage<UserModel>> ValidateUserCredentials(UserModel user)
        {
            return await Task.Run(() => proxy.ValidateUserCredentials(user));
        }

        public async Task<string> UpdatePassword(ResetPassword user)
        {
            return await Task.Run(() => proxy.UpdatePassword(user));
        }
    }
}
