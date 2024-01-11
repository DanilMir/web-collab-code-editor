import Container from "@mui/material/Container";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";

export default function LoginWarning() {
    return (
        <>
            <Container>
                <Box mt={5}>
                    <Typography variant="h4" color="error">
                        401 Unauthorized
                    </Typography>
                </Box>
            </Container>
        </>
    )
}
