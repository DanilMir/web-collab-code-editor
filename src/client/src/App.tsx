import './App.css';
import NavBar from "./components/navigation/NavBar";
import {BrowserRouter, Routes, Route, Navigate} from 'react-router-dom';
import NotFound from './pages/errors/NotFound';
import {ProjectsListPage} from './pages/ProjectsListPage';
import CreateProjectPage from "./pages/CreateProjectPage";
import EditProjectPage from "./pages/EditProjectPage";
import HomePage from "./pages/HomePage";
import {ProjectPage} from "./pages/ProjectPage";

function App() {

    return (
        <>
            <BrowserRouter>
            <NavBar />
                <Routes>
                    <Route path="/projects/:id" element={<ProjectPage />} />d
                    <Route path="/projects/:id/edit" element={<EditProjectPage />} />d
                    <Route path="/projects/" element={<ProjectsListPage />} />
                    <Route path="/projects/create" element={<CreateProjectPage />} />

                    <Route path='/' element={<HomePage />} />
                    <Route path='*' element={<NotFound />} />
                </Routes>
            </BrowserRouter>
        </>);
}

export default App;
