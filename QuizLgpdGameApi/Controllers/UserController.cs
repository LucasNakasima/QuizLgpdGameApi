using QuizLgpdGameApi.Entities;
using QuizLgpdGameApi.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace QuizLgpdGameApi.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [Route("register-user")]
        public async Task<IHttpActionResult> RegisterUser(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return BadRequest("Apelido inválido");

                if (name.Length != 3)
                    return BadRequest("O apelido deve conter exatos 3 caracteres");

                bool specialCharacters = Regex.IsMatch(name, (@"[^a-zA-Z0-9]"));
                if (specialCharacters)
                    return BadRequest("Não é permitido uso de caracteres especiais");

                using (var context = new gamequizlgpdEntities())
                {
                    var userModel = new Usuarios
                    {
                        Nome = name,
                        Pontos = 0,
                        Vida = 3,
                    };
                    
                    context.Entry(userModel).State = EntityState.Added;
                    context.SaveChanges();

                    return Ok(userModel.Id);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("list-ranking")]
        public IHttpActionResult GetRanking()
        {
            try
            {
                using (var context = new gamequizlgpdEntities())
                {
                    var viewModel = new RankingModel();

                    viewModel.ListUsers = context.Usuarios
                        .Where(p => p.Pontos != 0)
                        .OrderByDescending(p => p.Pontos)
                        .Select(p => new UserModel 
                        { 
                            Id = p.Id,
                            Name = p.Nome,
                            Points = p.Pontos,
                        })
                        .Take(10).ToList();

                    return Ok(viewModel);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("get-score")]
        public IHttpActionResult GetUserScore(int userId)
        {
            try
            {
                using (var context = new gamequizlgpdEntities())
                {
                    var userScore = context.Usuarios.FirstOrDefault(p => p.Id == userId).Pontos;

                    return Ok(userScore);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}