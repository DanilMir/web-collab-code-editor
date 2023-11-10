import React, {useEffect, useState} from 'react';
import logo from './logo.svg';
import './App.css';
import {useAuth} from "react-oidc-context";

function App() {
  const loggedOutMessage = 'You need to log in to be able to retrieve the data';
  const loggedInMessage = 'Click "Get weather forecast" button to retrieve the data';

  const auth = useAuth();
  const [data, setData] = useState<string>(
      auth.isAuthenticated
          ? loggedInMessage
          : loggedOutMessage);

  useEffect(() => {
    setData(auth.isAuthenticated ? loggedInMessage : loggedOutMessage)
  }, [auth]);

  switch (auth.activeNavigator) {
    case 'signinSilent':
      return <div>Signing you in...</div>;
    case 'signoutRedirect':
      return <div>Signing you out...</div>;
  }

  if (auth.isLoading) {
    return <div>Loading...</div>;
  }

  if (auth.error) {
    return <div>Error: {auth.error.message}</div>;
  }

  async function fetchData() {
    const request = new Request('https://localhost:7212/WeatherForecast', {
      headers: new Headers({
        'Authorization': `Bearer ${(auth.user!.access_token)}`,
        'X-CSRF': '1'
      })
    });

    let data: string;
    try {
      const response = await fetch(request);

      if (response.ok) {
        let jsonResponse = await response.json();
        data = JSON.stringify(jsonResponse, null, 2);
      } else {
        data = await response.text();
      }
    } catch (e) {
      data = 'Error getting weather forecast: ' + e;
    }

    setData(data);
  }

  return (
      <>
        {auth.isAuthenticated ?
            <>
              <p>
                {/*<button onClick={() => auth.removeUser()}>Log out</button>*/}
                <button onClick={() => auth.signoutRedirect()}>Log out</button>
                &nbsp;
                <button onClick={() => fetchData()}>Get weather forecast</button>
              </p>
            </>
            :
            <p>
              <button onClick={() => auth.signinRedirect()}>Log in</button>
            </p>
        }
        <pre>
        {data}
      </pre>
      </>);
}

export default App
