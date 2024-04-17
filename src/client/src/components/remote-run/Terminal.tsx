import React, { createRef, useEffect, useRef, useState } from "react";
import { VncScreen, VncScreenHandle } from "react-vnc";
import { CircularProgress } from "@mui/material";
import Box from "@mui/material/Box";
import { useRootStore } from "../../hooks/useRootStore";

interface Props {
  port: number;
  password: string;
}

export default function Terminal(props: Props) {
  const { sandboxStore } = useRootStore();
  const sandboxRef = createRef<VncScreenHandle>();
  const [connected, setConnected] = useState<boolean | undefined>(false);
  const varRef = useRef(connected);

  useEffect(() => {
    return () => {
      if (!varRef.current) {
        sandboxStore.setActive(false);
      }
    };
  }, []);

  useEffect(() => {
    setConnected(sandboxRef.current?.connected);
    varRef.current = sandboxRef.current?.connected;
  }, [sandboxRef.current?.connected]);

  return (
    <>
      <VncScreen
        ref={sandboxRef}
        url={`ws://localhost:${props.port}`}
        scaleViewport
        style={{
          width: "50vw",
          height: "80vh",
        }}
        loadingUI={
          <Box
            sx={{
              position: "absolute",
              top: "50%",
              right: "24%",
            }}
          >
            <Box
              sx={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
              }}
            >
              <svg width={0} height={0}>
                <defs>
                  <linearGradient
                    id="my_gradient"
                    x1="0%"
                    y1="0%"
                    x2="0%"
                    y2="100%"
                  >
                    <stop offset="0%" stopColor="#e11cd5" />
                    <stop offset="100%" stopColor="#2CB5E0" />
                  </linearGradient>
                </defs>
              </svg>
              <CircularProgress
                sx={{ "svg circle": { stroke: "url(#my_gradient)" } }}
              />
            </Box>
          </Box>
        }
        autoConnect={true}
        retryDuration={10000}
        rfbOptions={{ credentials: { password: "vncpassword" } }}
      />
    </>
  );
}
