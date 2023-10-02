// src/services/ApiService.ts
import axios, { AxiosResponse } from 'axios';

// Définissez l'URL de base de l'API
const BASE_URL = 'https://localhost:7097/api';

// Fonction pour effectuer un appel HTTP GET vers /api/Login
export const getProduct = async (
  category?: string,
  minPrice?: number,
  maxPrice?: number,
  page?: number,
  pageSize?: number
): Promise<AxiosResponse<any>> => {
  try {
    const response = await axios.get(`${BASE_URL}/Product`, {
      params: {
        category,
        minPrice,
        maxPrice,
        page,
        pageSize
      },
    });
    return response;
  } catch (error) {
    throw error;
  }
};
