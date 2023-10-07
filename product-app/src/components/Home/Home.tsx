/* eslint-disable react-hooks/exhaustive-deps */

import React, { useEffect, useState } from "react";
import { getProduct } from "../../services/ProductService";
import { getCategories } from "../../services/CategoryService";
import { Product } from "../../models/ProductModel";
import { Category } from "../../models/CategoryModel";
import MultiRangeSlider from "../MultiRangeSlider";
import "./Home.css";
import "react-awesome-button/dist/styles.css";
import { Brand } from "../../models/BrandModel";
import { getBrands } from "../../services/BrandService";
import makeAnimated from "react-select/animated";
import Select from "react-select";
import { Radio, RadioGroup } from "react-radio-group";
import { AwesomeButton } from "react-awesome-button";
import { FaChevronUp, FaChevronDown, FaChevronLeft, FaChevronRight, FaSearch } from "react-icons/fa";

function Home() {
  const [products, setProducts] = useState<Product[]>([]);
  const [brands, setBrands] = useState<Brand[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [categoriesArr, setCategoriesArr] = useState([]);
  const [brand, setBrand] = useState("");
  const [minPrice, setMinPrice] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(0);
  const [maxPrice, setMaxPrice] = useState(0);
  const [max, setMax] = useState(0);
  const okMsg = "Chargement en cours...";
  const koMsg = "Aucun produit trouvé...";
  const [msg, setMsg] = useState(okMsg);

  const [sortColumn, setSortColumn] = useState("id");
  const [sortOrder, setSortOrder] = useState<"desc" | "asc">("desc");

  const categoryOptions = [
    ...categories.map((category) => ({
      value: category.label || "",
      label: category.label || "",
    })),
  ];

  const brandOptions = [
    { value: "", label: "Toutes les marques" },
    ...brands.map((brand) => ({
      value: brand.label || "",
      label: brand.label || "",
    })),
  ];

  const animatedComponents = makeAnimated();

  const handleBrand = (selectedBrand: any) => {
    const selectedValue = (selectedBrand as { value: string; label: string })
      ?.value;
    setBrand(selectedValue);
  };

  const handleCategory = (selectedCategories: any) => {
    const selectedValues = selectedCategories.map(
      (cat: { value: string; label: string }) => cat.value
    );
    console.log(selectedValues);
    setCategoriesArr(selectedValues);
  };

  // Fonction pour gérer le tri lorsque vous cliquez sur un en-tête de colonne
  const handleSort = (column: string) => {
    if (sortColumn === column) {
      if (sortOrder === "asc") {
        setSortOrder("desc");
      } else {
        setSortOrder(column === "id" ? "asc" : "desc");
        setSortColumn("id");
      }
    } else {
      setSortColumn(column);
      setSortOrder("asc");
    }
  };

  const handleSearch = () => {
    setMsg(okMsg);
    getProduct(
      sortColumn,
      sortOrder,
      categoriesArr.join(","),
      brand,
      minPrice,
      maxPrice,
      currentPage,
      pageSize
    )
      .then((response) => {
        setProducts(response.data);

        const t_max = parseInt(response.headers["x-max-price"]);
        const t_totalPages = parseInt(response.headers["x-total-pages"]);
        const t_currentPage = parseInt(response.headers["x-current-page"]);

        setMax(t_max + 1);
        setTotalPages(t_totalPages);
        setCurrentPage(t_currentPage);
      })
      .catch((e) => {
        if (e.response.status === 404) {
          setProducts([]);
          setMsg(koMsg);
        }
        console.error(e);
      });
  };

  useEffect(() => {
    getCategories()
      .then((response) => {
        setCategories(response.data);
      })
      .catch((e) => console.error(e));
    getBrands()
      .then((response) => {
        setBrands(response.data);
      })
      .catch((e) => console.error(e));
  }, []);

  useEffect(() => {
    handleSearch();
  }, [sortColumn, sortOrder, currentPage, pageSize, categoriesArr, brand]);

  const renderTable = () => {
    if (products.length === 0) {
      return <p>{msg}</p>;
    }

    return (
      <table>
        <thead>
          <tr>
            {[
              "id",
              "label",
              "price",
              "description",
              "image_Url",
              "version",
              "category",
              "brand.label",
            ].map((column) => (
              <th key={column} onClick={() => handleSort(column)}>
                {column.charAt(0).toUpperCase() + column.slice(1)}
                {" "}
                {sortColumn === column && sortOrder === "asc" && <FaChevronUp />}
                {sortColumn === column && sortOrder === "desc" && <FaChevronDown />}
              </th>
            ))}
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
              <td>{product.brand?.label || "-"}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  };

  return (
    <div className="container">
      <div className="header">
        <div className="select-component">
          <Select
            closeMenuOnSelect={false}
            components={animatedComponents}
            defaultValue={[categoryOptions[0]]}
            isMulti
            options={categoryOptions}
            onChange={handleCategory}
          />
        </div>
        <div className="select-component">
          <Select
            closeMenuOnSelect={false}
            components={animatedComponents}
            defaultValue={[brandOptions[0]]}
            options={brandOptions}
            onChange={handleBrand}
          />
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
        <div>
          <RadioGroup
            name="pageSize"
            onChange={(val: number) => setPageSize(val)}
            selectedValue={pageSize}
            className="radio-group"
          >
            <div className="radio">
              <Radio value={5} />5
            </div>
            <div className="radio">
              <Radio value={10} />
              10
            </div>
            <div className="radio">
              <Radio value={20} />
              20
            </div>
          </RadioGroup>
        </div>
        <div>
          <AwesomeButton
            type="primary"
            onPress={() => setCurrentPage(currentPage - 1)}
            disabled={currentPage === 1}
          >
            <FaChevronLeft/>
          </AwesomeButton>
        </div>
        <div>
          Page {currentPage} sur {totalPages}
        </div>
        <div>
          <AwesomeButton
            type="primary"
            onPress={() => setCurrentPage(currentPage + 1)}
            disabled={currentPage === totalPages}
          >
            <FaChevronRight/>
          </AwesomeButton>
        </div>
        <div>
          <AwesomeButton type="secondary" onPress={handleSearch}>
          <FaSearch/>
          </AwesomeButton>
        </div>
      </div>
      <div className="table-container">{renderTable()}</div>
    </div>
  );
}

export default Home;
