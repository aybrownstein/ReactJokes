import React, {useState, useEffect, createContext, useContext} from 'react';
import axios from 'axios';

const AuthContext = createContext();

const AuthContextComponent = ({children}) => {
    const [user, setUser] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const getUser = async () => {
            const {data} = await axios.get('/api/account/getcurrentuser');
            setUser(data);
            setIsLoading(false);
        }
        getUser();
    }, []);

    const logout = () => {
        setUser(null);
    }

    const value = {
        setUser,
        user,
        logout
    }

    return (
        <AuthContext.Provider value={value}>
            {isLoading ? <span></span>: children}
        </AuthContext.Provider>
    )
}

const useAuthContext = () => useContext(AuthContext);
export {useAuthContext, AuthContextComponent};