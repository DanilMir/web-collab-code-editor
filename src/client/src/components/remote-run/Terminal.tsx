import React from 'react';
import {VncScreen} from "react-vnc";

export default function Terminal() {
    return (
        <>
            <VncScreen
                url='ws://localhost:6901'
                scaleViewport
                style={{
                    width: '50vw',
                    height: '75vh',
                }}
                autoConnect={true}
                rfbOptions={{credentials : {password: "vncpassword"}}}
            />
        </>
    )
}
