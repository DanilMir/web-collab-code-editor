import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Menu from "@mui/material/Menu";
import MenuIcon from "@mui/icons-material/Menu";
import Container from "@mui/material/Container";
import Avatar from "@mui/material/Avatar";
import Button from "@mui/material/Button";
import Tooltip from "@mui/material/Tooltip";
import MenuItem from "@mui/material/MenuItem";
import { Link, useLocation } from "react-router-dom";
import { useAuth } from "react-oidc-context";
import PlayArrowIcon from "@mui/icons-material/PlayArrow";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import DataObjectIcon from "@mui/icons-material/DataObject";
import { useRootStore } from "../../hooks/useRootStore";
import { RestartAlt, Stop } from "@mui/icons-material";

export default function NavBar() {
  const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(
    null,
  );
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(
    null,
  );

  const auth = useAuth();

  const { sandboxStore, projectStore } = useRootStore();
  const location = useLocation();
  const projectRegex = new RegExp(
    "^/projects/[0-9a-fA-F]{8}\\b-[0-9a-fA-F]{4}\\b-[0-9a-fA-F]{4}\\b-[0-9a-fA-F]{4}\\b-[0-9a-fA-F]{12}/?$",
  );

  const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  return (
    <AppBar
      position="static"
      sx={{ paddingRight: "20px", paddingLeft: "20px" }}
    >
      <Box>
        <Toolbar disableGutters>
          <DataObjectIcon sx={{ display: { xs: "none", md: "flex" }, mr: 1 }} />
          <Typography
            variant="h6"
            noWrap
            component={Link}
            to="/"
            sx={{
              mr: 2,
              display: { xs: "none", md: "flex" },
              fontFamily: "monospace",
              fontWeight: 700,
              // letterSpacing: '.3rem',
              color: "inherit",
              textDecoration: "none",
            }}
          >
            CollabCodeEditor
          </Typography>

          <Box sx={{ flexGrow: 1, display: { xs: "flex", md: "none" } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="inherit"
            >
              <MenuIcon />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: "bottom",
                horizontal: "left",
              }}
              keepMounted
              transformOrigin={{
                vertical: "top",
                horizontal: "left",
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{
                display: { xs: "block", md: "none" },
              }}
            >
              <MenuItem onClick={handleCloseNavMenu}>
                <Typography textAlign="center" component={Link} to="/projects">
                  Projects
                </Typography>
              </MenuItem>
            </Menu>
          </Box>
          <DataObjectIcon sx={{ display: { xs: "flex", md: "none" }, mr: 1 }} />

          <Typography
            variant="h5"
            noWrap
            component={Link}
            to="/"
            sx={{
              mr: 2,
              display: { xs: "flex", md: "none" },
              flexGrow: 1,
              fontFamily: "monospace",
              fontWeight: 700,
              letterSpacing: ".3rem",
              color: "inherit",
              textDecoration: "none",
            }}
          >
            CollabCodeEditor
          </Typography>
          <Box sx={{ flexGrow: 1, display: { xs: "none", md: "flex" } }}>
            <Link to="/projects" style={{ textDecoration: "none" }}>
              <Button
                onClick={handleCloseNavMenu}
                sx={{ my: 2, color: "white", display: "block" }}
              >
                Projects
              </Button>
            </Link>
            <Link to="/projects/create">
              <IconButton
                onClick={handleCloseNavMenu}
                sx={{
                  mt: "13px",
                }}
              >
                <AddCircleOutlineIcon sx={{ color: "white" }} />
              </IconButton>
            </Link>
          </Box>

          {projectRegex.test(location.pathname) ? (
            <Box
              sx={{
                flexGrow: 1,
                display: { xs: "none", md: "flex", gap: "15px" },
                paddingLeft: "20px",
              }}
            >
              <Button
                onClick={() => {
                  sandboxStore.runProject(
                    projectStore.currentId,
                    auth.user?.access_token!,
                  );
                  handleCloseUserMenu();
                }}
                variant="contained"
                color="success"
                sx={{ my: 2, color: "white" }}
                startIcon={<PlayArrowIcon />}
              >
                Run
              </Button>
              <Button
                onClick={() => {
                  sandboxStore.updateProject(
                    projectStore.currentId,
                    auth.user?.access_token!,
                  );
                  handleCloseUserMenu();
                }}
                variant="contained"
                color="warning"
                sx={{ my: 2, color: "white" }}
                startIcon={<RestartAlt />}
              >
                Rebuild
              </Button>
              <Button
                onClick={() => {
                  sandboxStore.deleteProject(
                    projectStore.currentId,
                    auth.user?.access_token!,
                  );
                  handleCloseUserMenu();
                }}
                variant="contained"
                color="error"
                sx={{ my: 2, color: "white" }}
                startIcon={<Stop />}
              >
                Stop
              </Button>
            </Box>
          ) : null}

          {auth.isAuthenticated ? (
            <Box sx={{ flexGrow: 0 }}>
              <Tooltip title="Open settings">
                <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                  <Avatar />
                </IconButton>
              </Tooltip>
              <Menu
                sx={{ mt: "45px" }}
                id="menu-appbar"
                anchorEl={anchorElUser}
                anchorOrigin={{
                  vertical: "top",
                  horizontal: "right",
                }}
                keepMounted
                transformOrigin={{
                  vertical: "top",
                  horizontal: "right",
                }}
                open={Boolean(anchorElUser)}
                onClose={handleCloseUserMenu}
              >
                <MenuItem
                  onClick={() => {
                    auth.signoutRedirect();
                    handleCloseUserMenu();
                  }}
                >
                  <Typography textAlign="center"> Logout </Typography>
                </MenuItem>
              </Menu>
            </Box>
          ) : (
            <Button color="inherit" onClick={() => void auth.signinRedirect()}>
              Login
            </Button>
          )}
        </Toolbar>
      </Box>
    </AppBar>
  );
}
