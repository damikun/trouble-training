import clsx from "clsx";
//@ts-ignore
import React, { Suspense } from "react";
import ErrorBoundary from "../ErrorBoundery/ErrorBoundary";
//@ts-ignore
import ContainerSpinner from "../Spinner/ContainerSpinner";

export type ViewContainerProps = {
  filters?: React.ReactNode;
  content: React.ReactNode;
  header?: React.ReactNode;
  errorfallback?: React.ReactNode;
  fallback?: React.ReactElement;
  className?: String;
  bgcolor?: string;
  fixedHeight?: boolean;
  headerBgColor?: string;
  border?: boolean;
  padding?: boolean;
  rounding?: boolean;
  shadow?: boolean;
  mobileDoubleLineFilters?: boolean;
};

export default function ViewContainer({
  filters,
  content,
  header,
  errorfallback,
  fallback,
  className,
  bgcolor,
  rounding = true,
  fixedHeight = true,
  border = true,
  headerBgColor,
  padding = true,
  shadow = true,
  mobileDoubleLineFilters,
}: ViewContainerProps) {
  return (
    <div
      className={clsx(
        "flex flex-nowrap flex-col float-left",
        "h-full max-h-full w-full max-w-full space-y-1",
        className
      )}
    >
      {filters && (
        <div
          className={clsx(
            "flex",
            fixedHeight
              ? mobileDoubleLineFilters
                ? " h-14 md:h-8"
                : " h-8"
              : ""
          )}
        >
          {filters}
        </div>
      )}

      <div
        className={clsx(
          "items-center justify-center",
          "flex-1 align-top overflow-hidden",
          "mb-auto max-h-full",
          bgcolor ? bgcolor : "bg-white",
          border && " border-opacity-80 border border-gray-200",
          shadow && "shadow-sm",
          rounding && "rounded-md"
        )}
      >
        <div
          className={clsx("flex-1 h-full w-full max-h-full", padding && "p-1")}
        >
          <div
            className={clsx(
              "flex flex-col max-h-full",
              "h-full w-full max-w-full"
            )}
          >
            {header && (
              <div
                className={clsx(
                  headerBgColor ? headerBgColor : "",
                  "border-b",
                  !padding && "pt-1"
                )}
              >
                {header}
              </div>
            )}

            <ErrorBoundary fallback={errorfallback ? errorfallback : <></>}>
              {/* <Suspense fallback={fallback ? fallback : <ContainerSpinner />}> */}
              {content}
              {/* </Suspense> */}
            </ErrorBoundary>
          </div>
        </div>
      </div>
    </div>
  );
}
