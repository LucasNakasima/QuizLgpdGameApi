using Newtonsoft.Json;

namespace QuizLgpdGameApi.Models
{
    public class AnswerModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("answerCorrect")]
        public bool AnswerCorrect { get; set; }
    }
}