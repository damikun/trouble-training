import clsx from "clsx";
import React from "react";

export type StayledLabelProps = {
  children: React.ReactNode;
} & React.DetailedHTMLProps<
  React.LabelHTMLAttributes<HTMLLabelElement>,
  HTMLLabelElement
>;
export default function StayledLabel({ children, ...rest }: StayledLabelProps) {
  return (
    <label
      className={clsx(
        "flex text-sm sm:text-base break-normal font-semibold px-3",
        rest.className
      )}
      {...rest}
    >
      {children}
    </label>
  );
}
