// components/Home.tsx
import React from 'react';
import { Link } from 'react-router-dom';

const Home: React.FC = () => {
  return (
    <div>
      <h1>דף הבית</h1>
      <Link to="/login">לכניסה</Link>
    </div>
  );
};

export default Home;