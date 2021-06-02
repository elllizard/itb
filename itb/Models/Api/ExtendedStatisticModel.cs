using System;
using System.Collections.Generic;

namespace itb.Models.Api
{
    public class ExtendedStatisticModel : StatisticModel
    {
        public int TotalLikes { get; set; }
        
        public int MinLikes { get; set; }
        
        public int MaxLikes { get; set; }
        
        public int TotalComments { get; set; }
        
        public int MinComments { get; set; }
        
        public int MaxComments { get; set; }
    }
}