import React from "react";
import { Navigate, RouteProps } from "react-router";
import { Route } from "react-router-dom";
import { useUserStore } from "./UserProvider";

type Props = {
  authorised?: boolean | undefined;
  unauthorisedUrl?: string;
  unauthorisedComponent?: React.ReactNode;
} & RouteProps;

export default function PrivateRouter({
  children,
  authorised = undefined,
  unauthorisedUrl,
  unauthorisedComponent,
  ...rest
}: Props) {
  //@ts-ignore
  const userStore = useUserStore();

  if (!userStore?.user?.me) {
    return <Navigate to="/login" replace state={undefined} />;
  }

  if (authorised !== undefined && authorised === false) {
    if (unauthorisedComponent) {
      return <Route element={<>{unauthorisedComponent}</>} />;
    } else {
      return (
        <Navigate
          to={unauthorisedUrl ? unauthorisedUrl : "/unauthorised"}
          replace
          state={undefined}
        />
      );
    }
  }

  return <Route {...rest}>{children}</Route>;
}
