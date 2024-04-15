import * as React from 'react';
import {useTheme} from '@mui/material/styles';
import Box from '@mui/material/Box';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableFooter from '@mui/material/TableFooter';
import TablePagination from '@mui/material/TablePagination';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import IconButton from '@mui/material/IconButton';
import FirstPageIcon from '@mui/icons-material/FirstPage';
import KeyboardArrowLeft from '@mui/icons-material/KeyboardArrowLeft';
import KeyboardArrowRight from '@mui/icons-material/KeyboardArrowRight';
import LastPageIcon from '@mui/icons-material/LastPage';
import {useRootStore} from "../../hooks/useRootStore";
import {observer} from "mobx-react";
import {useAuth} from "react-oidc-context";
import Button from "@mui/material/Button";
import {Link, Navigate} from 'react-router-dom';
import {useEffect} from "react";
import ReplayIcon from '@mui/icons-material/Replay';
import {ButtonGroup, TableHead} from "@mui/material";
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import Typography from "@mui/material/Typography";
import {flow} from "mobx";
import Container from "@mui/material/Container";


interface TablePaginationActionsProps {
    count: number;
    page: number;
    rowsPerPage: number;
    onPageChange: (
        event: React.MouseEvent<HTMLButtonElement>,
        newPage: number,
    ) => void;
}

function TablePaginationActions(props: TablePaginationActionsProps) {
    const theme = useTheme();
    const {count, page, rowsPerPage, onPageChange} = props;

    const handleFirstPageButtonClick = (
        event: React.MouseEvent<HTMLButtonElement>,
    ) => {
        onPageChange(event, 0);
    };

    const handleBackButtonClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        onPageChange(event, page - 1);
    };

    const handleNextButtonClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        onPageChange(event, page + 1);
    };

    const handleLastPageButtonClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        onPageChange(event, Math.max(0, Math.ceil(count / rowsPerPage) - 1));
    };

    return (
        <Box sx={{flexShrink: 0, ml: 2.5}}>
            <IconButton
                onClick={handleFirstPageButtonClick}
                disabled={page === 0}
                aria-label="first page"
            >
                {theme.direction === 'rtl' ? <LastPageIcon/> : <FirstPageIcon/>}
            </IconButton>
            <IconButton
                onClick={handleBackButtonClick}
                disabled={page === 0}
                aria-label="previous page"
            >
                {theme.direction === 'rtl' ? <KeyboardArrowRight/> : <KeyboardArrowLeft/>}
            </IconButton>
            <IconButton
                onClick={handleNextButtonClick}
                disabled={page >= Math.ceil(count / rowsPerPage) - 1}
                aria-label="next page"
            >
                {theme.direction === 'rtl' ? <KeyboardArrowLeft/> : <KeyboardArrowRight/>}
            </IconButton>
            <IconButton
                onClick={handleLastPageButtonClick}
                disabled={page >= Math.ceil(count / rowsPerPage) - 1}
                aria-label="last page"
            >
                {theme.direction === 'rtl' ? <FirstPageIcon/> : <LastPageIcon/>}
            </IconButton>
        </Box>
    );
}

export const CustomPaginationActionsTable = observer(() => {

        const {projectsStore} = useRootStore();
        const auth = useAuth();

        const [page, setPage] = React.useState(0);
        const [rowsPerPage, setRowsPerPage] = React.useState(10);


        useEffect(() => {
            projectsStore.getActiveProjects(page, rowsPerPage, auth.user?.access_token!);
        }, []);

        const handleChangePage = (
            event: React.MouseEvent<HTMLButtonElement> | null,
            newPage: number,
        ) => {
            handleChanges(newPage, rowsPerPage);
            setPage(newPage);
        };

        const handleChangeRowsPerPage = (
            event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
        ) => {
            handleChanges(0, parseInt(event.target.value, 10));
            setRowsPerPage(parseInt(event.target.value, 10));
            setPage(0);
        };

        const handleChanges = (pages: number, limit: number) => {
            projectsStore.getActiveProjects(pages, limit, auth.user?.access_token!)
        }

        return (
            <>
                <ButtonGroup>
                    <Button
                        variant="contained"
                        startIcon={<ReplayIcon/>}
                        onClick={() => {
                            projectsStore.getActiveProjects(page, rowsPerPage, auth.user?.access_token!)
                        }}
                        sx={{minWidth: '2px'}}
                    >Update projects list</Button>

                    <Button sx={{marginLeft: "15px"}} variant="outlined" component={Link} to="/projects/create">Create
                        project</Button>
                </ButtonGroup>


                <TableContainer component={Paper} sx={{marginTop: "20px"}}>
                    <Table sx={{minWidth: 500}} aria-label="custom pagination table">
                        <TableHead>
                            <TableRow>
                                <TableCell align="left">Name</TableCell>
                                <TableCell align="left">Description</TableCell>
                                <TableCell align="right">Visibility</TableCell>
                                <TableCell align="right">Type</TableCell>
                            </TableRow>
                        </TableHead>

                        <TableBody>
                            {projectsStore.projectArray.map((row) => (
                                <TableRow key={row.id} component={Link} to={`/projects/${row.id}/`} style={{
                                    textDecoration: 'none'
                                }}>
                                    <TableCell align="left">
                                        {row.title}
                                    </TableCell>
                                    <TableCell align="left">
                                        {row.description}
                                    </TableCell>
                                    <TableCell align="right">
                                        {row.visibility == 0 ?
                                            <Typography>
                                                Public
                                            </Typography>
                                            :
                                            <Typography>
                                                Private
                                            </Typography>
                                        }
                                    </TableCell>
                                    <TableCell align="right">
                                        {row.programmingLanguage}
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>

                        <TableFooter>
                            <TableRow>
                                <TablePagination
                                    rowsPerPageOptions={[5, 10, 25, 50]}
                                    colSpan={3}
                                    count={projectsStore.allProjectsCount}
                                    rowsPerPage={rowsPerPage}
                                    page={page}
                                    slotProps={{
                                        select: {
                                            inputProps: {
                                                'aria-label': 'rows per page',
                                            },
                                            native: true,
                                        },
                                    }}
                                    sx={{margin: '0px'}}
                                    onPageChange={handleChangePage}
                                    onRowsPerPageChange={handleChangeRowsPerPage}
                                    ActionsComponent={TablePaginationActions}
                                />
                            </TableRow>
                        </TableFooter>
                    </Table>
                </TableContainer>
            </>
        );
    }
);
