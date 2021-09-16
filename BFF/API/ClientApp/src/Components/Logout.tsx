import {  useEffect } from "react";
import { LOGOUT_ENDPOINT } from "../constants";

export default function Logout() {

      useEffect(() => {
        window.location.href = LOGOUT_ENDPOINT;
      }, [])

      return <></>
  }
  