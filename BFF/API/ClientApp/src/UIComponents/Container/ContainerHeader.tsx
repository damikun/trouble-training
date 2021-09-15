import clsx from "clsx";
import React from "react";

type Props = {
  name?: string;
  content?: React.ReactNode;
  textsize?: "text-xs" | "text-sm" | "text-base" | "text-lg";
  className?: String;
};

export default React.memo(ContainerHeader);

function ContainerHeader({
  name,
  className,
  content,
  textsize = "text-base",
}: Props) {
  return (
    <div
      className={clsx(
        textsize,
        "flex font-semibold flex-col w-full flex-no-wrap",
        className
      )}
    >
      {name ? (
        <div className="align-content-start">{name}</div>
      ) : content ? (
        content
      ) : null}
    </div>
  );
}
