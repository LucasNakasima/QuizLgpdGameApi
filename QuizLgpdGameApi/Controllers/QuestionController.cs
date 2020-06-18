using QuizLgpdGameApi.Entities;
using QuizLgpdGameApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace QuizLgpdGameApi.Controllers
{
    [RoutePrefix("api/question")]
    public class QuestionController : ApiController
    {
        [Route("list-questions")]
        public IHttpActionResult GetQuestions()
        {
            try
            {
                using (var context = new gamequizlgpdEntities())
                {
                    var viewModel = new ListQuestionsModel();

                    var iziQuestions = context.Questoes
                        .Where(p => p.NiveisDificuldadesId == 1 || p.NiveisDificuldadesId == 2 || p.NiveisDificuldadesId == 3)
                        .Include(p => p.Respostas)
                        .Take(10);

                    viewModel.ListQuestions = iziQuestions.Select(p => new QuestionModel
                    {
                        Id = p.Id,
                        Question = p.Questao,
                        ListAnswers = p.Respostas.Select(x => new AnswerModel
                        {
                            Id = x.Id,
                            Answer = x.Resposta,
                            AnswerCorrect = x.RespostaCorreta,
                        }).ToList(),
                    }).ToList();

                    return Ok(viewModel);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("validate-answer")]
        public async Task<IHttpActionResult> ValidateAnswer(int answerId, int userId)
        {
            try
            {
                using (var context = new gamequizlgpdEntities())
                {
                    if (userId == 0)
                        return BadRequest("Usuario invalido");

                    if (answerId == 0)
                        return BadRequest("Alternativa invalida");

                    var answerChosen = await context.Respostas.FirstOrDefaultAsync(p => p.Id == answerId);

                    if (answerChosen == null)
                        return BadRequest("Alternativa não encontrada");

                    //salva a resposta no banco
                    var answerUserModel = new RepostasUsuarios
                    {
                        QuestaoId = answerChosen.QuestaoId,
                        RespostaId = answerId,
                        UsuarioId = userId,
                    };

                    context.Entry(answerUserModel).State = EntityState.Added;
                    context.SaveChanges();

                    //valida se esta correta a resposta escolhida
                    var model = new ValidateAnswerModel();

                    if (answerChosen.RespostaCorreta)
                    {
                        AddScoreUser(userId, answerChosen.QuestaoId);

                        model.IsCorrectAnswer = true;
                        model.LifeUser = await UserLife(userId);
                    }
                    else
                    {
                        DamageLifeUser(userId);

                        model.IsCorrectAnswer = false;
                        model.LifeUser = await UserLife(userId);
                    }

                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("life-user")]
        public async Task<int> UserLife(int userId)
        {
            using (var context = new gamequizlgpdEntities())
            {
                var lifeUser = context.Usuarios.FirstOrDefault(p => p.Id == userId).Vida;

                return Convert.ToInt32(lifeUser);
            }
        }

        [Route("user-damage")]
        public void DamageLifeUser(int userId)
        {
            using (var context = new gamequizlgpdEntities())
            {
                var user = context.Usuarios.FirstOrDefault(p => p.Id == userId);

                if (user.Vida > 0)
                {
                    user.Vida = user.Vida - 1;

                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        [Route("add-score")]
        public void AddScoreUser(int userId, int questionId)
        {            
            using (var context = new gamequizlgpdEntities())
            {
                var user = context.Usuarios.FirstOrDefault(p => p.Id == userId);
                var questionPoints = context.Questoes.FirstOrDefault(p => p.Id == questionId).NiveisDificuldades.Pontos;

                user.Pontos = user.Pontos + questionPoints;

                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}