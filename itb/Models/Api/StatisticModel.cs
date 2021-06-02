using System;
using System.Collections.Generic;

namespace itb.Models.Api
{
    public class StatisticModel
    {
        public string Id { get; set; } = null;

        public string Username { get; set; } = null;

        public string AvatarUrl { get; set; }

        public int PostsCount { get; set; }

        public int FollowedBy { get; set; }

        public int Follows { get; set; }

        public List<StatisticsPostsModel> Posts { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
    
    public class StatisticsPostsModel
    {
        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }
    }
}