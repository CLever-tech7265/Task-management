// App.tsx
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './components/Home/Home';
import Login from './components/Login/Login';
import Requirements from './components/Requirements/Requirements';

import { Provider } from 'react-redux';
import { store } from './store/store';

const App: React.FC = () => {
  return (
    <Provider store={store}>
      <Router>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
           <Route path="/requirements" element={<Requirements />}/>
        </Routes>
      </Router>
    </Provider>
  );
};

export default App;