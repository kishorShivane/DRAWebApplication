using DRAWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DRAWeb.Core.Interface
{
    public interface IRegistrationBroker
    {
        Task<ResponseMessage<UserModel>> RegisterUser(UserModel user);
        Task<string> ActivateUser(int userID);
    }
}
