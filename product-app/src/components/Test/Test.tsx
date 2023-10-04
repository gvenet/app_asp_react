import React, { useEffect, useState } from "react";
import MultiRangeSlider from "../MultiRangeSlider";
import { getProduct } from "../../services/ProductService";

function App() {
  const [max, setMax] = useState(0);

  useEffect(() => {
    getProduct("", "", 0, 0, 1, 10)
      .then((response) => {
        console.log("getProduct");
        setMax(parseInt(response.headers["x-max-price"]));
      })
      .catch((e) => console.error(e));
  }, []);

  useEffect(() => {
    console.log("max : " + max);
  }, [max]);

  return (
    <div>
      <MultiRangeSlider min={0} max={1000} onChange={({ min, max }) => ""} />
      <MultiRangeSlider min={0} max={max} onChange={({ min, max }) => ""} />
    </div>
  );
}

export default App;
