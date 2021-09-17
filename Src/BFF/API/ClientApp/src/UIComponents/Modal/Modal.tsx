import clsx from "clsx";
import { AnimatePresence, motion, Variants } from "framer-motion";
import React, { Suspense, useCallback, useContext, useRef } from "react";
import useOnClickOutside from "../../Hooks/useOnOutsideElementClick";
import ModalBounderyErrorHandler from "../../Components/Errors/ModalBounderyErrorHandler";
import ErrorBoundary from "../ErrorBoundery/ErrorBoundary";

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
                    fallback={errorfallback ? errorfallback : <ModalBounderyErrorHandler />}
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
