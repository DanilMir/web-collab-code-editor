import Container from "@mui/material/Container";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";

export default function Forbidden() {
    return (
        <>
            <Container>
                <Box mt={5}>
                    <Typography variant="h4" color="error">
                        403 Forbidden
                    </Typography>
                    <Typography variant="body1">
                        Access to this resource is forbidden. Please contact the administrator for assistance.
                    </Typography>
                </Box>
            </Container>
        </>
    )
}
