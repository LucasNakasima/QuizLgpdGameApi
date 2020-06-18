using Newtonsoft.Json;

namespace QuizLgpdGameApi.Models
{
    public class UserModel
    {        
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("life")]
        public int Life { get; set; }

        [JsonProperty("points")]
        public int? Points { get; set; }
    }
}