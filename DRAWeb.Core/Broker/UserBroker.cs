using DRAWeb.Core.Interface;
using DRAWeb.Models;
using DRAWeb.Proxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DRAWeb.Core.Broker
{
    public class UserBroker : IUserBroker
    {
        DRAAzureServiceProxy proxy = new DRAAzureServiceProxy();

        public async Task<ResponseMessage<UserModel>> ValidateUserCredentials(UserModel user)
        {
            ResponseMessage<UserModel> result;
            //var t = Task.Run(() => proxy.ValidateUserCredentials(user));
            //t.Wait();
            //result = t.Result;
            result = await Task.Run(() => proxy.ValidateUserCredentials(user));
            return result;
        }

        public async Task<string> UpdatePassword(ResetPassword user)
        {
            string result;
            result = await Task.Run(() => proxy.UpdatePassword(user));
            //var t = Task.Run(() => proxy.UpdatePassword(user));
            //t.Wait();
            //result = t.Result;
            return result;
        }
    }
}
