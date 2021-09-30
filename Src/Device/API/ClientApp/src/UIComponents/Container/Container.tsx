import clsx from "clsx";
import React from "react";

export type ContainerProps = {
  children?: React.ReactNode;
  className?: string;
  flexwrap?: "flex-wrap" | "flex-no-wrap";
  flextype?: "flex" | "flex-1" | "flex-initial" | "flex-none";
};

export default function Container({
  children,
  className,
  flexwrap = "flex-wrap",
  flextype = "flex",
}: ContainerProps) {
  return <div className={clsx(flexwrap, flextype, className)}>{children}</div>;
}
