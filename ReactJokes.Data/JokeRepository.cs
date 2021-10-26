using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ReactJokes.Data
{
    public class JokeRepository
    {
        private readonly string _connectionString;

        public JokeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            using var context = new JokeContext(_connectionString);
            user.PasswordHash = hash;
            context.Users.Add(user);
            context.SaveChanges();
        }

        public User GetByEmail(string email)
        {
            using var context = new JokeContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if(user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isValidPassword)
            {
                return user;
            }
            return null;
        }

       public bool JokeExists(int jokeId)
        {
            using var context = new JokeContext(_connectionString);
            return context.Jokes.Any(j => j.JokeId == jokeId);
        }

        public Joke GetByOriginId(int jokeId)
        {
            using var context = new JokeContext(_connectionString);
            return context.Jokes.FirstOrDefault(j => j.JokeId == jokeId);
        }

        public void AddJoke(Joke joke)
        {
            using var context = new JokeContext(_connectionString);
            context.Jokes.Add(joke);
            context.SaveChanges();
        }

        public UserLikedJokes GetLikes(int userId, int jokeId)
        {
            using var context = new JokeContext(_connectionString);
            return context.UserLikedJokes.FirstOrDefault(u => u.UserId == userId && u.JokeId == jokeId);
        }

        public void InteractWithJoke(int userId, int jokeId, bool like)
        {
            using var context = new JokeContext(_connectionString);
            var userLike = context.UserLikedJokes.FirstOrDefault(u => u.UserId == userId && u.JokeId == jokeId);
            if(userLike == null)
            {
                context.UserLikedJokes.Add(new UserLikedJokes
                {
                    UserId = userId,
                    JokeId = jokeId,
                    Liked = like,
                    JokeTime = DateTime.Now
                });
            }
            else
            {
                userLike.Liked = like;
                userLike.JokeTime = DateTime.Now;
            }
            context.SaveChanges();
        }

        public Joke GetWithLikes(int jokeId)
        {
            using var context = new JokeContext(_connectionString);
            return context.Jokes.Include(u => u.UserLikedJokes).FirstOrDefault(j => j.Id == jokeId);
        }

        public List<Joke> GetAll()
        {
            using var context = new JokeContext(_connectionString);
            return context.Jokes.Include(j => j.UserLikedJokes).ToList();
        }
    }
}
