import { useDispatch, useSelector } from "react-redux";
import { setUser } from "../../store/userSlice";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import type { RootState } from "../../store/store";
import axios from "axios";
import "./EmployeeSetup.css";

const EmployeeSetup: React.FC = () => {
  const API = import.meta.env.VITE_API_URL;

  const dispatch = useDispatch();
  const navigate = useNavigate();

  const token = useSelector((state: RootState) => state.user.token);
  const isAuthenticated = useSelector((state: RootState) => state.user.isAuthenticated);
  const role = useSelector((state: RootState) => state.user.role);

  const [specializations, setSpecializations] = useState<any[]>([]);
  const [selectedSpecs, setSelectedSpecs] = useState<string[]>([]);

  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    peopleId: "",
    birthDate: "",
    email: "",
  });

  // ========================
  // טעינת התמחויות
  // ========================
  useEffect(() => {
    if (!token) return;

    axios
      .get(`${API}/api/specialization`, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => setSpecializations(res.data))
      .catch((err) => console.error(err));
  }, [token, API]);

  // ========================
  // ניווט מבוקר בלבד
  // ========================
//  useEffect(() => {
//   if (!isAuthenticated) return;

//   if (role === "Manager") {
//     navigate("/manager");
//     return;
//   }

//   if (role === "Employee") {
//     // ניווט רק אם המשתמש חדש
//     if (profileCompleted === true) {
//       navigate("/employee");
//     } 
//     // אחרת נשאר בעמוד setup
//   }
// }, [isAuthenticated, role, profileCompleted, navigate]);
 const profileCompleted = useSelector(
    (state: RootState) => state.user.profileCompleted
  );
useEffect(() => {
  if (!isAuthenticated) return;

  if (role === "Manager") {
    navigate("/manager");
    return;
  }

  if (role === "Employee" && profileCompleted) {
    navigate("/employee");
  }
}, [isAuthenticated, role, profileCompleted]);
  // ========================
  // שינוי שדות
  // ========================
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  // ========================
  // שליחה לשרת
  // ========================

 const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault();

  if (!token) return;
  if (selectedSpecs.length === 0) return;

  try {
    const formToSend = {
      ...form,
      specializationIds: selectedSpecs,
    };

    const response = await axios.post(
      `${API}/api/employees/complete-profile`,
      formToSend,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    const data = response.data;

    dispatch(
      setUser({
        isAuthenticated: true,
        role: "Employee",
        token,
        profileCompleted: data.profileCompleted,
        shifts: [],
      })
    );

    navigate("/employee");
  } catch (err) {
    console.error(err);
  }
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

        <select
          multiple
          value={selectedSpecs}
          onChange={(e) => {
            const options = Array.from(e.target.selectedOptions);
            setSelectedSpecs(options.map((o) => o.value));
          }}
        >
          {specializations.map((s) => (
            <option key={s.id} value={s.id}>
              {s.name}
            </option>
          ))}
        </select>

        <button type="submit">שמור והמשך</button>
      </form>
    </div>
  );
};

export default EmployeeSetup;