// src/routes.js
import React from 'react';
import { Route, BrowserRouter, Routes } from 'react-router-dom';
import Home from '../components/Home';
// import About from './components/About';
// import Contact from './components/Contact';

const AppRoutes = () => (
  <Routes>
    <Route path="/" element={<Home />} />
    {/* <Route path="/about" element={<About />} /> */}
    {/* <Route path="/contact" element={<Contact />} /> */}
  </Routes>
);

const RoutesConfig = () => (
  <BrowserRouter>
    <AppRoutes />
  </BrowserRouter>
);

export default RoutesConfig;
