// src/services/ApiService.ts
import axios, { AxiosResponse } from "axios";

// Définissez l'URL de base de l'API
const BASE_URL = "https://localhost:7097/api";

// Fonction pour effectuer un appel HTTP GET vers /api/Login
export const getProduct = async (
  sortColumn?: string,
  sortOrder?: string,
  categories?: string,
  brand?: string,
  minPrice?: number,
  maxPrice?: number,
  page?: number,
  pageSize?: number
): Promise<AxiosResponse<any>> => {
  try {
    // console.log(sortColumn + " " + sortOrder);
    const response = await axios.get(`${BASE_URL}/Product`, {
      params: {
        sortColumn,
        sortOrder,
        categories,
        brand,
        minPrice,
        maxPrice,
        page,
        pageSize,
      },
    });
    return response;
  } catch (error) {
    throw error;
  }
};
