import React, { useState } from 'react';
import './Login.css';

import { useDispatch } from 'react-redux';
import type { AppDispatch } from '../../store/store';
import { setUser } from '../../store/store';

import { useNavigate } from 'react-router-dom';

interface FormData {
  firstName: string;
  lastName;
  identity: string;
  birthDate: string;
  email: string;
}

const Login: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const [formData, setFormData] = useState<FormData>({
    firstName: '',
    lastName: '',
    identity: '',
    birthDate: '',
    email: ''
  });

  const [errors, setErrors] = useState<Partial<FormData>>({});

  const validate = () => {
    const newErrors: Partial<FormData> = {};

    if (formData.firstName.trim().length < 2)
      newErrors.firstName = 'שם פרטי חייב להכיל לפחות 2 תווים';

    if (formData.lastName.trim().length < 2)
      newErrors.lastName = 'שם משפחה חייב להכיל לפחות 2 תווים';

    if (!/^\d{9}$/.test(formData.identity))
      newErrors.identity = 'תעודת זהות חייבת להכיל 9 ספרות';

    if (!formData.birthDate)
      newErrors.birthDate = 'יש לבחור תאריך לידה';

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email))
      newErrors.email = 'אימייל לא תקין';

    setErrors(newErrors);

    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (validate()) {
      dispatch(
        setUser({
          ...formData,
          isAuthenticated: true
        })
      );

      navigate('/requirements'); // ✔️ מעבר עם Router
    }
  };

  return (
    <div className="login-container">
      <form className="login-form" onSubmit={handleSubmit}>
        <h1>התחברות למערכת</h1>

        <input name="firstName" onChange={handleChange} />
        <input name="lastName" onChange={handleChange} />
        <input name="identity" onChange={handleChange} />
        <input type="date" name="birthDate" onChange={handleChange} />
        <input type="email" name="email" onChange={handleChange} />

        <button type="submit">
          כניסה
        </button>
      </form>
    </div>
  );
};

export default Login;