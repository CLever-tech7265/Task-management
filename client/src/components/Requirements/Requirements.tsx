import React, { useState } from 'react';
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

const days = [
  'ראשון',
  'שני',
  'שלישי',
  'רביעי',
  'חמישי',
  'שישי'
];

const Requirements: React.FC = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate(); // <-- ניווט

  const [availability, setAvailability] = useState<AvailabilityState>(
    days.reduce((acc, day) => {
      acc[day] = {
        enabled: false,
        ranges: [{ start: '', end: '' }]
      };
      return acc;
    }, {} as AvailabilityState)
  );

  const toggleDay = (day: string) => {
    setAvailability(prev => ({
      ...prev,
      [day]: { ...prev[day], enabled: !prev[day].enabled }
    }));
  };

  const handleTimeChange = (
    day: string,
    index: number,
    field: 'start' | 'end',
    value: string
  ) => {
    const updatedRanges = [...availability[day].ranges];
    updatedRanges[index][field] = value;
    setAvailability(prev => ({
      ...prev,
      [day]: { ...prev[day], ranges: updatedRanges }
    }));
  };

  const addRange = (day: string) => {
    setAvailability(prev => ({
      ...prev,
      [day]: {
        ...prev[day],
        ranges: [...prev[day].ranges, { start: '', end: '' }]
      }
    }));
  };

  const convertToShifts = () => {
    const shifts: string[] = [];
    Object.entries(availability).forEach(([day, value]) => {
      if (!value.enabled) return;
      value.ranges.forEach(range => {
        if (range.start && range.end) {
          shifts.push(
            range.start <= range.end
              ? `${range.start}-${range.end}-${day}`
              : `${range.end}-${range.start}-${day}`
          );
        }
      });
    });
    return shifts;
  };

  const SHIFT_API = 'http://localhost:5063/api/employees/create-with-preference';

 const token = store.getState().user.token;

const sendShiftToServer = async (shift: string) => {
  const [startHour, finishHour, day] = shift.split('-');

 const res = await axios.post(
  SHIFT_API,
  { StartHour: startHour, FinishHour: finishHour, Day: day },
  {
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json"
    }
  }
);

  return res.data;
};

  const handleSubmit = async () => {
    const shifts = convertToShifts();
    dispatch(setShifts(shifts));
    for (const shift of shifts) {
      await sendShiftToServer(shift);
    }
    console.log('all shifts saved to SQL');
  };

  const handleGoHome = () => {
    navigate('/'); // חזרה ל-HOME
  };

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
                  <input
                    type="checkbox"
                    checked={availability[day].enabled}
                    onChange={() => toggleDay(day)}
                  />
                </td>
                <td>
                  {availability[day].enabled && (
                    <div className="ranges-container">
                      {availability[day].ranges.map((range, index) => (
                        <div className="time-range" key={index}>
                          <input
                            type="time"
                            value={range.start}
                            onChange={e =>
                              handleTimeChange(day, index, 'start', e.target.value)
                            }
                          />
                          <span>עד</span>
                          <input
                            type="time"
                            value={range.end}
                            onChange={e =>
                              handleTimeChange(day, index, 'end', e.target.value)
                            }
                          />
                        </div>
                      ))}
                      <button
                        type="button"
                        className="add-button"
                        onClick={() => addRange(day)}
                      >
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

        {/* כפתור חזרה ל-Home */}
        <button
          className="submit-button"
          style={{ marginTop: '20px', backgroundColor: '#6b7280' }}
          onClick={handleGoHome}
        >
          חזרה לדף הבית
        </button>
      </div>
    </div>
  );
};

export default Requirements;