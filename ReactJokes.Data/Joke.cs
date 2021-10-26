using System;
using System.Collections.Generic;

namespace ReactJokes.Data
{
    public class Joke
    {
        public int Id { get; set; }
        public int JokeId { get; set; }
        public string Setup { get; set; }
        public string Punchline { get; set; }

        public List<UserLikedJokes> UserLikedJokes { get; set; }
    }
}
