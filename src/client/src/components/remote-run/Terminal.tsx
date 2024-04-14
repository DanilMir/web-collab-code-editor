import React from 'react';
import {VncScreen} from "react-vnc";

interface Props {
    port: number,
    password: string
}

export default function Terminal(props: Props) {
    return (
        <>
            <VncScreen
                url={`ws://localhost:${props.port}`}
                scaleViewport
                style={{
                    width: '50vw',
                    height: '80vh',
                }}
                autoConnect={true}
                rfbOptions={{credentials : {password: "vncpassword"}}}
            />
        </>
    )
}
