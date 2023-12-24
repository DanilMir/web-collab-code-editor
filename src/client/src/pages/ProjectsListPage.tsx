import {useAuth} from "react-oidc-context";
import {CustomPaginationActionsTable} from "../components/PaginationProjectsTable";
import Box from '@mui/material/Box';
import {observer} from "mobx-react";
import {Navigate} from "react-router-dom";
import LoginWarning from "../components/LoginWarning";

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
                <Box px={30} pt={4}>
                    <div>
                        <CustomPaginationActionsTable/>
                    </div>
                </Box>
            </>
        )
    }
)
