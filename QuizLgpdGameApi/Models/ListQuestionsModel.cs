using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuizLgpdGameApi.Models
{
    public class ListQuestionsModel
    {
        public ListQuestionsModel()
        {
            ListQuestions = new List<QuestionModel>();
        }

        public List<QuestionModel> ListQuestions { get; set; }
    }
}