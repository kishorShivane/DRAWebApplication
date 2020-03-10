using DRAWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DRAWeb.Core.Interface
{
    public interface IUserBroker
    {
        Task<ResponseMessage<UserModel>> ValidateUserCredentials(UserModel user);

        Task<string> UpdatePassword(ResetPassword user);
    }
}
