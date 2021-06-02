using System.Collections.Generic;
using System.Threading.Tasks;
using itb.Models.Api;

namespace itb.Services.Statistic
{
    public interface IStatisticService
    {
        public Task<List<StatisticModel>> ReadAsync(string username1, string? username2 = "");
        public List<ExtendedStatisticModel> CalculateStatistic(List<StatisticModel> statistics);
    }
}