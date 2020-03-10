using DRAWeb.Core.Interface;
using DRAWeb.Models;
using DRAWeb.Proxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DRAWeb.Core.Broker
{
    public class ReportBroker : IReportBroker
    {
        DRAAzureServiceProxy proxy = new DRAAzureServiceProxy();
       
        public async Task<ResponseMessage<List<UserCompetencyMatrixModel>>> GetUserCompetencyMetrix(CompetenciesReportRequest request)
        {
            ResponseMessage<List<UserCompetencyMatrixModel>> result;
            var t = Task.Run(() => proxy.GetUserCompetencyMetrix(request));
            t.Wait();
            result = t.Result;
            return result;
        }
    }
}
