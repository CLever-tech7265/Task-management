import React, { useEffect, useState } from "react";
import "./Users.css";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import type { RootState } from "../../store/store";

interface User {
  id: string;
  userName: string;
  role: string;
}

interface RegisterDto {
  userName: string;
  password: string;
}

interface UpdateUserDto {
  userName?: string;
  password?: string;
  role?: string;
}

const Users: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("");

  const [editId, setEditId] = useState<string | null>(null);

  const navigate = useNavigate();

  const API = import.meta.env.VITE_API_URL + "/api/auth";
const token = useSelector((state: RootState) => state.user.token);
 const authHeader = token
  ? { Authorization: `Bearer ${token}` }
  : undefined;

  const goHome = () => navigate("/manager");

  // ================= LOAD USERS =================
  const loadUsers = async () => {
    setLoading(true);
    setError(null);

    try {
      const res = await axios.get<User[]>(`${API}/users`, {
        headers: authHeader,
      });
      setUsers(res.data);
    } catch (err: any) {
      console.log(err.response?.status, err.response?.data);
      setError(err.response?.data || "Failed to load users");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadUsers();
  }, []);

  // ================= SUBMIT =================
  const handleSubmit = async () => {
    if (!userName.trim()) return;

    try {
      if (editId) {
        const dto: UpdateUserDto = {
          userName,
          password: password || undefined,
          role: role || undefined,
        };

        await axios.put(`${API}/users/${editId}`, dto, {
          headers: authHeader,
        });
      } else {
        const dto: RegisterDto = {
          userName,
          password,
        };

        await axios.post(`${API}/register`, dto, {
          headers: authHeader,
        });
      }

      setUserName("");
      setPassword("");
      setRole("");
      setEditId(null);

      loadUsers();
    } catch (err: any) {
      setError(err.response?.data || "Failed to submit user");
    }
  };

  // ================= EDIT =================
  const handleEdit = (user: User) => {
    setUserName(user.userName);
    setRole(user.role);
    setEditId(user.id);
  };

  // ================= DELETE =================
  const handleDelete = async (id: string) => {
    try {
      await axios.delete(`${API}/users/${id}`, {
        headers: authHeader,
      });

      loadUsers();
    } catch (err: any) {
      setError(err.response?.data || "Failed to delete user");
    }
  };

  // ================= UI =================
  return (
    <div className="users-container">
      <h1>ניהול משתמשים</h1>

      <div className="user-form">
        <input
          placeholder="שם משתמש"
          value={userName}
          onChange={(e) => setUserName(e.target.value)}
        />

        <input
          type="password"
          placeholder={editId ? "סיסמה (אופציונלי)" : "סיסמה"}
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        <input
          placeholder="תפקיד"
          value={role}
          onChange={(e) => setRole(e.target.value)}
        />

        <button className="primary-btn" onClick={handleSubmit}>
          {editId ? "עדכן" : "הוסף"}
        </button>
      </div>

      {loading && <div>טוען...</div>}
      {error && <div className="error">{error}</div>}

      <table className="users-table">
        <thead>
          <tr>
            <th>שם</th>
            <th>תפקיד</th>
            <th>פעולות</th>
          </tr>
        </thead>

        <tbody>
          {users.map((u) => (
            <tr key={u.id}>
              <td>{u.userName}</td>
              <td>{u.role}</td>
              <td>
                <button onClick={() => handleEdit(u)}>✏️</button>
                <button onClick={() => handleDelete(u.id)}>🗑️</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <button className="secondary-btn" onClick={goHome}>
        חזרה לדף הבית
      </button>
    </div>
  );
};

export default Users;