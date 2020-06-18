using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuizLgpdGameApi.Models
{
    public class QuestionModel
    {
        public QuestionModel() 
        {
            ListAnswers = new List<AnswerModel>();
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }     
    
        public List<AnswerModel> ListAnswers { get; set; }
    }
}