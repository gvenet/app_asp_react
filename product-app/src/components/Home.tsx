// src/components/Home.tsx
import React, { useEffect, useState } from "react";
import { login } from "../services/LoginService";
import { product } from "../services/ProductService";


function Home() {
  const [loginData, setResponseData] = useState<any>(null);
  const [productData, setProductData] = useState<any>(null);

  useEffect(() => {
    login()
      .then((login) => {
        setResponseData(login.data);
      })
      .catch((error) => {
        console.error("Erreur lors de la requête:", error);
      });
  }, []);

  useEffect(() => {
    product()
      .then((product) => {
        setProductData(product.data);
      })
      .catch((error) => {
        console.error("Erreur lors de la requête:", error);
      });
  }, []);

  return (
    <div>
      <h1>Mon Composant</h1>
      {/* Afficher les données de la réponse si elles existent */}
      {loginData && (
        <div>
          <h2>Données de la réponse :</h2>
          {/* <pre>{JSON.stringify(loginData, null, 2)}</pre> */}
          <pre>{JSON.stringify(productData, null, 2)}</pre>
        </div>
      )}
      {/* Contenu de votre composant */}
    </div>
  );
}

export default Home;
