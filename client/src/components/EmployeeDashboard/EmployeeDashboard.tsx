import React from 'react';
import Requirements from '../Requirements/Requirements';
import './EmployeeDashboard.css';

const EmployeeDashboard: React.FC = () => {
  return (
    <div className="employee-container">
      <h1>אזור עובד</h1>

      {/* כאן העובד מזין זמינות */}
      <Requirements />
    </div>
  );
};

export default EmployeeDashboard;