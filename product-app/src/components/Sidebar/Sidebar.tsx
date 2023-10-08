import "./styles.css";
import { Link } from "react-router-dom";

import  { useState } from "react";
import {
  RiHome4Line,
  RiTeamLine,
  RiCalendar2Line,
  RiUserFollowLine,
  RiUserUnfollowLine,
  RiBeerLine,
} from "react-icons/ri";
import { FiChevronsLeft, FiChevronsRight } from "react-icons/fi/";
import { Sidebar, SubMenu, Menu, MenuItem } from "react-pro-sidebar";

function Sidebars() {
  const [collapsed, setCollapsed] = useState(false);

  const [toggled, setToggled] = useState(false);

  const handleCollapsedChange = () => {
    setCollapsed(!collapsed);
  };

  return (
    <div>
      <Sidebar
        className={`app ${toggled ? "toggled" : ""}`}
        style={{ height: "100vh" }}
        collapsed={collapsed}
        toggled={toggled}
      >
        <main>
          <Menu>
            {collapsed ? (
              <MenuItem
                icon={<FiChevronsRight />}
                onClick={handleCollapsedChange}
              ></MenuItem>
            ) : (
              <MenuItem
                suffix={<FiChevronsLeft />}
                onClick={handleCollapsedChange}
                icon={<RiBeerLine />}
              >
                La ChouffApp
              </MenuItem>
            )}
            <hr />
          </Menu>
          <Menu>
            <MenuItem icon={<RiHome4Line />} component={<Link to="/" />}>
              Dashboard
            </MenuItem>
            <SubMenu defaultOpen label={"Products"} icon={<RiTeamLine />}>
              <MenuItem
                icon={<RiUserFollowLine />}
                component={<Link to="/products" />}
              >
                Products
              </MenuItem>
              <MenuItem icon={<RiUserUnfollowLine />}>Categories</MenuItem>
              <MenuItem icon={<RiCalendar2Line />}>Brands</MenuItem>
            </SubMenu>
          </Menu>
        </main>
      </Sidebar>
    </div>
  );
}
export default Sidebars;
