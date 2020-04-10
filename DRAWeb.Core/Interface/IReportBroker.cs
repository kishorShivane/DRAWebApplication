using DRAWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRAWeb.Core.Interface
{
    public interface IReportBroker
    {
        Task<ResponseMessage<List<UserCompetencyMatrixModel>>> GetUserCompetencyMetrix(CompetenciesReportRequest request);
    }
}
