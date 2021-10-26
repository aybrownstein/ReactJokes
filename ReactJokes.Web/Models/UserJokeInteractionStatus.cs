using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactJokes.Web.Models
{
    public enum UserJokeInteractionStatus
    {
        Unauthenticated,
        Liked,
        Disliked,
        NeverInteracted,
        CanNoLongerInteract
    }
}
