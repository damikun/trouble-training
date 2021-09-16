import React from "react";
import { faWindowClose } from "@fortawesome/free-solid-svg-icons";
import StayledButton from "../../UIComponents/Buttons/StayledButton";
import clsx from "clsx";
import Container from "../../UIComponents/Container/Container";
import { useModalContext } from "../../UIComponents/Modal/Modal";
import { ReactComponent as NotFound } from "../../Images/isEmpty.svg";

export type ContentContainerProps = {
  children: React.ReactNode;
  header?: React.ReactNode;
  toolbar?: React.ReactNode;
  className?: string;
  id?: string;
  notFound?: boolean;
};

export default function ContentContainer({
  children,
  header,
  id,
  toolbar,
  className,
  notFound,
}: ContentContainerProps) {
  const modalContext = useModalContext();

  return (
    <div
      id={id ? id : "__ModalContainer"}
      className={clsx(
        "flex w-full text-xs md:text-sm",
        "select-text my-auto rounded-md shadow-xl",
        "flex-grow bg-white items-center",
        className ? className : "flex w-auto md:w-md lg:w-lg xl:w-xl"
      )}
    >
      <div className="flex flex-col w-full max-w-full h-full">
        <div
          className={clsx(
            "flexborder-gray-300 flex-row",
            "font-semibold flex-no-wrap text-nowrap",
            "align-content-center justify-between"
          )}
        >
          <div
            className={clsx(
              "flex flex-row justify-between w-full px-5",
              "py-2 pt-3 shadow-sm border-b border-gray-300",
              "bg-gray-200 rounded-t-md"
            )}
          >
            <div
              className={clsx(
                "my-auto align-content-center",
                "select-none items-center"
              )}
            >
              {header}
            </div>
            <div className="flex flex-row space-x-2 justify-end">
              {toolbar}
              <StayledButton
                transitionTime={0}
                className="flex my-auto  text-white"
                variant="secondarygray"
                onClick={modalContext.close}
                selected={false}
                type="button"
                iconLeft={faWindowClose}
              />
            </div>
          </div>
        </div>
        <div className="flex-1 flex-grow mt-2 mx-3 md:mx-5 pb-3">
          <div className="flex-1 flex-col w-full max-w-full h-full">
            {notFound ? <NotFoundSection /> : children}
          </div>
        </div>
      </div>
    </div>
  );
}

function NotFoundSection() {
  return (
    <Container
      flextype="flex-1"
      flexwrap="flex-no-wrap"
      className="h-full w-full max-h-full max-w-full items-center"
    >
      <div
        className={clsx(
          "flex flex-col relative w-full max-w-screen-lg mx-auto py-5"
        )}
      >
        <NotFound className="h-32" />
        <div
          className={clsx(
            "flex px-2 flex-col break-words bottom-0",
            "whitespace-normal text-center py-2 space-y-2 prose-md"
          )}
        >
          <h3
            className={clsx(
              "pt-2 text-lg md:text-2xl text-center text-gray-400",
              "break-words leading-tight capitalize font-semibold"
            )}
          >
            Not found
          </h3>

          <div className="font-semibold text-sm md:text-base break-words">
            <p className="break-normal text-gray-500">
              The item you`re looking for was not found.
            </p>
          </div>
        </div>
      </div>
    </Container>
  );
}
