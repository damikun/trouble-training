import {  useEffect } from "react";
import { LOGOUT_ENDPOINT } from "../constants";
import { useUserStore } from "../Utils/UserProvider";

export default function Logout() {

  const store = useUserStore();
    
      useEffect(() => {
        if(store?.user?.me?.sessionId){
          window.location.href = `${LOGOUT_ENDPOINT}?sid=${store?.user?.me?.sessionId}`;
        }else{
          window.location.href = LOGOUT_ENDPOINT;
        }

      }, [])

      return <></>
  }
  