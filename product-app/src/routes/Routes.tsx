// src/routes.js
import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Products from '../components/Products/Products';
import Home from '../components/Home/Home';
import AddProduct from '../components/AddProduct/AddProduct';
// import Test from '../components/Test/Test';
// import About from './components/About';
// import Contact from './components/Contact';

const AppRoutes = () => (
  <Routes>
    <Route path="/" element={<Home />} />
    <Route path="/products" element={<Products />} />
    <Route path="/add_product" element={<AddProduct />} />
  </Routes>
);

const RoutesConfig = () => (
    <AppRoutes />
);

export default RoutesConfig;
