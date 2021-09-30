import React, {
  useTransition,
  useCallback,
  useMemo,
} from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import clsx from "clsx";
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import useDebounce from "../../Hooks/useDebounce";

export type StayledButtonPorps = {
  children?: React.ReactNode;
  selected?: boolean;
  iconLeft?: IconProp;
  iconRight?: IconProp;
  isloading?: boolean;
  size?: keyof typeof SIZE;
  variant?: keyof typeof VARIANTS;
  rounding?: keyof typeof ROUNDING;
  iconOnly?: boolean;
  onMobileIconOnly?: boolean;
  transitionTime?: number;
  turncate?: boolean;
} & React.DetailedHTMLProps<
  React.ButtonHTMLAttributes<HTMLButtonElement>,
  HTMLButtonElement
>;

export const ROUNDING = {
  none: "rounded-none",
  small: "rounded-sm ",
  normal: "rounded",
  medium: "rounded-md ",
  large: "rounded-lg ",
  extra: "rounded-xl",
  full: "rounded-full",
};

export const SIZE = {
  auto: "h-full text-xs md:text-sm",
  tiny: "h-5 text-xxs",
  small: "h-6 text-xs md:text-sm",
  normal: "h-8 text-sm md:text-base",
  medium: "h-10 text-md md:text-lg",
  large: "h-12 text-lg md:text-xl",
};

export const VARIANTS = {
  primarygray: {
    base: "text-gray-500 hover:bg-gray-200 hover:text-gray-700 font-bold",
    selected:
      "ring-gray-500 ring-2 text-gray-500 hover:bg-gray-200 hover:text-gray-700",
    tab: "focus:ring-gray-500 focus:ring-2 focus:text-gray-500",
  },

  primaryblue: {
    base: "text-blue-500 hover:bg-gray-200 hover:text-blue-600 font-bold",
    selected:
      "ring-blue-500 ring-2 text-blue-500 hover:bg-gray-200 hover:text-blue-600 font-bold",
    tab: "focus:ring-blue-500 focus:ring-2 focus:text-blue-500",
  },

  primarygreen: {
    base: "text-green-500 hover:bg-green-200 hover:text-green-600 font-bold",
    selected:
      "ring-green-500 ring-2 text-green-500 hover:bg-gray-200 hover:text-green-600 font-bold",
    tab: "focus:ring-green-500 focus:ring-2 focus:text-green-500",
  },

  primaryred: {
    base: "text-red-500 hover:bg-gray-200 hover:text-red-500 font-bold",
    selected:
      "ring-gray-500 ring-2 text-red-500 hover:bg-gray-200 hover:text-red-500 font-bold",
    tab: "focus:ring-red-500 focus:ring-2 focus:text-red-500",
  },

  secondaryblue: {
    base: "bg-blue-500 text-white font-bold hover:bg-blue-600",
    selected:
      "ring-blue-500 ring-2 bg-blue-500 text-white font-bold hover:bg-blue-600",
    tab: "focus:ring-blue-500 focus:ring-2",
  },

  secondarygray: {
    base: "bg-gray-200 text-gray-700 hover:bg-gray-300 font-bold",
    selected:
      "ring-gray-500 ring-2 bg-gray-200 text-gray-700 hover:bg-gray-300 font-bold",
    tab: "focus:ring-gray-300 focus:ring-2",
  },

  secondarylightgray: {
    base: "bg-gray-100 text-gray-700 hover:bg-gray-200 font-bold",
    selected: "bg-gray-400 text-gray-700 font-bold",
    tab: "focus:ring-gray-200 focus:ring-2",
  },

  secondarygreen: {
    base: "bg-green-400 hover:bg-green-500 text-white font-bold",
    selected:
      "ring-green-500 ring-2 bg-green-400 hover:bg-green-500 text-white font-bold",
    tab: "focus:ring-green-500 focus:ring-2",
  },

  secondaryyellow: {
    base: "bg-yellow-400 hover:bg-yellow-500 text-white font-bold",
    selected:
      "ring-yellow-500 ring-2 bg-yellow-400 hover:bg-yellow-500 text-white font-bold",
    tab: "focus:ring-yellow-500 focus:ring-2",
  },

  ternarygray: {
    base: "bg-gray-800 text-white hover:bg-gray-700 font-bold",
    selected:
      "ring-gray-500 ring-2 bg-gray-800 text-white hover:bg-gray-700 font-bold",
    tab: "focus:ring-gray-900 focus:ring-2",
  },

  error: {
    base: "bg-red-500 text-white hover:bg-red-600 font-bold ",
    selected:
      "ring-red-500 ring-2 bg-red-500 hover:bg-red-600 text-white font-bold ",
    tab: "focus:ring-red-600 focus:ring-2",
  },

  errorInverted: {
    base: "bg-white hover:bg-red-500 text-red-500 hover:text-white font-bold ",
    selected:
      "ring-gray-50 ring-2 bg-gray-50 hover:text-red-600 text-white font-bold ",
    tab: "focus:ring-gray-50 focus:ring-2",
  },

  invisible: {
    base: "",
    selected: "",
    tab: "",
  },
};

export default React.memo(StayledButton);

function StayledButton({
  children,
  selected,
  variant = "primarygray",
  isloading,
  rounding = "medium",
  size = "auto",
  iconLeft,
  iconRight,
  disabled,
  onClick,
  turncate = true,
  transitionTime = 500,
  iconOnly = false,
  onMobileIconOnly = true,
  ...rest
}: StayledButtonPorps): JSX.Element {
  const Var = useMemo(() => VARIANTS[variant] || VARIANTS.primaryblue, [
    variant,
  ]);

  const Size = useMemo(() => (size ? SIZE[size] || SIZE.auto : SIZE.auto), [
    size,
  ]);

  const Rounding = useMemo(
    () => (rounding ? ROUNDING[rounding] || ROUNDING.medium : ROUNDING.medium),
    [rounding]
  );

  const [isPending,startTransition] = useTransition();

  const handleClick = useCallback(
    (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
      if (transitionTime === 0) {
        onClick && onClick(event);
      } else {
        startTransition(() => {
          onClick && onClick(event);
        });
      }
    },
    [startTransition, onClick, transitionTime]
  );

  const is_disabled = useMemo(
    () => disabled || (isPending as boolean) || isloading,
    [disabled, isPending, isloading]
  );

  const isloadingDebounced: boolean = useDebounce(isloading, 150);

  return (
    <button
      onClick={handleClick}
      disabled={is_disabled}
      {...rest}
      className={clsx(
        `bg-transparent 
        px-1
        focus:outline-none 
        font-semibold
        border 
        outline-none 
        transition
        border-transparent
        duration-200 focus:delay-75`,
        selected ? Var.selected : Var.base,
        Rounding,
        Var.tab,
        Size,
        rest.className,
        disabled && "cursor-not-allowed"
      )}
    >
      <div className={clsx("p-1 w-full m-auto")}>
        {isloadingDebounced ? (
          <FontAwesomeIcon className="animate-spin" icon={faSpinner} />
        ) : (
          <div className="flex flex-row flex-nowrap space-x-2 leading-none justify-center items-center">
            {iconLeft && <FontAwesomeIcon icon={iconLeft} />}

            {!iconOnly && children && (
              <div
                className={clsx(
                  "text-center items-center",
                  onMobileIconOnly &&
                    (iconLeft !== undefined || iconRight !== undefined) &&
                    "hidden md:flex"
                )}
              >
                <div
                  className={clsx(
                    "text-center",
                    turncate ? "truncate-1-lines break-all" : "truncate"
                  )}
                ></div>
                {children}
              </div>
            )}

            {iconRight && <FontAwesomeIcon icon={iconRight} />}
          </div>
        )}
      </div>
    </button>
  );
}
