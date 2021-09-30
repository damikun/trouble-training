import React from "react";
import clsx from "clsx";

export type LayoutProps = {
  content: React.ReactNode;
};

export default React.memo(Layout);

function Layout({ content }: LayoutProps) {

  return (
    <div
      className={clsx(
        "flex flex-col h-screen w-screen overflow-hidden",
        "bg-gradient-to-t relative",
        "from-gray-166 via-gray-50 to-gray-166"
      )}
    >

      {<div className="flex-1 h-full w-full max-h-full">{content}</div>}

    </div>
  );
}
