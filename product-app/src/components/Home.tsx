import React, { useEffect, useState } from "react";
import { getProduct } from "../services/ProductService";
import { fetchData } from "../utils/utils";
import { Product } from "../models/ProductModel";

function Home() {
  const [products, setProduct] = useState<Product[]>([]);

  useEffect(() => {
    fetchData(getProduct, setProduct);
  }, []);

  const renderTable = () => {
    if (products.length === 0) {
      return <p>Chargement en cours...</p>;
    }

    return (
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Label</th>
            <th>Prix</th>
            <th>Description</th>
            <th>Image URL</th>
            <th>Version</th>
            <th>Catégorie</th>
          </tr>
        </thead>
        <tbody>
          {products.map((product) => (
            <tr key={product.id}>
              <td>{product.id}</td>
              <td>{product.label || "-"}</td>
              <td>{product.price || "-"}</td>
              <td>{product.description || "-"}</td>
              <td>{product.image_Url || "-"}</td>
              <td>{product.version || "-"}</td>
              <td>{product.category || "-"}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  };

  return (
    <div>
      <div>{renderTable()}</div>
    </div>
  );
}

export default Home;
