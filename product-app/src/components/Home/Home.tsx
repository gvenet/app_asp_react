/* eslint-disable react-hooks/exhaustive-deps */

import "./styles.css";
import "react-awesome-button/dist/styles.css";
import { RiBeerLine } from "react-icons/ri";

function Home() {
  return (
    <div className="home-container">
      <div className="home-wrapper">
        <RiBeerLine /> Bienvenue dans la ChouffApp {" "}
        <RiBeerLine />
      </div>
    </div>
  );
}

export default Home;
