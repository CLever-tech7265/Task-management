import React, { useState, useEffect } from 'react';
import './Requirements.css';
import { useDispatch } from 'react-redux';
import { setShifts } from '../../store/userSlice';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { store } from '../../store/store';

interface TimeRange {
  start: string;
  end: string;
}

interface DayAvailability {
  enabled: boolean;
  ranges: TimeRange[];
}

type AvailabilityState = {
  [key: string]: DayAvailability;
};

const days = ['ראשון', 'שני', 'שלישי', 'רביעי', 'חמישי', 'שישי'];

const Requirements: React.FC = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const token = store.getState().user.token;

  const [availability, setAvailability] = useState<AvailabilityState>(
    days.reduce((acc, day) => {
      acc[day] = { enabled: false, ranges: [{ start: '', end: '' }] };
      return acc;
    }, {} as AvailabilityState)
  );

  const [shifts, setShiftsState] = useState<any[]>([]);
  const [editingId, setEditingId] = useState<string | null>(null);

  // --- Helpers for availability table ---
  const toggleDay = (day: string) => {
    setAvailability(prev => ({
      ...prev,
      [day]: { ...prev[day], enabled: !prev[day].enabled },
    }));
  };

  const handleTimeChange = (day: string, index: number, field: 'start' | 'end', value: string) => {
    const updatedRanges = [...availability[day].ranges];
    updatedRanges[index][field] = value;
    setAvailability(prev => ({ ...prev, [day]: { ...prev[day], ranges: updatedRanges } }));
  };

  const addRange = (day: string) => {
    setAvailability(prev => ({
      ...prev,
      [day]: { ...prev[day], ranges: [...prev[day].ranges, { start: '', end: '' }] },
    }));
  };

  const convertToShifts = () => {
    const shiftsArray: string[] = [];
    Object.entries(availability).forEach(([day, value]) => {
      if (!value.enabled) return;
      value.ranges.forEach(range => {
        if (range.start && range.end) {
          shiftsArray.push(
            range.start <= range.end
              ? `${range.start}-${range.end}-${day}`
              : `${range.end}-${range.start}-${day}`
          );
        }
      });
    });
    return shiftsArray;
  };

  // --- API Calls ---
  const API_BASE = `${import.meta.env.VITE_API_URL}/api/shift-preferences`;
console.log(API_BASE);
  const loadShifts = async () => {
    try {
      const res = await axios.get(`${API_BASE}/my-shifts`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setShiftsState(res.data);
    } catch (err) {
      console.error(err);
    }
  };

  const sendShiftToServer = async (shift: string) => {
    const [startHour, finishHour, day] = shift.split('-');
    try {
      await axios.post(
        `${API_BASE}/create-with-preference`,
        { startHour, finishHour, day },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      await loadShifts();
    } catch (err) {
      console.error(err);
    }
  };

  const updateShift = async (preferenceId: string, shift: any) => {
    try {
      await axios.put(
        `${API_BASE}/${preferenceId}`,
        { startHour: shift.startHour, finishHour: shift.finishHour, day: shift.day },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setEditingId(null);
      await loadShifts();
    } catch (err) {
      console.error(err);
    }
  };

  const deleteShift = async (preferenceId: string) => {
    try {
      await axios.delete(`${API_BASE}/${preferenceId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      await loadShifts();
    } catch (err) {
      console.error(err);
    }
  };

  // --- Lifecycle ---
  useEffect(() => {
    loadShifts();
  }, []);

  // --- Event Handlers ---
  const handleSubmit = async () => {
    const shiftsArray = convertToShifts();
    dispatch(setShifts(shiftsArray));
    for (const shift of shiftsArray) {
      await sendShiftToServer(shift);
    }
    console.log('All shifts saved to server.');
  };

  const handleGoHome = () => navigate('/');

  // --- Render ---
  return (
    <div className="requirements-container">
      <div className="requirements-card">
        <h1>זמינות לעבודה</h1>
        <table className="availability-table">
          <thead>
            <tr>
              <th>יום</th>
              <th>זמין</th>
              <th>טווח שעות</th>
            </tr>
          </thead>
          <tbody>
            {days.map(day => (
              <tr key={day}>
                <td className="day-name">{day}</td>
                <td>
                  <input type="checkbox" checked={availability[day].enabled} onChange={() => toggleDay(day)} />
                </td>
                <td>
                  {availability[day].enabled && (
                    <div className="ranges-container">
                      {availability[day].ranges.map((range, index) => (
                        <div className="time-range" key={index}>
                          <input
                            type="time"
                            value={range.start}
                            onChange={e => handleTimeChange(day, index, 'start', e.target.value)}
                          />
                          <span>עד</span>
                          <input
                            type="time"
                            value={range.end}
                            onChange={e => handleTimeChange(day, index, 'end', e.target.value)}
                          />
                        </div>
                      ))}
                      <button type="button" className="add-button" onClick={() => addRange(day)}>
                        + הוסף טווח שעות
                      </button>
                    </div>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        <button className="submit-button" onClick={handleSubmit}>
          שמור זמינות
        </button>

        {/* --- Shifts List --- */}
        <h2>המשמרות שלי</h2>
        {shifts.map(s => (
          <div key={s.preferenceId} className="shift-card">
            {editingId === s.preferenceId ? (
              <>
                <input
                  value={s.startHour}
                  onChange={e => setShiftsState(prev => prev.map(x => (x.preferenceId === s.preferenceId ? { ...x, startHour: e.target.value } : x)))}
                />
                <input
                  value={s.finishHour}
                  onChange={e => setShiftsState(prev => prev.map(x => (x.preferenceId === s.preferenceId ? { ...x, finishHour: e.target.value } : x)))}
                />
                <input
                  value={s.day}
                  onChange={e => setShiftsState(prev => prev.map(x => (x.preferenceId === s.preferenceId ? { ...x, day: e.target.value } : x)))}
                />
                <button onClick={() => updateShift(s.preferenceId, s)}>שמור</button>
              </>
            ) : (
              <>
                <span>{s.startHour} - {s.finishHour} ({s.day})</span>
                <button onClick={() => setEditingId(s.preferenceId)}>ערוך</button>
                <button onClick={() => deleteShift(s.preferenceId)}>מחק</button>
              </>
            )}
          </div>
        ))}

        <button className="submit-button" style={{ marginTop: '20px', backgroundColor: '#6b7280' }} onClick={handleGoHome}>
          חזרה לדף הבית
        </button>
      </div>
    </div>
  );
};

export default Requirements;