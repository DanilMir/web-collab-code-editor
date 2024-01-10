import {
    createTheme,
    CssBaseline,
    FormControl, Grid,
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
import React, {useState} from "react";
import {red} from "@mui/material/colors";
import {ProjectEditorModel} from "../../moduls/project/ProjectModel";

interface IParams {
    handleSubmit: (e: any) => void,
    handleChange: (e: any) => void,
    formData: ProjectEditorModel,
    title: string
}

export default function ProjectEditorForm({handleSubmit, handleChange, formData, title}: IParams) {

    return (
        <>
            <Container component="main" maxWidth="xs" sx={{paddingBottom: '20px'}}>
                <CssBaseline/>

                <Box style={{
                    marginTop: 8,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center'
                }}>
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
                            {title}
                        </Button>

                    </Box>
                </Box>
            </Container>
        </>
    )
}
