import React, { useEffect, useState } from "react";
import axios from "axios";
import "./Tasks.css";

interface Shift {
  id: string;
  startHour: string;
  finishHour: string;
  day: string;
}

interface Specialization {
  id: string;
  name: string;
}

interface Task {
  id: string;
  name: string;
  description: string;
  shifts: Shift[];
  specializations: Specialization[];
}

const Task: React.FC = () => {
  const API = import.meta.env.VITE_API_URL;

  const token = localStorage.getItem("token");

  const headers = {
    Authorization: `Bearer ${token}`,
  };

  const [tasks, setTasks] = useState<Task[]>([]);
  const [shifts, setShifts] = useState<Shift[]>([]);
  const [specializations, setSpecializations] = useState<Specialization[]>([]);

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");

  const [selectedShifts, setSelectedShifts] = useState<string[]>([]);
  const [selectedSpecs, setSelectedSpecs] = useState<string[]>([]);

  const [editId, setEditId] = useState<string | null>(null);

  // ================= LOAD =================
  useEffect(() => {
    loadAll();
  }, []);

  const loadAll = async () => {
    try {
      const [t, s, sp] = await Promise.all([
        axios.get(`${API}/api/task`, { headers }),
        axios.get(`${API}/api/shift`, { headers }),
        axios.get(`${API}/api/specialization`, { headers }),
      ]);

      setTasks(t.data);
      setShifts(s.data);
      setSpecializations(sp.data);
    } catch (err) {
      console.log("LOAD ERROR", err);
    }
  };

  // ================= CREATE / UPDATE =================
  const saveTask = async () => {
    try {
      const payload = {
        name,
        description,
        shiftIds: selectedShifts,
        specializationIds: selectedSpecs,
      };

      if (editId) {
        await axios.put(`${API}/api/task/${editId}`, payload, { headers });
      } else {
        await axios.post(`${API}/api/task`, payload, { headers });
      }

      reset();
      loadAll();
    } catch (err) {
      console.log(err);
    }
  };

  // ================= DELETE =================
  const deleteTask = async (id: string) => {
    try {
      await axios.delete(`${API}/api/task/${id}`, { headers });
      setTasks(prev => prev.filter(t => t.id !== id));
    } catch (err) {
      console.log(err);
    }
  };

  // ================= EDIT =================
  const startEdit = (t: Task) => {
    setEditId(t.id);
    setName(t.name);
    setDescription(t.description);

    setSelectedShifts(t.shifts.map(s => s.id));
    setSelectedSpecs(t.specializations.map(s => s.id));
  };

  const reset = () => {
    setName("");
    setDescription("");
    setSelectedShifts([]);
    setSelectedSpecs([]);
    setEditId(null);
  };

  const toggle = (id: string, list: string[], setFn: any) => {
    setFn(list.includes(id) ? list.filter(x => x !== id) : [...list, id]);
  };

  // ================= UI =================
  return (
    <div className="task-container">

      <h1>ניהול משימות</h1>

      {/* FORM */}
      <div className="form">

        <input
          placeholder="שם משימה"
          value={name}
          onChange={e => setName(e.target.value)}
        />

        <input
          placeholder="תיאור"
          value={description}
          onChange={e => setDescription(e.target.value)}
        />

        {/* SHIFTS */}
        <div className="section">
          <h3>משמרות</h3>
          <div className="chip-container">
            {shifts.map(s => (
              <div
                key={s.id}
                className={`chip ${selectedShifts.includes(s.id) ? "selected" : ""}`}
                onClick={() => toggle(s.id, selectedShifts, setSelectedShifts)}
              >
                {s.day} | {s.startHour} - {s.finishHour}
              </div>
            ))}
          </div>
        </div>

        {/* SPECS */}
        <div className="section">
          <h3>התמחויות</h3>
          <div className="chip-container">
            {specializations.map(s => (
              <div
                key={s.id}
                className={`chip ${selectedSpecs.includes(s.id) ? "selected" : ""}`}
                onClick={() => toggle(s.id, selectedSpecs, setSelectedSpecs)}
              >
                {s.name}
              </div>
            ))}
          </div>
        </div>

        <button className="primary-btn" onClick={saveTask}>
          {editId ? "עדכון משימה" : "יצירת משימה"}
        </button>

      </div>

      {/* TABLE */}
      <table className="table">
        <thead>
          <tr>
            <th>שם</th>
            <th>תיאור</th>
            <th>משמרות</th>
            <th>התמחויות</th>
            <th>פעולות</th>
          </tr>
        </thead>

        <tbody>
          {tasks.map(t => (
            <tr key={t.id}>
              <td>{t.name}</td>
              <td>{t.description}</td>

              <td>
                {t.shifts?.map(s => (
                  <div key={s.id} className="mini">
                    {s.day} {s.startHour}-{s.finishHour}
                  </div>
                ))}
              </td>

              <td>
                {t.specializations?.map(s => (
                  <div key={s.id} className="mini">
                    {s.name}
                  </div>
                ))}
              </td>

              <td>
                <button className="edit-btn" onClick={() => startEdit(t)}>ערוך</button>
                <button className="delete-btn" onClick={() => deleteTask(t.id)}>מחק</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

    </div>
  );
};

export default Task;