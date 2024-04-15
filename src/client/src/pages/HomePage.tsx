import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import { styled } from '@mui/system';
import Button from "@mui/material/Button";
import BrowserIcon from "../components/icons/BrowserIcon";
import {SvgIcon} from "@mui/material";
import MainPage from "../components/illustrations/MainPage";
import {useAuth} from "react-oidc-context";
import {useNavigate} from "react-router-dom";


const MainContainer = styled(Container)({
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    height: '100vh',
});

const Title = styled(Typography)({
    marginBottom: '20px',
});

const Description = styled(Typography)({
    marginBottom: '40px',
    textAlign: 'center',
});

const Image = styled('img')({
    maxWidth: '100%',
    height: 'auto',
    marginBottom: '40px',
});

export default function HomePage() {
    const auth = useAuth();
    const navigate = useNavigate();

    return (
        <Container sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            flexDirection: 'column',
            paddingTop: 10
        }}>
            <Title variant="h2">
                Welcome to Collab Code Editor
            </Title>
            <Description variant="body1">
                Collaboratively write, edit and run code with your team in real-time.
            </Description>
            <SvgIcon sx={{width: "500px", height: "500px"}}>
                <MainPage/>
            </SvgIcon>
            <Button variant="contained" color="primary" size="large" onClick={() => {
                if(!auth.isAuthenticated) {
                    auth.signinRedirect()
                } else {
                    navigate('projects')
                }
            }}>
                Get Started
            </Button>
        </Container>
    );
}
