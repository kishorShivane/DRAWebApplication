using DRAWeb.Logger;
using DRAWeb.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DRAWeb.Proxy
{
    public interface IDRAAzureServiceProxy
    {
        Task<ResponseMessage<List<UserCompetencyMatrixModel>>> GetUserCompetencyMetrix(CompetenciesReportRequest reportRequest);
        Task<ResponseMessage<UserModel>> ValidateUserCredentials(UserModel user);
        Task<ResponseMessage<UserModel>> RegisterUser(UserModel user);
        Task<string> UpdatePassword(ResetPassword user);
        Task<string> ActivateUser(int userID);
    }
}
