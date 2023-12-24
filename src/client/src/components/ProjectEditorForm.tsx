import {
    createTheme,
    CssBaseline,
    FormControl,
    InputLabel,
    makeStyles,
    Select,
    SelectChangeEvent,
    TextField,
    Theme
} from "@mui/material";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import CodeIcon from "@mui/icons-material/Code";
import Typography from "@mui/material/Typography";
import MenuItem from "@mui/material/MenuItem";
import Button from "@mui/material/Button";
import Container from "@mui/material/Container";
import React from "react";
import {ProjectEditorModel} from "../moduls/projects/ProjectModel";

interface IParams {
    handleSubmit: (e: any) => void,
    handleChange: (e: any) => void,
    formData: ProjectEditorModel,
    title: string
}

export default function ProjectEditorForm({handleSubmit, handleChange, formData, title}: IParams) {
    return (
        <>
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
                        {title}
                    </Typography>

                    <Box component="form" onSubmit={handleSubmit} noValidate sx={{mt: 1}}>
                        <TextField fullWidth onChange={handleChange} name="title" value={formData.title} required
                                   id="title" label="Title" variant="outlined"
                                   margin="normal"/>
                        <TextField fullWidth onChange={handleChange} name="description" value={formData.description}
                                   id="description" label="Description" variant="outlined" margin="normal"/>


                        <FormControl fullWidth margin="normal">
                            <InputLabel required id="visibility">Visibility</InputLabel>
                            <Select
                                fullWidth
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

                        <FormControl fullWidth margin="normal">
                            <InputLabel required id="language">Language</InputLabel>
                            <Select
                                fullWidth
                                required={true}
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
                        <Button fullWidth type="submit" variant="contained" color="primary" sx={{mt: 3}}>
                            Create Project
                        </Button>

                    </Box>
                </Box>
            </Container>
        </>
    )
}
