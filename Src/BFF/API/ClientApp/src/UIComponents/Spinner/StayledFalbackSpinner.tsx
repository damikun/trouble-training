import clsx from "clsx";
import React from "react";
import Spiner from "./Spinner";

export type StayledFallbackSpinnerProps = {
  className?: string;
};

export default React.memo(StayledFallbackSpinner);

function StayledFallbackSpinner({ className }: StayledFallbackSpinnerProps) {
  return (
    <div
      className={clsx(
        "flex flex-col w-40 h-40 select-text my-auto",
        "rounded-md shadow-lg py-5 px-5 bg-white",
        className && className
      )}
    >
      <Spiner />
    </div>
  );
}
