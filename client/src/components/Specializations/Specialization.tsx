// SpecializationRedux.tsx
import React, { useEffect, useState } from 'react';
import './Specialization.css';
import { useSelector } from 'react-redux';
import type { RootState } from '../../store/store';
import axios from 'axios';

interface Specialization {
  id: string;
  name: string;
}

const API = 'http://localhost:5063/api/specialization';

const SpecializationRedux: React.FC = () => {
  const token = useSelector((state: RootState) => state.user.token);
  const [list, setList] = useState<Specialization[]>([]);
  const [name, setName] = useState('');
  const [editId, setEditId] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const axiosInstance = axios.create({
    baseURL: API,
    headers: {
      'Content-Type': 'application/json',
    }
  });

  // מוסיף אוטומטית Authorization לכל בקשה
  axiosInstance.interceptors.request.use((config) => {
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });

  const load = async () => {
    setLoading(true);
    setError(null);

    if (!token) {
      setError('Missing token');
      setLoading(false);
      return;
    }

    try {
      const res = await axiosInstance.get<Specialization[]>('');
      setList(res.data);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to load specializations');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
  }, [token]);

  const handleSubmit = async () => {
    if (!name.trim()) return;
    setError(null);

    try {
      if (editId) {
        await axiosInstance.put(`/${editId}`, { name });
      } else {
        await axiosInstance.post('', { name });
      }
      setName('');
      setEditId(null);
      load();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to submit specialization');
    }
  };

  const handleEdit = (item: Specialization) => {
    setName(item.name);
    setEditId(item.id);
  };

  const handleDelete = async (id: string) => {
    setError(null);
    try {
      await axiosInstance.delete(`/${id}`);
      load();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to delete specialization');
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

      {loading && <div>Loading...</div>}
      {error && <div className="error">{error}</div>}

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
                <button className="delete" onClick={() => handleDelete(item.id)}>
                  מחק
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default SpecializationRedux;