using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuizLgpdGameApi.Models
{
    public class RankingModel
    {
        public RankingModel()
        {
            ListUsers = new List<UserModel>();
        }

        public List<UserModel> ListUsers { get; set; }
    }
}