import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';


import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import {AuthProvider} from "react-oidc-context";


const oidcConfig = {
    authority: 'http://localhost:5001',
    client_id: 'react-client',
    redirect_uri: 'http://localhost:3000/signin-oidc',
    post_logout_redirect_uri: 'http://localhost:3000/signout-callback-oidc',
    scope: 'openid profile api1',
    onSigninCallback: () => {
        window.history.replaceState({}, document.title, '/');
    }
};


const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);
root.render(
    <React.StrictMode>
        <AuthProvider {...oidcConfig}>
            <App/>
        </AuthProvider>
    </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
