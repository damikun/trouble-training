import clsx from "clsx";
import React, { Suspense } from "react";
import ErrorBoundary from "../ErrorBoundery/ErrorBoundary";
import RouterTabList, { RouterTabItemType } from "../RouterTab/RouterTabList";
import ContainerSpinner from "../Spinner/ContainerSpinner";

export type ViewBodyContainerProps = {
  children?: React.ReactNode;
  breadcrumbs?: boolean;
  header?: React.ReactNode;
  name?: string;
  tabs?: RouterTabItemType[];
  handleSuspense?: boolean;
  fallback?: React.ReactElement;
};

export default function ViewBodyContainer({
  children,
  breadcrumbs,
  header,
  handleSuspense = true,
  name,
  tabs,
  fallback,
}: ViewBodyContainerProps) {
  return (
    <div
      className={clsx(
        "flex flex-col h-full w-full",
        "text-xs md:text-sm max-h-full space-y-1"
      )}
    >
      {(header || tabs) && (
        <div
          className={clsx(
            "flex flex-col bg-gradient-to-b from-gray-100 via-gray-50",
            "to-gray-50  w-full max-h-full space-y-1 xl:px-15",
            "pt-2 shadow-sm px-3 sm:px-5 lg:px-10"
          )}
        >
          {/* {breadcrumbs && (
          <Breadcrumb disableResolutionDetail name={name}></Breadcrumb>
        )} */}

          <div className="flex-1">{header}</div>

          {tabs && (
            <div
              className={clsx(
                "flex w-full flex-row flex-wrap-reverse",
                "justify-center sm:justify-between my-auto",
                "overflow-x-scroll scrollbarwidth scrollbarhide",
                "scrollbarhide2 scrolling-touch items-end"
              )}
            >
              <div
                className={clsx(
                  "flex flex-wrap w-full",
                  "md:w-auto truncate justify-center"
                )}
              >
                <RouterTabList defaultIndex={0} Tabs={tabs} />
              </div>
            </div>
          )}
        </div>
      )}

      <div
        className={clsx(
          "flex-1 flex-no-wrap h-full max-h-full lg:py-3",
          "xl:px-15 xl:py-3 px-3 sm:px-5 py-2 lg:px-10"
        )}
      >
        <div
          className={clsx(
            "flex-1 flex-no-wrap rounded-xs",
            "flex-shrink h-full max-h-full"
          )}
        >
          {handleSuspense ? (
            <ErrorBoundary fallback={<div></div>}>
              <Suspense fallback={fallback ? fallback : <ContainerSpinner />}>
                {children}
              </Suspense>
            </ErrorBoundary>
          ) : (
            children
          )}
        </div>
      </div>
    </div>
  );
}
