import {  useEffect } from "react";
import { LOGIN_ENDPOINT } from "../constants";

export default function Login() {

      useEffect(() => {
        window.location.href = LOGIN_ENDPOINT;
      }, [])

      return <></>
  }
  