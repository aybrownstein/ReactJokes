import React, {useState, useEffect} from 'react';
import {Link} from 'react-router-dom';
import axios from 'axios';
import useInterval from '../useInterval';

const Home = () => {
    comst [joke, setJoke] = useState({
        id: '',
        setup: '',
        punchline: '',
        likesCount: '',
        dislikesCount: ''
    });
    const [userInteractionstatus, setUserInteractionStatus] = useState('');

    const updateCounts = async () => {
        const {id} = joke;
        if(!id){
            return;
        }
        const {data} = await axios.get(`/api/jokes/getlikescount/${id}`);
        setJoke({...joke, likesCount: data.likes, dislikesCount: data.dislikes});
    }

    useInterval(updateCounts, 500);

    useEffect(() => {
        const getJoke = async () =>{
            const {data} = await axios.get('/api/jokes/randomjoke');
            const {data: interactionStatus} = await axios.get(`/api/jokes/getinteractionstatus/${data.id}`);
            setJoke(data);
            setUserInteractionStatus(interactionStatus.status);
        }
        getJoke()
    },[]);

    const interactWithJoke = async like => {
        const {id} = joke;
        await axios.post(`/api/jokes/interactwithjoke`,{jokeId: id, like});
        const {data: interactionStatus} = await axios.get(`/api/jokes/getinteractionsstatus/${id}`);
        setUserInteractionStatus(interactionStatus.status);
    }

    const {setup, punchline, likesCount, dislikesCount} = joke;
   const canLike = userInteractionstatus !== 'Likes' && userInteractionstatus !== 'canNoLongerInteract';
   const canDisLike = userInteractionstatus !== 'Disliked' && userInteractionstatus !== 'canNoLongerInteract';
   return(
       <div className='row'>
           <div className="col-md-6 offset-md-3 card card-body bg-light">
               {setup && <div>
                   <h4>{setup}</h4>
                   <h4>{punchline}</h4>
                   <div>
                       {userInteractionstatus !== 'unauthenticated' && <div>
                           <button disabled={!canLike} onClick={() => interactWithJoke(true)} className='btn btn-primary'>Like</button>
                           <button disabled={!canDisLike} onClick={() => interactWithJoke(false)} className='btn btn-danger'>Dislike</button>
                           </div>}
                           {userInteractionstatus === 'Unauthenticated' && <div>
                               <Link to='/login'>Log in to your account to like/dislike this joke</Link></div>}<br/>
                               <h4>Likes: {likesCount}</h4>
                               <h4>Dislikes: {dislikesCount}</h4>
                               <h4>
                                   <button className='btn btn-link' onClick={() => window.location.reload()}>Refresh</button>
                               </h4>
</div>
      </div>         }
      {!setup && <h3>Loading...</h3>}
           </div>
       </div>
   )
}
export default Home;