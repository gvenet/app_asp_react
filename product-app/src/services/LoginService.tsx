// src/services/LoginService.ts
import axios, { AxiosResponse } from 'axios';

// Définissez l'URL de base de l'API
const BASE_URL = 'https://localhost:7097/api';

// Fonction pour effectuer un appel HTTP GET vers /api/Login
export const login = async (): Promise<AxiosResponse<any>> => {
  try {
    const response = await axios.get(`${BASE_URL}/Login`);
    return response;
  } catch (error) {
    throw error;
  }
};
