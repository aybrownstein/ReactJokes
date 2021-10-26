import React, { useEffect, useState } from "react";
import axios from "axios";

const ViewAll = () => {
    const [jokes, setJokes] = useState([]);

    useEffect(() => {
        const getJokes = async () => {
            const {data} = await axios.get('/api/jokes/viewall');
            setJokes(data);
        }
        getJokes();
    }, []);

    return(
        <div className='row'>
            <div className='col-md-6 offset-md-3'>
{jokes.map(joke => {
    return(<div key={joke.id} className=' card cardbody bg-light mb=3'>
        <h5>{joke.setup}</h5>
        <h5>{joke.punchline}</h5>
        <span>Likes: {joke.userLikedJokes.filter(j => j.like).length} </span><br/>
        <span>Dislikes: {joke.userLikedJokes.filter(j => !j.like).length} </span></div>
        )
})}
            </div>
        </div>
    )
}
export default ViewAll;