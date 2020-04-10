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
    public class ReportBroker : BrokerBase, IReportBroker
    {
        public ReportBroker(IConfiguration _config, ILoggerManager loggerManager, IDRAAzureServiceProxy _proxy)
        {
            logger = loggerManager;
            config = _config;
            proxy = _proxy;
        }

       
        public async Task<ResponseMessage<List<UserCompetencyMatrixModel>>> GetUserCompetencyMetrix(CompetenciesReportRequest request)
        {
            return await Task.Run(() => proxy.GetUserCompetencyMetrix(request));
        }
    }
}
