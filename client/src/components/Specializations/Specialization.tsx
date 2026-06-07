// Specialization.tsx
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './Specialization.css';

interface Specialization {
  id: string;
  name: string;
}

const API = 'http://localhost:5063/api/specialization';

const Specialization: React.FC = () => {
  const [list, setList] = useState<Specialization[]>([]);
  const [name, setName] = useState('');
  const [editId, setEditId] = useState<string | null>(null);

  const load = async () => {
    try {
      const res = await axios.get(API);
      setList(res.data);
    } catch (err) {
      console.error('Error loading data:', err);
    }
  };

  useEffect(() => {
    load();
  }, []);

  const handleSubmit = async () => {
    if (!name.trim()) return;

    try {
      if (editId) {
        await axios.put(`${API}/${editId}`, { name });
      } else {
        await axios.post(API, { name });
      }
      setName('');
      setEditId(null);
      load();
    } catch (err) {
      console.error('Error submitting:', err);
    }
  };

  const handleEdit = (item: Specialization) => {
    setName(item.name);
    setEditId(item.id);
  };

  const handleDelete = async (id: string) => {
    try {
      await axios.delete(`${API}/${id}`);
      load();
    } catch (err) {
      console.error('Error deleting:', err);
    }
  };

  return (
    <div className="spec-container">
      <h1>התמחויות</h1>

      <div className="spec-form">
        <input
          type="text"
          placeholder="הקלד שם התמחות..."
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <button onClick={handleSubmit}>
          {editId ? 'עדכן' : 'הוסף'}
        </button>
      </div>

      <table className="spec-table">
        <thead>
          <tr>
            <th>שם</th>
            <th>פעולות</th>
          </tr>
        </thead>
        <tbody>
          {list.map((item) => (
            <tr key={item.id}>
              <td>{item.name}</td>
              <td>
                <button onClick={() => handleEdit(item)}>ערוך</button>
                <button className="delete" onClick={() => handleDelete(item.id)}>מחק</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default Specialization;