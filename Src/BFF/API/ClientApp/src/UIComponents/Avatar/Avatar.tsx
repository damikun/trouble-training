import React, { Suspense, useMemo } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import clsx from "clsx";
import Badge from "../Badged/Badge";
import { SuspenseImg } from "../../Utils/SuspenseImage";
import ErrorBoundary from "../ErrorBoundery/ErrorBoundary";

export type AvatarProps = {
  size?: keyof typeof SIZE_VARIANTS;
  src?: string;
  label?: string;
  showStatus?: boolean;
  info?: string;
  infoVariant?: keyof typeof INFO_VARIANTS;
  status?: boolean;
  variant?: keyof typeof VARIANTS;
  icon?: IconProp;
  hovereffect?: boolean;
  hideImage?: boolean;
  border?: boolean;
  className?: string;
};

const INFO_VARIANTS = {
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
};

const SIZE_VARIANTS = {
  "w-5 h-5": {
    sizeAvatar: "w-5 h-5",
    sizeStatus: "w-2 h-2",
    roundedstatusPositionTop: "-bottom-0.5 -right-1",
    roundedstatusPositionBottom: "-bottom-1 -right-1",
    squarestatusPositionTop: "-top-1 -right-1",
    squarestatusPositionBottom: "-bottom-1 -right-1",
    fontSize: "text-xxxs",
    infostyle: "hidden",
    infosize: "thin",
    sizepx: 20,
  },
  "w-6 h-6": {
    sizeAvatar: "w-6 h-6",
    sizeStatus: "w-2 h-2 ",
    roundedstatusPositionTop: "-bottom-0.5 -right-1",
    roundedstatusPositionBottom: "-bottom-1 -right-1",
    squarestatusPositionTop: "-top-1 -right-1",
    squarestatusPositionBottom: "-bottom-1 -right-1",
    fontSize: "text-xxs",
    infostyle: "hidden",
    infosize: "thin",
    sizepx: 24,
  },
  "w-8 h-8": {
    sizeAvatar: "w-8 h-8",
    sizeStatus: "w-2.5 h-2.5 ",
    roundedstatusPositionTop: "-bottom-0.5 -right-1",
    roundedstatusPositionBottom: "-bottom-1 -right-1",
    squarestatusPositionTop: "-top-1 -right-1",
    squarestatusPositionBottom: "-bottom-1 -right-1",
    fontSize: "text-xs",
    infostyle: "hidden",
    infosize: "thin",
    sizepx: 32,
  },
  "w-10 h-10": {
    sizeAvatar: "w-10 h-10",
    sizeStatus: "w-3 h-3 -bottom-1 -right-1",
    roundedstatusPositionTop: "-bottom-0.5 -right-1",
    roundedstatusPositionBottom: "bottom-0.5 -right-",
    squarestatusPositionTop: "-top-1.5 -right-1.5",
    squarestatusPositionBottom: "-bottom-1.5 -right-1.5",
    fontSize: "text-sm",
    infostyle: "hidden",
    infosize: "thin",
    sizepx: 40,
  },
  "w-12 h-12": {
    sizeAvatar: "w-12 h-12",
    sizeStatus: "w-3.5 h-3.5 ",
    roundedstatusPositionTop: "-top-0 -right-1",
    roundedstatusPositionBottom: "-bottom-0.5 -right-1",
    squarestatusPositionTop: "-top-1.5 -right-1.5",
    squarestatusPositionBottom: "-bottom-1.5 -right-1.5",
    fontSize: "text-md",
    infostyle: "flex",
    infosize: "thin",
    sizepx: 48,
  },
  "w-16 h-16": {
    sizeAvatar: "w-16 h-16",
    sizeStatus: "w-4 h-4 ",
    roundedstatusPositionTop: "-top-0 -right-0.5",
    roundedstatusPositionBottom: "-bottom-0.5 -right-0.5",
    squarestatusPositionTop: "-top-1.5 -right-1.5",
    squarestatusPositionBottom: "-bottom-1.5 -right-1.5",
    fontSize: "text-md",
    infostyle: "flex",
    infosize: "small",
    sizepx: 64,
  },
  "w-20 h-20": {
    sizeAvatar: "w-20 h-20",
    sizeStatus: "w-4 h-4 ",
    roundedstatusPositionTop: "top-0.5 right-0",
    roundedstatusPositionBottom: "bottom-0.5 right-0",
    squarestatusPositionTop: "-top-2 -right-2",
    squarestatusPositionBottom: "-bottom-2 -right-2",
    fontSize: "text-xl",
    infostyle: "flex",
    infosize: "small",
    sizepx: 80,
  },
  "w-32 h-32": {
    sizeAvatar: "w-32 h-32",
    sizeStatus: "w-5 h-5",
    roundedstatusPositionTop: "top-2 right-1",
    roundedstatusPositionBottom: "bottom-2 right-1",
    squarestatusPositionTop: "-top-2 -right-2",
    squarestatusPositionBottom: "-bottom-2 -right-2",
    fontSize: "text-3xl",
    infostyle: "flex",
    infosize: "normal",
    sizepx: 128,
  },
  "w-40 h-40": {
    sizeAvatar: "w-40 h-40",
    sizeStatus: "w-5 h-5",
    roundedstatusPositionTop: "top-4 right-1.5",
    roundedstatusPositionBottom: "bottom-4 right-1.5",
    squarestatusPositionTop: "-top-2 -right-2",
    squarestatusPositionBottom: "-bottom-2 -right-2",
    fontSize: "text-3xl",
    infostyle: "flex",
    infosize: "normal",
    sizepx: 160,
  },
  "w-48 h-48": {
    sizeAvatar: "w-48 h-48",
    sizeStatus: "w-5 h-5",
    roundedstatusPositionTop: "top-6 right-2",
    roundedstatusPositionBottom: "bottom-6 right-2",
    squarestatusPositionTop: "-top-2 -right-2",
    squarestatusPositionBottom: "-bottom-2 -right-2",
    fontSize: "text-3xl",
    infostyle: "flex",
    infosize: "normal",
    sizepx: 192,
  },
};

const VARIANTS = {
  rounded: {
    style: "rounded-full",
  },
  square: {
    style: "rounded-md",
  },
};

const hoverbehaviour =
  "transition duration-300 ease-in-out transform hover:-translate-y-0.5";

export default React.memo(Avatar);

function Avatar({
  size = "w-12 h-12",
  src,
  label,
  status = false,
  showStatus = false,
  variant = "rounded",
  icon,
  border = true,
  hovereffect = false,
  className,
  info,
  hideImage = false,
  infoVariant,
}: AvatarProps) {
  const Var = VARIANTS[variant] || VARIANTS.rounded;

  const Size = SIZE_VARIANTS[size];

  const memorisedLabel = useMemo(() => {
    if (label) {
      if (label.length >= 2) {
        return `${label[0]}${label[1]}`;
      } else if (label.length === 1) {
        return `${label[0]}`;
      }
    }
    return undefined;
  }, [label]);

  const MemorisedFallback = useMemo(
    () => (
      <AvatarFallback
        style={Var.style}
        fontSize={Size.fontSize}
        icon={icon}
        label={memorisedLabel}
      />
    ),
    [memorisedLabel, icon, Var.style, Size.fontSize]
  );

  return (
    <div
      className={clsx(
        "flex shadow-xs select-none flex-no-wrap text-lg uppercase",
        "font-semibold border-white leading-none relative",
        "my-auto bg-gray-50 items-center justify-center cursor-pointer",
        "shadow-sm",
        Size.sizeAvatar,
        Var.style,
        Size.fontSize,
        border && "border-2 ring-1",
        className ? className : "text-gray-800",
        status && showStatus ? "ring-blue-400" : "ring-gray-300"
      )}
    >
      {showStatus && (
        <span
          className={clsx(
            "absolute inline-block",
            "border ring-1 ring-white",
            "border-white rounded-full",
            Size.sizeStatus,
            status ? "bg-green-500" : "bg-gray-300",
            info && info.length > 1
              ? variant === "rounded"
                ? Size.roundedstatusPositionTop
                : Size.squarestatusPositionTop
              : variant === "rounded"
              ? Size.roundedstatusPositionBottom
              : Size.squarestatusPositionBottom
          )}
        />
      )}

      {src ? (
        <ErrorBoundary fallback={<>{MemorisedFallback}</>}>
          <Suspense fallback={<>{MemorisedFallback}</>}>
            <div className="flex w-full h-full">
              {src !== undefined && src !== null && !hideImage && (
                <SuspenseImg
                  className={clsx(
                    "w-full h-full overflow-hidden",
                    Var.style
                    // hovereffect && hoverbehaviour
                  )}
                  height={Size.sizepx}
                  width={Size.sizepx}
                  src={src ? src : undefined}
                />
              )}
            </div>
          </Suspense>
        </ErrorBoundary>
      ) : (
        <>{MemorisedFallback}</>
      )}

      {info && info.length > 1 && (
        <span
          className={clsx(
            "absolute inline-block -bottom-3 capitalize",
            Size.infostyle
          )}
        >
          <Badge
            rounding="full"
            variant={infoVariant}
            //@ts-ignore
            size={Size.infosize!}
            turncate={false}
          >
            {info}
          </Badge>
        </span>
      )}
    </div>
  );
}

///////////////////////////////////////
///////////////////////////////////////

type AvatarFallbackProps = {
  style: string;
  fontSize: string;
  icon?: IconProp;
  label?: string;
};

function AvatarFallback({ style, fontSize, icon, label }: AvatarFallbackProps) {
  return (
    <div className={clsx(style, fontSize)}>
      {icon ? (
        <FontAwesomeIcon
          className={" mx-auto justify-center block"}
          icon={icon}
        />
      ) : (
        <div className="leading-none ">{label}</div>
      )}
    </div>
  );
}
