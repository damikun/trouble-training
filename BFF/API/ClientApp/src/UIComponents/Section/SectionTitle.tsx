import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import clsx from "clsx";
import React from "react";

export type SectionTitleProps = {
  name?: string;
  children?: React.ReactNode | string;
  className?: string;
  hideBoubleDot?: boolean;
  mdIconOnly?: boolean;
  variant?: keyof typeof STYLE_VARINAT;
  icon?: IconProp;
};

const STYLE_VARINAT = {
  upercase: "font-extrabold tracking-wider uppercase",
  lovercase: "font-bold break-normal tracking-wide",
};

export default React.memo(SectionTitle);

function SectionTitle({
  name,
  icon,
  children,
  className,
  mdIconOnly,
  variant = "upercase",
  hideBoubleDot,
}: SectionTitleProps) {
  const style = variant
    ? STYLE_VARINAT[variant] || STYLE_VARINAT.upercase
    : STYLE_VARINAT.upercase;

  return (
    <div
      className={clsx(
        "flex flex-nowrap text-gray-700 ",
        "text-xs md:text-sm items-center",
        "flex-nowrap",
        className,
        style
      )}
    >
      {icon && (
        <FontAwesomeIcon
          className={clsx(mdIconOnly ? "hidden md:flex md:mr-1" : "mr-1")}
          icon={icon}
        />
      )}

      <h3 className="whitespace-pre">
        {children ? children : `${name}${!hideBoubleDot ? ":" : ""}`}
      </h3>
    </div>
  );
}
