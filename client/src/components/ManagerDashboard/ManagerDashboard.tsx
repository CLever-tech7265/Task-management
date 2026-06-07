import React from 'react';
import { useNavigate } from 'react-router-dom';
import './ManagerDashboard.css';

const ManagerDashboard: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="manager-container">
      <div className="manager-card">

        <h1>מנהל מערכת</h1>

        <button onClick={() => navigate('/Specialization')}>
          ניהול התמחויות
        </button>

        <button onClick={() => navigate('/employees')}>
          ניהול עובדים
        </button>

        <button onClick={() => navigate('/shifts')}>
          ניהול משמרות
        </button>

        <button onClick={() => navigate('/tasks')}>
          ניהול משימות
        </button>

        <button onClick={() => navigate('/employee')}>
          כניסה כעובד
        </button>

      </div>
    </div>
  );
};

export default ManagerDashboard;