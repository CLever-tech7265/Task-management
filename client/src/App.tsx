import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from './store/store';
import Home from './components/Home/Home';
import Login from './components/Login/Login';
import Requirements from './components/Requirements/Requirements';
import Specialization from './components/Specializations/Specialization';
import EmployeeSetup from './components/EmployeeSetup/EmployeeSetup';
import ManagerDashboard from './components/ManagerDashboard/ManagerDashboard';
import EmployeeDashboard from './components/EmployeeDashboard/EmployeeDashboard';
import Shift  from './components/Shift/Shift';
import Users from './components/Users/Users';
import { Provider } from 'react-redux';
import { store } from './store/store';

const App: React.FC = () => {
  return (
    <Provider store={store}>
      <Router>
        <AppRoutes />
      </Router>
    </Provider>
  );
};

const AppRoutes: React.FC = () => {
  const user = useSelector((state: RootState) => state.user);

  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/login" element={<Login />} />

      {/* מנהל או עובד לפי הרשאה */}
      <Route
        path="/manager"
        element={user.role === 'Manager' ? <ManagerDashboard /> : <EmployeeDashboard />}
      />

      {/* עובד */}
      <Route path="/employee" element={<EmployeeDashboard />} />
      <Route path="/employee/setup" element={<EmployeeSetup />} />
      {/* קיים אצלך */}
            <Route path="/shifts" element={<Shift />} />

      <Route path="/requirements" element={<Requirements />} />
      <Route path="/employees" element={<Users />} />

      <Route path="/Specialization" element={<Specialization />} />
    </Routes>
  );
};

export default App;