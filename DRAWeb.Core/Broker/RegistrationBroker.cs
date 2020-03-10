using DRAWeb.Core.Interface;
using DRAWeb.Models;
using DRAWeb.Proxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DRAWeb.Core.Broker
{
    public class RegistrationBroker : IRegistrationBroker
    {
        DRAAzureServiceProxy proxy = new DRAAzureServiceProxy();
       
        public async Task<ResponseMessage<UserModel>> RegisterUser(UserModel user)
        {
            ResponseMessage<UserModel> result;
            result = await Task.Run(() => proxy.RegisterUser(user));
            //var t = Task.Run(() => proxy.RegisterUser(user));
            //t.Wait();
            //result = t.Result;
            return result;
        }

        public async Task<string> ActivateUser(int userID)
        {
            string result;
            result = await Task.Run(() => proxy.ActivateUser(userID));
            //var t =  Task.Run(() => proxy.ActivateUser(userID));
            //t.Wait();
            //result = t.Result;
            return result;
        }
    }
}
