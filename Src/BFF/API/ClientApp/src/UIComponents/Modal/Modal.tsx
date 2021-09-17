import { faSyncAlt } from "@fortawesome/free-solid-svg-icons";
import clsx from "clsx";
import { AnimatePresence, motion, Variants } from "framer-motion";
import React, { Suspense, useCallback, useContext, useRef } from "react";
import ContentContainer from "../Container/ContentContainer";
import useOnClickOutside from "../../Hooks/useOnOutsideElementClick";
import StayledButton from "../Buttons/StayledButton";
import Container from "../Container/Container";
import ErrorBoundary, {
  useBoundaryContext,
} from "../ErrorBoundery/ErrorBoundary";
import { ReactComponent as ServerDown } from "../../Images/serverDown.svg";

export type ModalProps = {
  children?: React.ReactNode;
  isOpen: boolean;
  position: "center" | "top" | "fullscreen";
  OnClose: () => void;
  OnConfirm?: () => void;
  fallback?: React.ReactElement;
  errorfallback?: React.ReactNode;
  className?: string;
  component?: React.ReactNode;
};

const modalVariant: Variants = {
  initial: {
    opacity: 0,
    transition: {
      duration: 0.1,
    },
  },
  isOpen: {
    opacity: 1,
    transition: {
      duration: 0.1,
    },
  },
  exit: {
    opacity: 0,
    transition: {
      duration: 0.1,
    },
  },
};

const containerVariant = {
  initial: {
    top: "-100%",
  },

  isOpen: {
    top: "0%",
    transition: {
      mass: 0.4,
      type: "spring",
    },
  },
  exit: {
    top: "0%",
    transition: {
      duration: 0.1,
      mass: 0.4,
      type: "spring",
    },
  },
};

export type ModalContextType = {
  close: () => void;
  confirm: () => void;
};

export const ModalContext = React.createContext<ModalContextType>({
  close: () => {},
  confirm: () => {},
});

export const useModalContext = () => useContext(ModalContext);

export default function Modal({
  children,
  isOpen,
  OnClose,
  OnConfirm,
  position = "top",
  fallback,
  errorfallback,
  className,
  component,
}: ModalProps) {
  const $modalref = useRef<HTMLDivElement>(null);

  useOnClickOutside($modalref, isOpen ? OnClose : () => {});

  const ModalContexd = useCallback(() => {
    return {
      close() {
        OnClose();
      },
      confirm() {
        OnConfirm && OnConfirm();
      },
    };
  }, [OnClose, OnConfirm]);

  return (
    <ModalContext.Provider value={ModalContexd()}>
      <AnimatePresence>
        {isOpen && (
          <motion.div
            key="modal"
            initial={"initial"}
            animate={"isOpen"}
            exit={"exit"}
            variants={modalVariant}
            className={clsx(
              "flex-1 z-40 opacity-100 fixed modal-background",
              "top-0 left-0 h-full w-full overflow-x-hidden",
              "overflow-y-scroll scrollbarwidth scrollbarhide scrollbarhide2",
              "scrolling-touch h-full",
              className
            )}
          >
            <motion.div
              variants={containerVariant}
              className="flex select-none relative h-full w-full"
            >
              <div
                className={clsx(
                  "flex mx-auto w-full justify-center items-center",
                  "px-5 max-w-full",
                  position === "center" && "my-auto",
                  position === "fullscreen" && "h-full pt-10 pb-10",
                  position === "top" && "mb-auto pt-16 pb-10"
                )}
              >
                <div
                  className="w-full md:w-auto h-full"
                  key={"modalcontent"}
                  ref={$modalref}
                >
                  <ErrorBoundary
                    fallback={errorfallback ? errorfallback : <ModalError />}
                  >
                    <Suspense fallback={fallback ? fallback : <></>}>
                      {children ? children : component}
                    </Suspense>
                  </ErrorBoundary>
                </div>
              </div>
            </motion.div>
          </motion.div>
        )}
      </AnimatePresence>
    </ModalContext.Provider>
  );
}

function ModalError() {
  const context = useBoundaryContext();

  return (
    <ContentContainer className={"flex"}>
      <Container
        flextype="flex-1"
        className="h-full w-full max-h-full max-w-full items-center"
      >
        <div
          className={clsx(
            "flex flex-col relative w-full",
            "mx-auto py-5 px-5 justify-center"
          )}
        >
          <ServerDown className="h-32" />

          <div
            className={clsx(
              "flex px-2 flex-col break-words bottom-0 mt-5",
              "whitespace-normal text-center py-2 space-y-2 prose-md"
            )}
          >
            <h3
              className={clsx(
                "pt-2 text-lg md:text-xl text-center text-gray-400",
                "break-words leading-tight capitalize font-semibold"
              )}
            >
              Failed to fetch
            </h3>

            <div
              className={clsx(
                "flex font-semibold text-sm md:text-base",
                "break-words pt-3 justify-center"
              )}
            >
              <StayledButton
                className={clsx(
                  "flex my-auto text-lg text-white",
                  "w-full md:w-32 py-1"
                )}
                variant="secondarygray"
                onClick={context.reset}
                name="Retry"
                selected={false}
                iconLeft={faSyncAlt}
              >
                Retry
              </StayledButton>
            </div>
          </div>
        </div>
      </Container>
    </ContentContainer>
  );
}
