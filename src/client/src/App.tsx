import './App.css';
import NavBar from "./components/NavBar";
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Project from "./pages/Project";
import NotFound from './pages/errors/NotFound';
import ProjectsListPage from './pages/ProjectsListPage';

function App() {

    return (
        <>
            <BrowserRouter>
            <NavBar />
                <Routes>
                    <Route path="/projects/:id" element={<Project />} />
                    <Route path="/projects/" element={<ProjectsListPage />} />


                    <Route path='*' element={<NotFound />} />
                </Routes>
            </BrowserRouter>
        </>);
}

export default App;