import React, { useEffect, useState } from "react";
import { getProduct } from "../services/ProductService";
// import { fetchData } from "../utils/utils";
import { Product } from "../models/ProductModel";

function Home() {
  const [products, setProduct] = useState<Product[]>([]);
  const [category, setCategory] = useState("");
  const [minPrice, setMinPrice] = useState(0);
  const [maxPrice, setMaxPrice] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10;
  const [totalPages, setTotalPage] = useState(0)

  useEffect(() => {
    getProduct(category, minPrice, maxPrice, currentPage, pageSize)
    .then((response) => {
      setProduct(response.data);
      setTotalPage(parseInt(response.headers['x-total-pages']))
    })
    .catch((error) => {
      console.error("Erreur lors de la récupération des produits:", error);
    });
  }, [category, minPrice, maxPrice, currentPage]);

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
      <div>
        <div>
          <label htmlFor="category">Catégorie:</label>
          <input
            type="text"
            id="category"
            value={category}
            onChange={(e) => setCategory(e.target.value)}
          />
        </div>
        <div>
          <label htmlFor="minPrice">Prix minimum:</label>
          <input
            type="number"
            id="minPrice"
            value={minPrice}
            onChange={(e) => setMinPrice(parseFloat(e.target.value))}
          />
        </div>
        <div>
          <label htmlFor="maxPrice">Prix maximum:</label>
          <input
            type="number"
            id="maxPrice"
            value={maxPrice}
            onChange={(e) => setMaxPrice(parseFloat(e.target.value))}
          />
        </div>
        <button
          onClick={() => setCurrentPage(currentPage - 1)}
          disabled={currentPage === 1}
        >
          Précédent
        </button>
        <span>
          Page {currentPage} sur {totalPages}
        </span>
        <button
          onClick={() => setCurrentPage(currentPage + 1)}
          disabled={currentPage === totalPages}
        >
          Suivant
        </button>
      </div>
      <div>{renderTable()}</div>
    </div>
  );
}

export default Home;
