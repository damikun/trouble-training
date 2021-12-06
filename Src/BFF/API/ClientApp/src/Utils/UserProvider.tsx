import React, { useContext, useMemo } from "react";
import { graphql } from "babel-plugin-relay/macro";
import {  usePreloadedQuery } from "react-relay/hooks";
import * as MeQuery from "../Utils/__generated__/UserProviderQuery.graphql";
import { PreloadedQuery } from "react-relay";

export const UserProviderQueryTag = graphql`
  query UserProviderQuery {
    me {
      id
      name
      sessionId
    }
  }
`;

export type usertype = {
    readonly me: {
      readonly id: string | null;
      readonly name: string | null;
      readonly sessionId: string | null;
  } | null;
} | null;

export type UserProviderProps = {
  children?: React.ReactNode;
  initialQueryRef: PreloadedQuery<MeQuery.UserProviderQuery>;
};

type userStoreContextType = {
  user: usertype;
};

export const userStoreContext = React.createContext<
  userStoreContextType | undefined
>(undefined);

export const useUserStore = () => useContext(userStoreContext);

export default function UserProvider({ children,initialQueryRef }: UserProviderProps) {

  // const preloaded_user_data = useLazyLoadQuery<UserProviderQuery>(
  //   UserProviderQueryTag,
  //   {},
  //   { fetchPolicy: "store-or-network" }
  // );

  const preloaded_user_data = usePreloadedQuery(
    UserProviderQueryTag,
    initialQueryRef
  );
  
  const userStoreInitCtx = useMemo(() => {
    return {
      user: preloaded_user_data,
    };
  }, [preloaded_user_data]);

  return (
    <userStoreContext.Provider value={userStoreInitCtx}>
      {children}
    </userStoreContext.Provider>
  );
}
