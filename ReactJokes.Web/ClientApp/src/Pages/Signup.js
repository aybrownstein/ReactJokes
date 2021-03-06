import React, {useState} from 'react';
import { useHistory } from 'react-router-dom';
import axios from 'axios';

const Signup = () => {
    const history = useHistory();
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password: ''
    });
    const onTextChange = e => {
        const copy = {...formData};
        copy[e.target.name] = e.target.value;
        setFormData(copy);
    }

    const onFormSubmit = async e => {
        e.preventDefault();
        await axios.post('/api/account/signup', formData);
        history.push('/login');
    }

    return(
        <div className='row'>
            <div className='col-md-6 offset-md-3 card card-body bg-light'>
                <h3>Signup for a new account</h3>
                <form onSubmit={onFormSubmit}>
                    <input onChange={onTextChange} value={formData.firstName} type='text' name='firstName' placeholder='first name' className='form-control'/>
                    <br/>
                    <input onChange={onTextChange} value={formData.lastName} type='text' name='lastName' placeholder='last name' className='form-control'/>
                    <br/>
                    <input onChange={onTextChange} value={formData.email} type='text' name='email' placeholder='email' className='form-control'/><br/>
                    <input onChange={onTextChange} value={formData.password} type='password' name='password' placeholder='password' className='form-control'/>
                    <br/>
                    <button className='btn btn-primary'>Signup</button>
                </form>
            </div>
        </div>
    );
}
export default Signup;