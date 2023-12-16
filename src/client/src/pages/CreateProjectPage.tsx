import React, {useState} from "react";
import {CssBaseline, FormControl, InputLabel, Select, TextField} from "@mui/material";
import MenuItem from "@mui/material/MenuItem";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Container from "@mui/material/Container";
import Avatar from "@mui/material/Avatar";
import Typography from "@mui/material/Typography";
import CodeIcon from '@mui/icons-material/Code';
import {useRootStore} from "../hooks/useRootStore";
import {useAuth} from "react-oidc-context";
import {ProjectCreateModel} from "../moduls/projects/ProjectModel";

export default function CreateProjectPage() {
    const {projectStore} = useRootStore()
    const auth = useAuth();

    const [formData, setFormData] = useState<ProjectCreateModel>({
        title: '123',
        description: "",
        visibility: 0,
        programmingLanguage: 'python'
    });


    const handleChange = (event : any) => {
        setFormData({
            ...formData,
            [event.target.name]: event.target.value,
        });
    };
    const handleSubmit = (e: any) => {
        e.preventDefault();
        projectStore.createProject(formData, auth.user?.access_token!);
    };

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline/>

            <Box style={{
                marginTop: 8,
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center'
            }}>

                <Avatar sx={{m: 1, bgcolor: 'secondary.main'}}>
                    <CodeIcon/>
                </Avatar>
                <Typography component="h1" variant="h5">
                    Create project
                </Typography>

                <Box component="form" onSubmit={handleSubmit} noValidate sx={{mt: 1}}>
                    <TextField fullWidth onChange={handleChange} name="title" value={formData.title} required id="title" label="Title" variant="outlined"/>
                    <TextField fullWidth onChange={handleChange} name="description" value={formData.description} id="description" label="Description" variant="outlined"/>


                    <FormControl fullWidth>
                        <InputLabel required id="visibility">Visibility</InputLabel>
                        <Select
                            fullWidth
                            labelId="demo-simple-select-label"
                            id="visibility"
                            label="Visibility"
                            name="visibility"
                            value={formData.visibility}
                            onChange={handleChange}
                        >
                            <MenuItem value={0}>Public</MenuItem>
                            <MenuItem value={1}>Private</MenuItem>
                        </Select>

                    </FormControl>

                    <FormControl fullWidth>
                        <InputLabel required id="language">Language</InputLabel>
                        <Select
                            fullWidth
                            required={true}
                            labelId="demo-simple-select-label"
                            id="language"
                            label="Language"
                            name="programmingLanguage"
                            value={formData.programmingLanguage}
                            onChange={handleChange}
                        >
                            <MenuItem value="C#">C#</MenuItem>
                            <MenuItem value="python">Python</MenuItem>
                        </Select>
                    </FormControl>
                    <Button fullWidth type="submit" variant="contained" color="primary">
                        Create Project
                    </Button>

                </Box>
            </Box>
        </Container>
    )
}