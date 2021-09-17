import clsx from "clsx";
import React, { CSSProperties } from "react";

export declare type flextype =
  | "flex" // Keeps w
  | "flex-1" // Row / col  (no wrap) with auto width
  | "flex-auto"; // Row / col, responsive wrap and auto width

export default function ContainerItem({
  ref,
  fullwidth,
  flextype,
  marginx,
  bgcolor,
  children,
  className,
  rounded = "rounded",
  shahow = "shadow-md",
  onClick,
  style,
}: {
  ref?:
    | string
    | ((instance: HTMLDivElement | null) => void)
    | React.RefObject<HTMLDivElement>
    | null
    | undefined;
  fullwidth?: boolean;
  flextype?: flextype;
  marginx?: number;
  bgcolor?: string;
  children: React.ReactNode;
  shahow?:
    | "shadow"
    | "shadow-none"
    | "shadow-xs"
    | "shadow-sm"
    | "shadow-md"
    | "shadow-lg";
  rounded?:
    | "rounded"
    | "rounded-sm"
    | "rounded-md"
    | "rounded-lg"
    | "rounded-xl"
    | "rounded-b-sm"
    | "rounded-b-md"
    | "rounded-b-lg"
    | "rounded-b-xl"
    | "rounded-b"
    | "rounded-t-sm"
    | "rounded-t-md"
    | "rounded-t-lg"
    | "rounded-t"
    | "rounded-none"
    | "rounded-full";
  style?: CSSProperties;

  className?: string;
  onClick?: (
    event: React.MouseEvent<HTMLDivElement, MouseEvent>
  ) => void | undefined;
}) {
  return (
    <div
      style={style}
      ref={ref}
      onClick={onClick}
      className={clsx(
        fullwidth && "w-full",
        flextype,
        "mx-" + marginx,
        shahow,
        !bgcolor ? "bg-white" : bgcolor,
        "flex items-center justify-center",
        rounded,
        className
      )}
    >
      {children}
    </div>
  );
}
