import React, { useEffect, useRef } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import StayledLabel from "../Label/StayledLabel";
import clsx from "clsx";

export type StayledInput = {
  icon?: IconProp;
  children?: React.ReactNode;
  className?: string;
  textstyle?: "capitalize" | "lowercase" | "uppercase";
  label?: string;
  isError?: string | undefined;
  variant?: keyof typeof VARIANTS;
  height?: string;
  reserveValidationErrorSpace?: boolean;
  labelstyle?: string;
  leftPadding?: number;
  focuseOnMount?: boolean;
} & React.DetailedHTMLProps<
  React.InputHTMLAttributes<HTMLInputElement>,
  HTMLInputElement
>;

const VARIANTS = {
  inline: {
    basestyle:
      "flex w-full flex-row flex-no-wrap my-auto space-x-4 flex-grow-0 break-normal",
    errorstyle:
      "flex flex-row flex-no-wrap my-auto space-x-2 flex-grow-0 break-normal",
  },
  inrow: {
    basestyle: "w-full flex flex-col flex-no-wrap",
    errorstyle: "flex flex-col flex-no-wrap",
  },
  inlineinrow: {
    basestyle:
      "flex w-full flex-row flex-no-wrap space-x-4 flex-grow-0 break-normal",
    errorstyle: "flex flex-col flex-no-wrap",
  },
};

type RenderErrorLabelProps = {
  error: string | undefined;
  reservearea?: boolean;
};

function RenderErrorLabel({ error, reservearea }: RenderErrorLabelProps) {
  return (
    <label
      className={clsx(
        "font-normal text-xs my-auto truncate ",
        reservearea ? (error ? " visible " : " invisible ") : " visible"
      )}
    >
      {reservearea ? `(${error})` : error && `(${error})`}
    </label>
  );
}

export default React.memo(StayledFormInput);

function StayledFormInput({
  icon,
  className,
  label,
  textstyle,
  isError,
  children,
  variant = "inrow",
  reserveValidationErrorSpace,
  leftPadding = 0,
  height,
  labelstyle,
  focuseOnMount,
  ...rest
}: StayledInput) {
  const Var = VARIANTS[variant] || VARIANTS.inlineinrow;

  const ref = useRef<HTMLInputElement | null>(null);

  useEffect(() => {
    setTimeout(() => {
      if (ref.current && focuseOnMount) {
        ref.current?.focus();
      }
    }, 50);
  }, [ref.current]);

  return (
    <div className={clsx(Var.basestyle, "space-y-1.5")}>
      {label && (
        <StayledLabel
          className={clsx(
            "flex ",
            labelstyle
              ? labelstyle
              : clsx(
                  "text-sm sm:text-base font-semibold",
                  "align-items-center pb-2 flex-no-wrap",
                  "flex-grow-0 break-normal"
                )
          )}
          htmlFor={rest.id ? rest.id : rest.name ? rest.name : ""}
        >
          {`${label}:`}
        </StayledLabel>
      )}
      <div className={Var.errorstyle}>
        <div
          className={clsx(
            "flex flex-row my-auto justify-start align-middle",
            "outline content-center p-1 my-2 border-2 rounded-md",
            "transition duration-200 focus:bg-white",
            "focus-within:bg-white",
            isError
              ? " border-red-500 "
              : clsx(
                  "border-gray-100 focus-within:border-blue-500",
                  "hover:border-blue-500"
                ),
            className ? className : "bg-gray-50"
          )}
        >
          {icon && (
            <label
              className={clsx(
                "mx-1 font-normal text-xs my-auto",
                "align-content-center"
              )}
            >
              <FontAwesomeIcon
                className="align-content-center align-content-center"
                icon={icon}
              />
            </label>
          )}
          <input
            ref={ref}
            {...rest}
            value={rest.value == null ? "" : rest.value}
            className={clsx(
              "mx-1 w-full text-gray-500 focus:text-gray-700",
              "my-auto placeholder-gray-500",
              "outline-none border-transparent",
              "bg-transparent font-semibold",
              height,
              textstyle,
              rest.disabled ? "cursor-not-allowed" : "cursor-pointer"
            )}
          >
            {children}
          </input>
        </div>
        {reserveValidationErrorSpace !== undefined && (
          <RenderErrorLabel
            error={isError}
            reservearea={reserveValidationErrorSpace}
          />
        )}
      </div>
    </div>
  );
}
