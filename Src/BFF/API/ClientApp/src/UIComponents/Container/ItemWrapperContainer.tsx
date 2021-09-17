import clsx from "clsx";
import React from "react";
type ItemWrapperContainerProps = {
  children: React.ReactNode;
  className?: string;
  innerClassName?: string;
  turncate?: "truncate-1-lines" | "truncate-2-lines" | "truncate-3-lines";
};

export function ItemWrapperContainer({
  children,
  className,
  turncate,
  innerClassName,
}: ItemWrapperContainerProps) {
  return (
    <div
      className={clsx(
        "flex flex-wrap relative bg-white w-full max-w-full",
        "rounded-md shadow-xs w-full text-xs md:text-sm h-full",
        "break-words overflow-hidden overflow-y-auto",
        "items-center align-middle w-full px-5 py-3",
        className
      )}
    >
      <div
        className={clsx(
          "flex-1 w-full max-w-full overflow-hidden whitespace-pre-wrap",
          turncate,
          innerClassName
        )}
      >
        {children}
      </div>
    </div>
  );
}
