import React from 'react';
import './App.css';
import RoutesConfig from './routes/Routes';
import Sidebar from './components/Sidebar/Sidebar';
import Topmenu from './components/Menu/Topmenu';

function App() {
  return (
    <div className="App">
      <main className="main">
        <Sidebar />
        <div className="app">
          <Topmenu />
          <RoutesConfig />
        </div>
      </main>
    </div>
    );
}

export default App;
