import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Container from "@mui/material/Container";
import {useLocation} from "react-router-dom";

export default function NotFound() {
    return (
        <>
            <Container>
                <Box mt={5}>
                    <Typography variant="h4" color="error">
                        404 NotFound
                    </Typography>
                </Box>
            </Container>
        </>
    )
}
