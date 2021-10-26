using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using ReactJokes.Data;
using ReactJokes.Web.Models;

namespace ReactJokes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokesController : ControllerBase
    {
        private readonly string _connectionString;
        private const int MinutesAllowedToChange = 5;

        public JokesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpGet]
        [Route("randomjoke")]
        public Joke GetRandomJoke()
        {
            var joke = JokeApi.GetJoke();
            var jokeRepo = new JokeRepository(_connectionString);
            if (!jokeRepo.JokeExists(joke.JokeId))
            {
                jokeRepo.AddJoke(joke);
                return joke;
            }

            return jokeRepo.GetByOriginId(joke.JokeId);
        }

        [HttpGet]
        [Route("getlikescount/{jokeid}")]
        public object GetLikesCount(int jokeId)
        {
            var repo = new JokeRepository(_connectionString);
            var joke = repo.GetWithLikes(jokeId);
            return new
            {
                likes = joke.UserLikedJokes.Count(u => u.Liked),
                dislikes = joke.UserLikedJokes.Count(u => !u.Liked)
            };
        }

        [HttpGet]
        [Route("getinteractionstatus/{jokeid}")]
        public object GetInteractionStatus(int jokeId)
        {
            UserJokeInteractionStatus status = GetStatus(jokeId);
            return new { status };
        }

        [HttpPost]
        [Authorize]
        [Route("interactwithjoke")]
        public void InteractWithJoke(InteractViewModel viewModel)
        {
            var repo = new JokeRepository(_connectionString);
            var user = repo.GetByEmail(User.Identity.Name);
            repo.InteractWithJoke(user.Id, viewModel.JokeId, viewModel.Like);
        }

        [HttpGet]
        [Route("viewall")]
        public List<Joke> ViewAll()
        {
            var jokeRepo = new JokeRepository(_connectionString);
            return jokeRepo.GetAll();
        }

        private UserJokeInteractionStatus GetStatus(int jokeId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return UserJokeInteractionStatus.Unauthenticated;
            }

            var repo = new JokeRepository(_connectionString);
            var user = repo.GetByEmail(User.Identity.Name);
            UserLikedJokes likedStatus = repo.GetLikes(user.Id, jokeId);

            if(likedStatus == null)
            {
                return UserJokeInteractionStatus.NeverInteracted;
            }
            if(likedStatus.JokeTime.AddMinutes(MinutesAllowedToChange) < DateTime.Now)
            {
                return UserJokeInteractionStatus.CanNoLongerInteract;
            }

            return likedStatus.Liked ? UserJokeInteractionStatus.Liked : UserJokeInteractionStatus.Disliked;
        }
    }
}
