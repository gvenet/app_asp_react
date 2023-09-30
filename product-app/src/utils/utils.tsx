// utils.tsx
import { SetStateAction } from 'react';

type FetchFunction = () => Promise<any>;

/**
 * Effectue une requête en utilisant une fonction de requête et met à jour les données avec une fonction de mise à jour.
 *
 * @param {FetchFunction} fetchFunction - La fonction de requête.
 * @param {SetDataFunction} setDataFunction - La fonction de mise à jour des données.
 * @returns {void}
 */
export async function fetchData(
  fetchFunction: FetchFunction,
  setDataFunction: SetStateAction<any>
) {
  try {
    const response = await fetchFunction();
    setDataFunction(response.data);
  } catch (error) {
    console.error("Erreur lors de la requête:", error);
  }
}
