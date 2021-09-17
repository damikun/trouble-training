import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import clsx from "clsx";
import React from "react";

const VARIANTS = {
  primarydark: {
    base: "text-gray-700 bg-gray-200",
    border: "border-gray-400 border-2 border-opacity-50",
  },
  primarygray: {
    base: "text-gray-500 bg-gray-50",
    border: "border-gray-200 border-2 border-opacity-50",
  },
  primarygreen: {
    base: "text-green-500 bg-green-50",
    border: "border-green-200 border-2 border-opacity-50",
  },
  primaryellow: {
    base: "text-yellow-400 bg-yellow-50",
    border: "border-yellow-300 border-2 border-opacity-50",
  },
  primarypink: {
    base: "text-pink-400 bg-pink-50",
    border: "border-pink-300 border-2 border-opacity-50",
  },
  primaryblue: {
    base: "text-blue-400 bg-blue-50",
    border: "border-blue-300 border-2 border-opacity-50",
  },
  primaryred: {
    base: "text-red-500 bg-red-50",
    border: "border-red-300 border-2 border-opacity-50",
  },
  secondarydark: {
    base: "bg-gray-500 text-white",
    border: "border-gray-600 border-2 border-opacity-50",
  },
  secondarygray: {
    base: "bg-gray-200 hover:text-gray-700",
    border: "border-gray-300 border-2 border-opacity-50",
  },
  secondarygreen: {
    base: "bg-green-500 text-white",
    border: "border-green-600 border-2 border-opacity-50",
  },
  secondaryellow: {
    base: "bg-yellow-400 text-white",
    border: "border-yellow-500 border-2 border-opacity-50",
  },
  secondaryblue: {
    base: "bg-blue-500 text-white",
    border: "border-blue-600 border-2 border-opacity-50",
  },
  secondaryred: {
    base: "bg-red-500 text-white",
    border: "border-red-600 border-2 border-opacity-50",
  },
  secondarypink: {
    base: "bg-pink-500 text-white",
    border: "border-pink-600 border-2 border-opacity-50",
  },
  ternarygray: {
    base: "bg-gray-50 text-gray-500 shadow-sm",
    border: "border-gray-500 border-l-2 border-opacity-50",
  },
  ternaryblue: {
    base: "bg-gray-50 text-blue-500 shadow-sm",
    border: "border-blue-500 border-l-2 border-opacity-50",
  },
  ternaryyellow: {
    base: "bg-gray-50 text-yellow-500 shadow-sm",
    border: "border-yellow-500 border-l-2 border-opacity-50",
  },
  ternarygreen: {
    base: "bg-gray-50 text-green-500 shadow-sm",
    border: "border-green-500 border-l-2 border-opacity-50",
  },
  ternaryred: {
    base: "bg-gray-50 text-red-500 shadow-sm",
    border: "border-red-500 border-l-2 border-opacity-50",
  },
  clean: {
    base: "",
    border: "",
  },
};

const ROUNDING = {
  small: "rounded-sm",
  normal: "rounded",
  medium: "rounded-md",
  large: "rounded-lg",
  full: "rounded-full",
  none: " ",
};

const SIZE = {
  auto: "h-full text-xs md:text-sm",
  nano: "h-4 text-xxs md:text-xxs",
  thin: "h-5 text-xxs md:text-xs",
  small: "h-6 text-xs md:text-sm",
  normal: "h-8 text-sm md:text-base",
  medium: "h-10 text-md md:text-lg",
  large: "h-12 text-lg md:text-xl",
};

export type BadgeProps = {
  children?: React.ReactNode;
  variant?: keyof typeof VARIANTS;
  size?: keyof typeof SIZE;
  rounding?: keyof typeof ROUNDING;
  className?: string;
  classNameWrapper?: string;
  icon?: IconProp;
  turncate?: boolean;
  onMobileIconOnly?: boolean;
  border?: boolean;
  shadow?: boolean;
};

export default React.memo(Badge);

function Badge({
  variant = "secondaryblue",
  children,
  size = "auto",
  className,
  classNameWrapper,
  icon,
  turncate = true,
  onMobileIconOnly = false,
  rounding = "large",
  border = true,
  shadow = false,
}: BadgeProps) {
  const Var_Style = VARIANTS[variant] || VARIANTS.clean;
  const Var_Rounding = ROUNDING[rounding] || ROUNDING.large;
  const Var_Size = SIZE[size] || SIZE.auto;

  if (children || icon) {
    return (
      <div className={clsx("flex", classNameWrapper)}>
        <div
          className={clsx(
            "flex px-1.5 font-semibold cursor-pointer",
            "content-center items-center select-none",
            Var_Style.base,
            Var_Size,
            Var_Rounding,
            className,
            border && Var_Style.border,
            shadow && "shadow-sm"
          )}
        >
          <div className="flex space-x-1 flex-nowrap">
            {icon && (
              <div>
                <FontAwesomeIcon icon={icon} />
              </div>
            )}
            {children && (
              <div
                className={clsx(
                  "flex-1 my-auto items-center",
                  "flex-nowrap my-auto",
                  onMobileIconOnly && "hidden md:flex"
                )}
              >
                <div
                  className={clsx(
                    turncate === true && "break-all truncate-1-lines my-auto"
                  )}
                >
                  {children}
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    );
  } else {
    return (
      <div
        className={clsx(
          "flex circle rounded-full text white",
          Var_Style,
          Var_Size,
          className
        )}
      >
        <div className="invisible whitespace-pre" />
      </div>
    );
  }
}
