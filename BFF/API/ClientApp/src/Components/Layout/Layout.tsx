import React from "react";
import clsx from "clsx";
import { useUserStore } from "../../Utils/UserProvider";

export type LayoutProps = {
  children?: React.ReactNode;
  content: React.ReactNode;
};

export default React.memo(Layout);

function Layout({ content }: LayoutProps) {
  // const store = useUserStore();

  return (
    <div
      className={clsx(
        "flex flex-col h-screen w-screen overflow-hidden",
        "bg-gradient-to-t",
        "from-gray-166 via-gray-50 to-gray-166"
      )}
    >
      {/* {header && store?.user?.me?.id ? (
        <nav className="flex-initial w-full">{header}</nav>
      ) : (
        <></>
      )} */}

      {<div className="flex-1 h-full w-full max-h-full">{content}</div>}

      {/* {mobileBar && <div>{mobileBar}</div>} */}
    </div>
  );
}
