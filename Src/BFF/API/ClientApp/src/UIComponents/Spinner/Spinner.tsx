import React from "react";
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import clsx from "clsx";

export type StyledSpinnerProps = {
  label?: string;
  size?: keyof typeof VARIANTS;
  flex?: keyof typeof FLEXVARIANT;
  className?: string;
};

const VARIANTS = {
  small: {
    spinner: "text-xs",
    label: "text-xs",
  },
  medium: {
    spinner: "text-md",
    label: "text-md",
  },
  default: {
    spinner: "text-base",
    label: "text-base",
  },
  large: {
    spinner: "text-lg",
    label: "text-lg",
  },
  extralarge: {
    spinner: "text-3xl",
    label: "text-xl",
  },
};

const FLEXVARIANT = {
  row: "flex-row",
  col: "flex-col",
};

export default React.memo(StyledSpinner);

function StyledSpinner({
  label,
  size = "default",
  flex = "row",
  className,
}: StyledSpinnerProps) {
  const SizeVar = VARIANTS[size] || VARIANTS.default;
  const FlexVar = FLEXVARIANT[flex] || FLEXVARIANT.row;

  return (
    <div
      className={clsx(
        "flex w-full mx-auto my-auto justify-center rounded-lg p-1",
        className
      )}
    >
      <div className={clsx("flex mx-auto my-auto px-1", FlexVar)}>
        <div
          className={clsx(
            "flex items-center justify-center font-thin",
            SizeVar.spinner
          )}
        >
          <FontAwesomeIcon
            className=" flex mx-auto justify-center animate-spin"
            icon={faSpinner}
          />
        </div>
        {label && (
          <div
            className={clsx("pl-2 mx-auto my-auto font-bold", SizeVar.label)}
          >
            {label ? label : ""}
          </div>
        )}
      </div>
    </div>
  );
}
