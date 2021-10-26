using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReactJokes.Data
{
    public static class JokeApi
    {
        private class WeirdJoke {

            public int Id { get; set; }
            public string Setup { get; set; }
            public string Punchline { get; set; }
        }
        
   

    public static Joke GetJoke()
    {
        var client = new HttpClient();
        var json = client.GetStringAsync("https://official-joke-api.appspot.com/jokes/programming/random").Result;
        var result = JsonConvert.DeserializeObject<List<WeirdJoke>>(json).FirstOrDefault();
            return new Joke
            {
                JokeId = result.Id,
                Setup = result.Setup,
                Punchline = result.Punchline
            };
    }

    }
}
