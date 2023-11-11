import logo from './logo.svg';
import './App.css';
import {useAuth} from "react-oidc-context";
import NavBar from "./components/NavBar";
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Project from "./pages/Project";
import NotFound from './pages/errors/NotFound';

function App() {

  return (
      <>
          <NavBar/>
          <BrowserRouter>
              <Routes>
                  <Route path="/projects/:id" element={<Project/>}/>



                  <Route path='*' element={<NotFound />} />
              </Routes>
          </BrowserRouter>
      </>);
}

export default App;
