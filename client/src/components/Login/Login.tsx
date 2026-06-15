import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { setUser } from "../../store/userSlice";
import type { AppDispatch, RootState } from "../../store/store";
import "./Login.css";

interface FormData {
  userName: string;
  password: string;
}

const Login: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const token = useSelector((state: RootState) => state.user.token);
  const role = useSelector((state: RootState) => state.user.role);
  const profileCompleted = useSelector(
    (state: RootState) => state.user.profileCompleted
  );

  const [formData, setFormData] = useState<FormData>({
    userName: "",
    password: "",
  });

  const [errors, setErrors] = useState<Partial<FormData>>({});
  const [loading, setLoading] = useState(false);
  const [authError, setAuthError] = useState("");

  // ========================
  // אם כבר מחובר → ניתוב אוטומטי
  // ========================
  useEffect(() => {
    if (!token) return;

    if (role === "Manager") {
      navigate("/manager", { replace: true });
      return;
    }

    if (role === "Employee") {
      if (profileCompleted) {
        navigate("/employee", { replace: true });
      } else {
        navigate("/employee/setup", { replace: true });
      }
    }
  }, [token, role, profileCompleted, navigate]);

  // ========================
  // שינוי שדות
  // ========================
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  // ========================
  // ולידציה בסיסית
  // ========================
  const validate = () => {
    const newErrors: Partial<FormData> = {};

    if (!formData.userName || formData.userName.trim().length < 3) {
      newErrors.userName = "שם משתמש קצר מדי";
    }

    if (!formData.password || formData.password.trim().length < 4) {
      newErrors.password = "סיסמה חייבת להיות לפחות 4 תווים";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };
  const API = import.meta.env.VITE_API_URL;

  // ========================
  // login
  // ========================
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setAuthError("");

    if (!validate()) return;

    setLoading(true);

    try {
      const response = await fetch(
        `${API}/api/auth/login`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(formData),
        }
      );

      if (!response.ok) {
        setAuthError("שם משתמש או סיסמה שגויים / אין הרשאה למשתמש");
        return;
      }

      const data = await response.json();

      dispatch(
        setUser({
          isAuthenticated: true,
          role: data.role,
          token: data.token,
          profileCompleted: data.profileCompleted,
          shifts: [],
        })
      );

      // חשוב: אל תסמוך רק על useEffect — תנתב מיד
      if (data.role === "Manager") {
        navigate("/manager", { replace: true });
      } else if (data.role === "Employee") {
        navigate(
          data.profileCompleted
            ? "/employee"
            : "/employee/setup",
          { replace: true }
        );
      }
    } catch {
      setAuthError("שגיאה בשרת, נסה שוב מאוחר יותר");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <form className="login-form" onSubmit={handleSubmit}>
        <h1>התחברות למערכת</h1>

        <input
          name="userName"
          placeholder="שם משתמש"
          value={formData.userName}
          onChange={handleChange}
        />
        {errors.userName && <span>{errors.userName}</span>}

        <input
          type="password"
          name="password"
          placeholder="סיסמה"
          value={formData.password}
          onChange={handleChange}
        />
        {errors.password && <span>{errors.password}</span>}

        {authError && <div className="auth-error">{authError}</div>}

        <button disabled={loading}>
          {loading ? "טוען..." : "כניסה"}
        </button>
      </form>
    </div>
  );
};

export default Login;