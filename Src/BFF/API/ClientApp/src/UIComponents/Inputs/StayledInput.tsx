import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import clsx from "clsx";

export type StayledInput = {
  icon?: IconProp;
  className?: string;
  textstyle?: "capitalize" | "lowercase" | "uppercase";
  isError?: boolean;
  border?: boolean;
} & React.DetailedHTMLProps<
  React.InputHTMLAttributes<HTMLInputElement>,
  HTMLInputElement
>;

export default function StayledInput({
  icon,
  className,
  textstyle,
  isError,
  border = true,
  ...rest
}: StayledInput) {
  return (
    <div
      className={clsx(
        "flex flex-row justify-start align-middle outline",
        "content-center rounded-md transition duration-200 focus:bg-white",
        className,
        border === false ? "border-none p-0" : "border p-1",
        isError && border
          ? " border-red-500 "
          : " border-gray-400 focus-within:border-blue-500 hover:border-blue-500",
        rest.disabled ? "cursor-not-allowed" : "cursor-pointer"
      )}
    >
      <label className="mx-auto font-normal text-xs my-auto align-content-center">
        {icon ? (
          <FontAwesomeIcon
            className="align-content-center align-content-center "
            icon={icon}
          />
        ) : null}
      </label>
      <input
        {...rest}
        className={clsx(
          "w-full placeholder-gray-700 z-20",
          "my-auto bg-transparent outline-none border-transparent",
          textstyle && textstyle,
          border === false ? "mx-0" : "mx-1"
        )}
      ></input>
    </div>
  );
}
