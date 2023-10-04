// src/services/BrandService.ts
import axios, { AxiosResponse } from 'axios';

const BASE_URL = 'https://localhost:7097/api';

export const getBrands = async (): Promise<AxiosResponse<any>> => {
  try {
    const response = await axios.get(`${BASE_URL}/Brand`, {});
    return response;
  } catch (error) {
    throw error;
  }
};
