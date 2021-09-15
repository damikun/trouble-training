import {  useEffect } from "react";
import { BASE_SERVER_URL } from "../constants";

export default function Login() {

      useEffect(() => {
        window.location.href = `${BASE_SERVER_URL}/system/login`;
      }, [])

      return <></>
  }
  