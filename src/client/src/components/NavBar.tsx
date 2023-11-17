import { useAuth } from "react-oidc-context";
import { Link } from "react-router-dom";

export default function NavBar() {
    const auth = useAuth();
    return (
        <>
            <nav className="navbar navbar-expand-lg navbar-light bg-light">
                <div className="container-fluid">
                    <Link to='' className="navbar-brand">Navbar w/ text</Link>
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse"
                        data-bs-target="#navbarText" aria-controls="navbarText" aria-expanded="false"
                        aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarText">
                        <ul className="navbar-nav me-auto mb-2 mb-lg-0"></ul>
                        <ul className="navbar-nav mb-2 mb-lg-0">
                            <li className="nav-item">
                                <Link to='/projects' className="nav-link active" aria-current="page">Projects</Link>
                            </li>

                            {!auth.isAuthenticated ?
                                <li className="nav-item">
                                    <button className="nav-link" onClick={() => auth.signinRedirect()}>Sign In</button>
                                </li>
                                :
                                <>
                                    <li className="nav-item">
                                        <a className="nav-link" href="#">Profile</a>
                                    </li>
                                    <li className="nav-item">
                                        <button className="nav-link" onClick={() => auth.signoutRedirect()}>Sign Out
                                        </button>
                                    </li>
                                </>
                            }
                        </ul>
                    </div>
                </div>
            </nav>
        </>
    )
}