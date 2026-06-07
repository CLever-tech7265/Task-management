import { useDispatch, useSelector } from "react-redux";
import { setUser } from "../../store/userSlice";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import type { RootState } from "../../store/store";
import axios from "axios";
import './EmployeeSetup.css'
const EmployeeSetup: React.FC = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  // הקריאה ל-useSelector חייבת להיות כאן, לא בתוך handleSubmit
  const token = useSelector((state: RootState) => state.user.token);

  const [form, setForm] = useState({
    firstName: '',
    lastName: '',
    peopleId: '',
    birthDate: '',
    email: ''
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault();

  console.log('Submitting form with values:', form);
  console.log('Token from Redux:', token);

  if (!token) {
    console.error('No token found in Redux state!');
    return;
  }

  try {
    const res = await axios.post(
      'http://localhost:5063/api/employees/complete-profile',
      form,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      }
    );

    console.log('Server response:', res.data);

    dispatch(setUser({
      firstName: form.firstName,
      lastName: form.lastName,
      identity: form.peopleId,
      birthDate: form.birthDate,
      email: form.email,
      isAuthenticated: true,
      role: 'Employee',
      token,
      shifts: []
    }));

    navigate('/employee');
  } catch (error: any) {
    console.error('Error status:', error.response?.status);
console.log(JSON.stringify(error.response?.data, null, 2));  }
};

  return (
    <div className="setup-container">
      <form className="setup-card" onSubmit={handleSubmit}>
        <h1>השלמת פרופיל עובד</h1>

        <input name="firstName" placeholder="שם פרטי" onChange={handleChange} />
        <input name="lastName" placeholder="שם משפחה" onChange={handleChange} />
        <input name="peopleId" placeholder="תעודת זהות" onChange={handleChange} />
        <input type="date" name="birthDate" onChange={handleChange} />
        <input name="email" placeholder="אימייל" onChange={handleChange} />

        <button type="submit">שמור והמשך</button>
      </form>
    </div>
  );
};
export default EmployeeSetup