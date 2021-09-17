import { faFilter, IconDefinition } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import clsx from "clsx";
import { To } from "history";
import React, { useRef } from "react";
import ContextMenu from "../ContextMenu/ContextMenu";
import { Link } from "../Link/Link";

const ROUNDING_VARIANT = {
  none: {
    all: "",
    firstandlast: "",
  },
  small: {
    all: "rounded-sm",
    firstandlast: "last:rounded-b-sm first:rounded-t-sm",
  },
  normal: {
    all: "rounded",
    firstandlast: "last:rounded-b first:rounded-t",
  },
  medium: {
    all: "rounded-md",
    firstandlast: "last:rounded-b-md first:rounded-t-md",
  },
  large: {
    all: "rounded-lg",
    firstandlast: "last:rounded-b-lg first:rounded-t-lg",
  },
  full: {
    all: "rounded-lg",
    firstandlast: "last:rounded-b-full first:rounded-t-full",
  },
};

type Props = {
  onClick?:
    | ((event: React.MouseEvent<HTMLDivElement, MouseEvent>) => void)
    | undefined;
  className?: string;
  bgColor?: string;
  hoverEffect?: boolean;
  rounding?: keyof typeof ROUNDING_VARIANT;
  roundingType?: "all" | "firstlast" | "none";
  to?: To;
} & ItemContentProps;
export type Ref = HTMLDivElement;

const StayledContainerItemWrapper = React.forwardRef<Ref, Props>(
  (
    {
      className,
      onClick,
      bgColor,
      to,
      children,
      isSelected,
      rounding = "medium",
      roundingType = "firstlast",
      hoverEffect = true,
      ...rest
    }: Props,
    ref
  ) => {
    const Rounding_Var = rounding
      ? ROUNDING_VARIANT[rounding] || ROUNDING_VARIANT.medium
      : ROUNDING_VARIANT.medium;

    return (
      <>
        <div
          onClick={onClick}
          ref={ref}
          className={clsx(
            "flex-1 ",
            "focus:outline-none",
            "cursor-pointer h-full max-w-full",
            roundingType == "all"
              ? Rounding_Var.all
              : roundingType === "firstlast"
              ? Rounding_Var.firstandlast
              : "",
            className,
            isSelected && "bg-gray-50",
            !bgColor && "bg-white",
            hoverEffect && "hover:bg-gray-50"
          )}
        >
          {to ? (
            <Link to={to}>
              <ItemContent isSelected={isSelected} {...rest}>
                {children}
              </ItemContent>
            </Link>
          ) : (
            <ItemContent isSelected={isSelected} {...rest}>
              {children}
            </ItemContent>
          )}
        </div>
      </>
    );
  }
);

export default StayledContainerItemWrapper;

//////////////////////////////////
//////////////////////////////////

type ItemContentProps = {
  children: React.ReactNode;
  onItemClick?:
    | ((event: React.MouseEvent<HTMLDivElement, MouseEvent>) => void)
    | undefined;
  onIconClick?:
    | ((event: React.MouseEvent<HTMLDivElement, MouseEvent>) => void)
    | undefined;
  isSelected?: boolean;
  selectedEffect?: boolean;
  selectedIcon?: IconDefinition;
  defaultPadding?: boolean;
  icon?: IconDefinition;
  contextMemu?: any;
  iconStyle?: string;
};

function ItemContent({
  isSelected,
  selectedEffect,
  selectedIcon,
  contextMemu,
  onItemClick,
  onIconClick,
  children,
  defaultPadding = true,
  icon,
  iconStyle,
}: ItemContentProps) {
  const itemref = useRef<HTMLDivElement>(null);
  return (
    <>
      {contextMemu && (
        <ContextMenu parrentref={itemref}>{contextMemu}</ContextMenu>
      )}

      <div
        onClick={onItemClick}
        ref={itemref}
        className={clsx(
          "flex flex-row space-x-2 transition-all duration-200",
          isSelected && selectedEffect && "ml-4",
          icon && "ml-2",
          defaultPadding && "py-2"
        )}
      >
        {(selectedIcon || icon) && (
          <div onClick={onIconClick} className="flex items-center p-1">
            {isSelected || !icon ? (
              <FontAwesomeIcon
                className={clsx(
                  !isSelected && "hidden",
                  "text-xs md:text-md",
                  iconStyle
                )}
                icon={selectedIcon ? selectedIcon : faFilter}
              />
            ) : (
              <FontAwesomeIcon
                className={clsx("text-xs md:text-md", iconStyle)}
                icon={icon}
              />
            )}
          </div>
        )}
        <div className="flex-1 items-center transition-none">{children}</div>
      </div>
    </>
  );
}
