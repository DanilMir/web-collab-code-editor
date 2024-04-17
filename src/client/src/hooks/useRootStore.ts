import { storesContext } from "../RootStore";
import React from "react";

export const useRootStore = () => React.useContext(storesContext);
