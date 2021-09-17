import clsx from "clsx";
import React, {
  createRef,
  useMemo,
  SuspenseList as SuspenseList,
} from "react";
import IsLoading from "../IsLoading";
import NoRecords from "../NoRecords";
import useDivInfinityScroll from "../../Hooks/useDivInfinityScroll";
import Container from "../Container/Container";
import LoadingBar from "../LoadingBar/LoadingBar";

export type ScrollEndProps = {
  className?: string;
  children: React.ReactNode;
  onEnd?: () => void;
  offset?: number;
  isLoading?: boolean;
  isLoadingMore?: boolean;
  bgcolor?: string;
  padding?: boolean;
  divide?: boolean;
  header?: React.ReactNode;
  isEmpty?: boolean;
  bootomFade?: boolean;
  isEmptyVisibility?: "always" | "medium" | "large" | "extralarge";
  isEmptyMessage?: string;
  isEmptySize?: "small" | "big" | "medium" | "extra" | undefined;
};

export default function StayledInfinityScrollContainer({
  children,
  className,
  onEnd,
  offset = 20,
  isLoadingMore = undefined,
  isLoading = undefined,
  isEmptySize,
  bgcolor,
  padding = true,
  header,
  isEmptyMessage,
  isEmpty,
  isEmptyVisibility = "always",
  bootomFade,
  divide = false,
}: ScrollEndProps) {
  const reference = createRef<HTMLDivElement>();

  useDivInfinityScroll({
    ref: reference,
    handleOnEnd: () => {
      onEnd && onEnd();
    },
    fromEnd_px: offset,
  });

  return (
    <div
      className={clsx(
        "flex flex-col max-h-full",
        "h-full w-full max-w-full ",
        bgcolor ? bgcolor : "bg-gray-100"
      )}
    >
      {header}
      {isLoading !== undefined && <LoadingBar isloading={isLoading} />}

      <div
        ref={reference}
        className={clsx(
          "flex-1 rounded-b-md max-h-full overflow-hidden",
          "relative min-h-9rem max-w-full overflow-y-scroll",
          "scrollbarwidth scrollbarhide2 scrollbarhide",
          className,
          bootomFade && "top-bottom-overflow-fade"
        )}
      >
        {isEmpty ? (
          <div className="flex w-full h-full justify-center items-center ">
            <NoRecords
              visibility={isEmptyVisibility}
              size={isEmptySize}
              message={isEmptyMessage}
            />
          </div>
        ) : (
          <div className="absolute w-full align-middle">
            <Container
              flextype="flex"
              flexwrap="flex-no-wrap"
              className={clsx(
                "flex-col min-h-9rem max-w-full",
                padding && "p-0 md:p-1 max-w-full"
              )}
            >
              <SuspenseList revealOrder="together">
                <div className={clsx(divide && "divide-y")}>
                  <div className={clsx(divide && "divide-y")}>{children}</div>
                </div>
              </SuspenseList>

              {isLoadingMore != undefined && (
                <IsLoading isloading={isLoadingMore} />
              )}
            </Container>
          </div>
        )}
      </div>
    </div>
  );
}
