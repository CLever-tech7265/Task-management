import React, { useEffect, useState } from "react";
import axios from "axios";
import "./Shift.css";
import { useNavigate } from "react-router-dom";
import { store } from "../../store/store";

interface ShiftType {
  id: string;
  startHour: string;
  finishHour: string;
  day: string;
}

const days = ["ראשון", "שני", "שלישי", "רביעי", "חמישי", "שישי"];

const Shift: React.FC = () => {
  const navigate = useNavigate();
  const token = store.getState().user.token;

  const [shifts, setShifts] = useState<ShiftType[]>([]);
  const [form, setForm] = useState({ startHour: "", finishHour: "", day: "" });
  const [editingId, setEditingId] = useState<string | null>(null);

  const API = `${import.meta.env.VITE_API_URL}/api/shift`;

  const loadShifts = async () => {
    try {
      const res = await axios.get(API, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setShifts(res.data);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    loadShifts();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const createShift = async () => {
    if(form.startHour>form.finishHour){
      var time=form.startHour;
      form.startHour=form.finishHour;
      form.finishHour=time;
    }
    if (!form.startHour || !form.finishHour || !form.day) return;
    try {
      await axios.post(API, form, { headers: { Authorization: `Bearer ${token}` } });
      setForm({ startHour: "", finishHour: "", day: "" });
      loadShifts();
    } catch (err) {
      console.error(err);
    }
  };

  const updateShift = async (id: string) => {
    if(form.startHour>form.finishHour){
      var time=form.startHour;
      form.startHour=form.finishHour;
      form.finishHour=time;
    }
    try {
      await axios.put(`${API}/${id}`, form, { headers: { Authorization: `Bearer ${token}` } });
      setEditingId(null);
      setForm({ startHour: "", finishHour: "", day: "" });
      loadShifts();
    } catch (err) {
      console.error(err);
    }
  };

  const deleteShift = async (id: string) => {
    try {
      await axios.delete(`${API}/${id}`, { headers: { Authorization: `Bearer ${token}` } });
      loadShifts();
    } catch (err) {
      console.error(err);
    }
  };

  const handleEdit = (shift: ShiftType) => {
    setEditingId(shift.id);
    setForm({ startHour: shift.startHour, finishHour: shift.finishHour, day: shift.day });
  };

  const handleGoBack = () => navigate("/manager");

  return (
    <div className="shift-container">
      <div className="shift-card">
        <h1>ניהול משמרות</h1>

        <button className="back-btn" onClick={handleGoBack}>
          ← חזרה לדף מנהל
        </button>

        <div className="shift-form">
          <input type="time" name="startHour" value={form.startHour} onChange={handleChange} />
          <input type="time" name="finishHour" value={form.finishHour} onChange={handleChange} />
          <select name="day" value={form.day} onChange={handleChange}>
            <option value="">בחר יום</option>
            {days.map((d) => (
              <option key={d} value={d}>
                {d}
              </option>
            ))}
          </select>
          <button className="primary-btn" onClick={editingId ? () => updateShift(editingId) : createShift}>
            {editingId ? "עדכן משמרת" : "הוסף משמרת"}
          </button>
        </div>

        <h2>כל המשמרות הקיימות</h2>
        <table className="shift-table">
          <thead>
            <tr>
              <th>יום</th>
              <th>שעת התחלה</th>
              <th>שעת סיום</th>
              <th>פעולות</th>
            </tr>
          </thead>
          <tbody>
            {shifts.map((s) => (
              <tr key={s.id}>
                <td>{s.day}</td>
                <td>{s.startHour}</td>
                <td>{s.finishHour}</td>
                <td>
                  <button onClick={() => handleEdit(s)}>ערוך</button>
                  <button className="delete" onClick={() => deleteShift(s.id)}>
                    מחק
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default Shift;