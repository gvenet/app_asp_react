import React, { useEffect, useState } from "react";
import TextField from "@mui/material/TextField";
import Container from "@mui/material/Container";
import Grid from "@mui/material/Grid";
import "./styles.css";
import { AwesomeButton } from "react-awesome-button";
import { Link, redirect } from "react-router-dom";
import Select from "react-select";
import { getCategories } from "../../services/CategoryService";
import { getBrands } from "../../services/BrandService";
import { Brand } from "../../models/BrandModel";
import { Category } from "../../models/CategoryModel";
import makeAnimated from "react-select/animated";
import { postProduct } from "../../services/ProductService";

function AddProduct() {
  const [formData, setFormData] = useState({
    label: "",
    price: "",
    description: "",
    image_Url: "",
    version: "",
    categories: [],
    brandLabel: "",
  });

  const [brands, setBrands] = useState<Brand[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);

  const animatedComponents = makeAnimated();

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

  const handleChange = (event: any) => {
    const { name, value } = event.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = async (event: any) => {
    event.preventDefault();

    try {
      await postProduct(formData);
      console.log("Produit créé avec succès!");
      return redirect("/products");
    } catch (error) {
      console.error("Erreur lors de la requête POST :", error);
    }
  };

  const handleBrand = (selectedBrand: any) => {
    const selectedValue = (selectedBrand as { value: string; label: string })
      ?.value;
    setFormData({
      ...formData,
      brandLabel: selectedValue,
    });
  };

  const handleCategory = (selectedCategories: any) => {
    const selectedValues = selectedCategories.map(
      (cat: { value: string; label: string }) => cat.value
    );
    setFormData({
      ...formData,
      categories: selectedValues,
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

  return (
    <Container className="new-product-container">
      <form onSubmit={handleSubmit} className="new-product-form">
        <Grid container spacing={2}>
          <Grid item xs={12}>
            <TextField
              label="Label"
              variant="outlined"
              fullWidth
              name="label"
              value={formData.label}
              onChange={handleChange}
              required
            />
          </Grid>
          <Grid item xs={6}>
            <TextField
              label="Price"
              variant="outlined"
              fullWidth
              name="price"
              type="number"
              value={formData.price}
              onChange={handleChange}
              required
            />
          </Grid>
          <Grid item xs={6}>
            <TextField
              label="Version"
              variant="outlined"
              fullWidth
              name="version"
              value={formData.version}
              onChange={handleChange}
              required
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              label="Description"
              variant="outlined"
              fullWidth
              multiline
              rows={4}
              name="description"
              value={formData.description}
              onChange={handleChange}
              required
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              label="Image URL"
              variant="outlined"
              fullWidth
              name="image_Url"
              value={formData.image_Url}
              onChange={handleChange}
              required
            />
          </Grid>
          <Grid item xs={6}>
            <div className="select-category">
              <Select
                closeMenuOnSelect={false}
                components={animatedComponents}
                defaultValue={[categoryOptions[0]]}
                isMulti
                options={categoryOptions}
                onChange={handleCategory}
              />
            </div>
          </Grid>
          <Grid item xs={6}>
            <div className="select-brand">
              <Select
                closeMenuOnSelect={false}
                components={animatedComponents}
                defaultValue={[brandOptions[0]]}
                options={brandOptions}
                onChange={handleBrand}
              />
            </div>
          </Grid>
        </Grid>
        <AwesomeButton size="large" className="submit-form-product">
          Submit
        </AwesomeButton>
      </form>
      <Link to="/products">
        <AwesomeButton
          size="large"
          type="danger"
          className="cancel-form-product"
        >
          Cancel
        </AwesomeButton>
      </Link>
    </Container>
  );
}

export default AddProduct;
