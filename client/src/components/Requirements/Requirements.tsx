// Requirements.tsx

import React, { useState } from 'react';
import './Requirements.css';

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
  const [availability, setAvailability] =
    useState<AvailabilityState>(
      days.reduce((acc, day) => {
        acc[day] = {
          enabled: false,
          ranges: [
            {
              start: '',
              end: ''
            }
          ]
        };

        return acc;
      }, {} as AvailabilityState)
    );

  const toggleDay = (day: string) => {
    setAvailability((prev) => ({
      ...prev,
      [day]: {
        ...prev[day],
        enabled: !prev[day].enabled
      }
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

    setAvailability((prev) => ({
      ...prev,
      [day]: {
        ...prev[day],
        ranges: updatedRanges
      }
    }));
  };

  const addRange = (day: string) => {
    setAvailability((prev) => ({
      ...prev,
      [day]: {
        ...prev[day],
        ranges: [
          ...prev[day].ranges,
          {
            start: '',
            end: ''
          }
        ]
      }
    }));
  };

  const handleSubmit = () => {
    console.log(availability);
  };

  return (
    <div className="requirements-container">
      <div className="requirements-card">

        <h1>
          זמינות לעבודה
        </h1>

        <table className="availability-table">
          <thead>
            <tr>
              <th>יום</th>
              <th>זמין</th>
              <th>טווח שעות</th>
            </tr>
          </thead>

          <tbody>
            {days.map((day) => (
              <tr key={day}>
                <td className="day-name">
                  {day}
                </td>

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

                      {availability[day].ranges.map(
                        (range, index) => (
                          <div
                            className="time-range"
                            key={index}
                          >
                            <input
                              type="time"
                              value={range.start}
                              onChange={(e) =>
                                handleTimeChange(
                                  day,
                                  index,
                                  'start',
                                  e.target.value
                                )
                              }
                            />

                            <span>
                              עד
                            </span>

                            <input
                              type="time"
                              value={range.end}
                              onChange={(e) =>
                                handleTimeChange(
                                  day,
                                  index,
                                  'end',
                                  e.target.value
                                )
                              }
                            />
                          </div>
                        )
                      )}

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

        <button
          className="submit-button"
          onClick={handleSubmit}
        >
          שמור זמינות
        </button>

      </div>
    </div>
  );
};

export default Requirements;
// גישה לנתונים
// import { useSelector } from 'react-redux';

// import type {
//   RootState
// } from '../../store/store';

// const user = useSelector(
//   (state: RootState) => state.user
// );