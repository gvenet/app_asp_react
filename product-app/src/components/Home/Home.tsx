/* eslint-disable react-hooks/exhaustive-deps */

import React, { useEffect, useState } from "react";
import { getProduct } from "../../services/ProductService";
import { getCategories } from "../../services/CategoryService";
import { Product } from "../../models/ProductModel";
import { Category } from "../../models/CategoryModel";
import MultiRangeSlider from "../MultiRangeSlider";
import "./Home.css";

function Home() {
  const [products, setProducts] = useState<Product[]>([]);
  const [category, setCategory] = useState("");
  const [minPrice, setMinPrice] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(0);
  const [categories, setCategories] = useState<Category[]>([]);
  const [maxPrice, setMaxPrice] = useState(0);
  const [max, setMax] = useState(0);

  const handleSearch = () => {
    getProduct(category, minPrice, maxPrice, currentPage, pageSize)
      .then((response) => {
        console.log("getProducts")
        setProducts(response.data);
        setMax(parseInt(response.headers["x-max-price"]));
        setTotalPages(parseInt(response.headers["x-total-pages"]));
        setCurrentPage(parseInt(response.headers["x-current-page"]));
      })
      .catch((e) => console.error(e));
  };

  useEffect(() => {
    getCategories()
      .then((response) => {
        setCategories(response.data);
        console.log("getCategories");
      })
      .catch((e) => console.error(e));
  }, []);

  useEffect(() => {
    handleSearch();
  }, [currentPage, pageSize, category]);

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
            <th>Categories</th>
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
              <td>{product.categories || "-"}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  };

  return (
    <div className="container">
      <div>
        <div>
          <label className="label" htmlFor="category">
            Catégorie:
          </label>
          <select
            className="input"
            id="category"
            value={category}
            onChange={(e) => setCategory(e.target.value)}
          >
            <option value="">Toutes les catégories</option>
            {categories.map((cat) => (
              <option key={cat.id} value={cat.label || ""}>
                {cat.label || ""}
              </option>
            ))}
          </select>
        </div>
        <div>
          <MultiRangeSlider
            min={0}
            max={max}
            onChange={({ min, max }: { min: number; max: number }) => {
              setMinPrice(min);
              setMaxPrice(max);
            }}
          />
        </div>
        <div className="button-container">
          <label className="label">Afficher:</label>
          <div>
            <input
              type="radio"
              id="pageSize5"
              name="pageSize"
              value={5}
              checked={pageSize === 5}
              onChange={() => setPageSize(5)}
            />
            <label htmlFor="pageSize5">5</label>
          </div>
          <div>
            <input
              type="radio"
              id="pageSize10"
              name="pageSize"
              value={10}
              checked={pageSize === 10}
              onChange={() => setPageSize(10)}
            />
            <label htmlFor="pageSize10">10</label>
          </div>
          <div>
            <input
              type="radio"
              id="pageSize20"
              name="pageSize"
              value={20}
              checked={pageSize === 20}
              onChange={() => setPageSize(20)}
            />
            <label htmlFor="pageSize20">20</label>
          </div>
        </div>

        <div className="button-container">
          <button
            className="button"
            onClick={() => setCurrentPage(currentPage - 1)}
            disabled={currentPage === 1}
          >
            Précédent
          </button>
          <span>
            Page {currentPage} sur {totalPages}
          </span>
          <button
            className="button"
            onClick={() => setCurrentPage(currentPage + 1)}
            disabled={currentPage === totalPages}
          >
            Suivant
          </button>
        </div>

        <div className="button-container">
          <button className="button" onClick={handleSearch}>
            Rechercher
          </button>
        </div>
      </div>
      <div className="table-container">{renderTable()}</div>
    </div>
  );
}

export default Home;
