using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using itb.Models.Api;
using itb.Models.Configurtations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace itb.Services.Statistic
{
    public class StatisticService : IStatisticService
    {
        private readonly ILogger<StatisticService> _logger;
        private readonly ApplicationConfiguration _applicationConfig;

        public StatisticService(
            ILogger<StatisticService> logger,
            IOptions<ApplicationConfiguration> applicationConfig
        )
        {
            _logger = logger;
            _applicationConfig = applicationConfig.Value;
        }

        public async Task<List<StatisticModel>> ReadAsync(string username1, string? username2 = "")
        {
            _logger.LogInformation(
                $"Making read request for statistic with username '{username1}' and username '{username2}'."
                );

            StringBuilder _url = new StringBuilder();
            _url.Append(_applicationConfig.ApiUrl);
            _url.Append("/api/statistics/[username1]/[username2]");
            _url.Replace("[username1]", username1);
            _url.Replace("[username2]", username2);

            using HttpClient _httpClient = new HttpClient();

            HttpResponseMessage _result = await _httpClient.GetAsync(_url.ToString());

            if (_result.StatusCode == HttpStatusCode.OK)
            {
                string _response = await _result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<StatisticModel>>(_response);
            }

            throw new Exception(
                $"Failed making read request for statistic with username '{username1}' and username '{username2}'. Got {_result.StatusCode} status code."
            );
        }

        public List<ExtendedStatisticModel> CalculateStatistic(List<StatisticModel> statistics)
        {
            return statistics.ConvertAll<ExtendedStatisticModel>(statistic =>
            {
                int _totalLikes = statistic.Posts.Aggregate(0, (accum, post) => accum += post.LikesCount);
                int _minLikes = statistic.Posts.Min(post => post.LikesCount);
                int _maxLikes = statistic.Posts.Max(post => post.LikesCount);
                int _totalComments = statistic.Posts.Aggregate(0, (accum, post) => accum += post.CommentsCount);
                int _minComments = statistic.Posts.Min(post => post.CommentsCount);
                int _maxComments = statistic.Posts.Max(post => post.CommentsCount);
                
                return new ExtendedStatisticModel()
                {
                    Id = statistic.Id,
                    Username = statistic.Username,
                    AvatarUrl = statistic.AvatarUrl,
                    PostsCount = statistic.PostsCount,
                    FollowedBy = statistic.FollowedBy,
                    Follows = statistic.Follows,
                    TotalLikes = _totalLikes,
                    MinLikes = _minLikes,
                    MaxLikes = _maxLikes,
                    TotalComments = _totalComments,
                    MinComments = _minComments,
                    MaxComments = _maxComments,
                    CreatedAt = statistic.CreatedAt,
                    UpdatedAt = statistic.UpdatedAt
                };
            });
        }
    }
}