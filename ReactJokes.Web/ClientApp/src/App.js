import React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { AuthContextComponent } from './AuthContext';
import Home from './Pages/Home';
import Signup from './Pages/Signup';
import Login from './Pages/Login';
import ViewAll from './Pages/VieAll';
import Logout from './Pages/Logout';

const App = () => {
  return (
    <AuthContextComponent>  <Layout>
    <Route exact path='/' component={Home} />
   <Route exact path='/signup' component={Signup}/>
   <Route exact path='/login' component={Login}/>
   <Route exact path='/vieall' component={ViewAll}/>
   <Route exact path='/logout' component={Logout}/>
  </Layout></AuthContextComponent>
  
  );
}
export default App;

  
    
  
