import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from '../../store/store';
import './Home.css';

const Home: React.FC = () => {
  const navigate = useNavigate();

 
  const handleLogin = () => {
    
    navigate('/login');
  };

  return (
    <div className="home-container">
      <div className="home-card">
        <h1>מערכת ניהול משימות</h1>
        <p>ברוכים הבאים למערכת ניהול ושיבוץ המשימות</p>
        <button className="home-button" onClick={handleLogin}>
          מעבר להתחברות
        </button>
      </div>
    </div>
  );
};

export default Home;