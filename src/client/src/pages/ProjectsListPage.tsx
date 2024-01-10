import {useAuth} from "react-oidc-context";
import {CustomPaginationActionsTable} from "../components/projects/PaginationProjectsTable";
import Box from '@mui/material/Box';
import {observer} from "mobx-react";
import {Navigate} from "react-router-dom";
import LoginWarning from "../components/generic/LoginWarning";

export const ProjectsListPage = observer(() => {
        const auth = useAuth();

        if(auth.isLoading) {
            return null;
        }

        if (!auth.isAuthenticated) {
            return <LoginWarning/>;
        }

        return (
            <>
                <Box px={60} pt={4}>
                    <div>
                        <CustomPaginationActionsTable/>
                    </div>
                </Box>
            </>
        )
    }
)
