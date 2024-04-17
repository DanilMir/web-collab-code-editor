import { useAuth } from "react-oidc-context";
import { CustomPaginationActionsTable } from "../components/projects/PaginationProjectsTable";
import Box from "@mui/material/Box";
import { observer } from "mobx-react";
import { Navigate } from "react-router-dom";
import LoginWarning from "../components/generic/LoginWarning";
import Container from "@mui/material/Container";

export const ProjectsListPage = observer(() => {
  const auth = useAuth();

  if (auth.isLoading) {
    return null;
  }

  if (!auth.isAuthenticated) {
    return <LoginWarning />;
  }

  return (
    <>
      <Box
        sx={{
          display: "flex",
          alignItems: "start",
          justifyContent: "center",
          flexDirection: "column",
          paddingTop: 5,
          paddingLeft: "40px",
          paddingRight: "40px",
        }}
      >
        <CustomPaginationActionsTable />
      </Box>
    </>
  );
});
