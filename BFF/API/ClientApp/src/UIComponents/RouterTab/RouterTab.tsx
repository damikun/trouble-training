import clsx from "clsx";
import React from "react";
import { useCallback, useEffect, useState } from "react";
import { useMatch, useNavigate, useResolvedPath } from "react-router";
import { Link } from "../Link/Link";
import { useRouterTabContext } from "./RouterTabList";

export type RouterTabProps = {
  hoverEffect?: boolean;
  to: string;
  pattern?: string;
  name: string;
  width?: string;
  tabStyle?: string;
  fillOnActiv?: boolean;
  flexVariant?: keyof typeof FLEX_VARIANT;
};

const FLEX_VARIANT = {
  row: {
    item_style: "border-b-2 p-2 text-center justify-center ",
    rounding: "last:rounded-tr-md first:rounded-tl-md",
  },
  col: {
    item_style: "border-l-2 p-3 pl-5",
    rounding: "last:rounded-b-md first:rounded-t-md",
  },
  row_md_col: {
    item_style:
      "border-b-2 md:border-l-2 md:border-b-0 p-3 md:pl-5 text-center md:text-left justify-center md:justify-start",
    rounding:
      "last:rounded-tr-md first:rounded-tl-md md:last:rounded-tr-none md:first:rounded-tl-none md:last:rounded-b-md md:first:rounded-t-md",
  },
};

export default React.memo(RouterTab);

function RouterTab({
  to,
  name,
  pattern,
  hoverEffect,
  tabStyle,
  width,
  flexVariant = "row",
  fillOnActiv = false,
}: RouterTabProps) {
  const navigate = useNavigate();

  const tabCtx = useRouterTabContext();

  const resolver = useResolvedPath(pattern ? pattern : to);

  const match = useMatch({ path: resolver.pathname });

  useEffect(() => {
    if (match) {
      tabCtx.setNavigating(undefined);
    }
  }, [match, tabCtx.setNavigating]);

  const [state, setstate] = useState(false);

  const handleResetState = useCallback(
    (state: boolean) => {
      if (!state) {
        setstate(false);
      }
    },
    [setstate]
  );

  function HandleClick() {
    setstate(true);
    tabCtx.setNavigating(to);
  }

  const flex_Var = flexVariant ? FLEX_VARIANT[flexVariant] : FLEX_VARIANT.row;

  return (
    <Link
      className={clsx(
        hoverEffect && "hover:bg-gray-50",
        flex_Var.rounding,
        tabStyle
      )}
      onClick={HandleClick}
      onTransitionStateChange={handleResetState}
      to={to}
    >
      <li
        tabIndex={0}
        className={clsx(
          "flex cursor-pointer text-gray-700",
          "flex-no-wrap font-normal hover:text-gray-700",
          "select-none leading-none h-full items-center",
          "transition duration-200 focus:outline-none outline-none",
          width ? width : "min-w-24 w-24",
          state && "animate-pulse",
          match !== null || (match === null && state)
            ? clsx("border-blue-500 font-bold px-2", flex_Var.item_style)
            : clsx(
                "border-transparent hover:border-gray-300 focus:border-gray-300",
                flex_Var.item_style
              ),
          match && fillOnActiv && "bg-blue-500 text-white"
        )}
      >
        {name}
      </li>
    </Link>
  );
}
