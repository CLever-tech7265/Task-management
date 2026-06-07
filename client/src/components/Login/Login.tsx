import React, { useEffect, useState } from 'react';
import './Login.css';

import { useDispatch, useSelector } from 'react-redux';
import type { AppDispatch, RootState } from '../../store/store';

import { setUser } from '../../store/userSlice';

import { data, useNavigate } from 'react-router-dom';

interface FormData {
  userName: string;
  password: string;
}

const Login: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();

  const navigate = useNavigate();
 const isAuthenticated = useSelector(
    (state: RootState) => state.user.isAuthenticated
  );
  const role = useSelector(
    (state: RootState) => state.user.role
  );
  // נווט אוטומטית אם המשתמש כבר מחובר
  useEffect(() => {
    console.log(isAuthenticated);
    
    if (isAuthenticated) {
      if (role === 'Manager') {
         navigate('/manager');
      } else {
         navigate('/employee');
      }
    }
    
  }, [isAuthenticated, role, navigate]);
  

  const [formData, setFormData] = useState<FormData>({
   userName:'',
   password:''
  });

  const [errors, setErrors] =
    useState<Partial<FormData>>({});

  const validate = () => {
  const newErrors: Partial<FormData> = {};

  if (formData.userName.trim().length < 3) {
    newErrors.userName = 'שם משתמש קצר מדי';
  }

  if (formData.password.trim().length < 4) {
    newErrors.password = 'סיסמה חייבת להיות לפחות 4 תווים';
  }

  setErrors(newErrors);
  return Object.keys(newErrors).length === 0;
};
  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement>
  ) => {

    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault();

  if (!validate()) return;

  try {
    const response = await fetch('http://localhost:5063/api/auth/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        userName: formData.userName,
        password: formData.password
      })
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || 'Login failed');
    }

    const data = await response.json();

    dispatch(setUser({
      firstName: '',
      lastName: '',
      identity: '',
      birthDate: '',
      email: '',
      isAuthenticated: true,
      shifts: [],
      role: data.role,
      token: data.token
    }));

    if (data.role === 'Manager') {
      navigate('/manager');
    } else {
      navigate('/employee/setup');
    }

  } catch (error) {
    console.error(error);
    alert('Login failed');
  }
};

  return (
    <div className="login-container">

      <form
        className="login-form"
        onSubmit={handleSubmit}
      >

        <h1>התחברות למערכת</h1>

       <input
  type="text"
  name="userName"
  placeholder="שם משתמש"
  onChange={handleChange}
/>

<input
  type="password"
  name="password"
  placeholder="סיסמה"
  onChange={handleChange}
/>

        

        <button type="submit">
          כניסה
        </button>

      </form>

    </div>
  );
};

export default Login;