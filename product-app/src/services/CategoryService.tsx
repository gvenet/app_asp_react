// src/services/CategoryService.ts
import axios, { AxiosResponse } from 'axios';

const BASE_URL = 'https://localhost:7097/api';

export const getCategories = async (): Promise<AxiosResponse<any>> => {
  try {
    const response = await axios.get(`${BASE_URL}/Category`, {});
    return response;
  } catch (error) {
    throw error;
  }
};
