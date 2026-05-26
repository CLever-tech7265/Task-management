
import React from 'react';
import { Link } from 'react-router-dom';
import './Home.css';

const Home: React.FC = () => {
  return (
    <div className="home-container">
      <div className="home-card">
        <h1>מערכת ניהול משימות</h1>

        <p>
          ברוכים הבאים למערכת ניהול ושיבוץ המשימות
        </p>

        <Link to="/login" className="home-button">
          מעבר להתחברות
        </Link>
      </div>
    </div>
  );
};

export default Home;
