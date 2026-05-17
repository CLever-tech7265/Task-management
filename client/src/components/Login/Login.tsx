// components/Login.tsx
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { setUser } from '../../store';

const Login: React.FC = () => {
  const dispatch = useDispatch();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = () => {
    dispatch(setUser({ username, password }));
    // כאן ניתן להוסיף ניווט נוסף לאחר שמירה
  };

  return (
    <div>
      <h1>Login</h1>
      <input
        type="text"
        placeholder="Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <button onClick={handleLogin}>כניסה</button>
    </div>
  );
};

export default Login;