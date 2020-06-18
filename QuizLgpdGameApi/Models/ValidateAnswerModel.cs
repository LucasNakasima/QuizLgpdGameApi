using Newtonsoft.Json;

namespace QuizLgpdGameApi.Models
{
    public class ValidateAnswerModel
    {
        [JsonProperty("lifeUser")]
        public int LifeUser { get; set; }

        [JsonProperty("isCorrectAnswer")]
        public bool IsCorrectAnswer { get; set; }
    }
}