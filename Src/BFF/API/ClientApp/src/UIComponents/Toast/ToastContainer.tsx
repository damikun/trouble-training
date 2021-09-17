/* Author: Dalibor Kundrat  https://github.com/damikun */

import clsx from "clsx";
import { motion } from "framer-motion";
import React, { useContext, useEffect, useMemo, useState } from "react";
import ToastMessage from "./ToastMessage";
import { ToastContext } from "./ToastProvider";

export type ToastContainerProps = {
  variant?: keyof typeof VARIANTS;
};

const VARIANTS = {
  top_left: {
    style: "top-0 left-0",
  },
  top_right: {
    style: "top-0 right-0",
  },
  bottom_right: {
    style: "bottom-0 right-0",
  },
  bottom_left: {
    style: "bottom-0 left-0",
  },
  top_middle: {
    style: "top-0 left-1/2 -translate-x-1/2 transform",
  },
  bottom_middle: {
    style: "bottom-0 left-1/2 -translate-x-1/2 transform",
  },
  undefined: {
    style: "top-0 right-0",
  },
};

const variants = {
  open: {
    transition: { staggerChildren: 0.3, delayChildren: 0 },
  },
  closed: {
    transition: {
      staggerChildren: 0.05,
      staggerDirection: -1,
    },
  },
};

const variants_li = {
  open: {
    opacity: 1,
    x: 0,
  },
  closed: {
    opacity: 0,
    x: 150,
  },
};

export default React.memo(ToastContainer);

function ToastContainer({ variant = "top_right" }: ToastContainerProps) {
  const context = useContext(ToastContext);

  const Var = useMemo(() => VARIANTS[variant] || VARIANTS.top_right, [
    VARIANTS,
    variant,
  ]);

  function handleRemove(id: string) {
    context?.remove(id);
  }

  const [isOpen, setisOpen] = useState(false);

  useEffect(() => {
    context?.data.length && setisOpen(context?.data.length > 0);
  }, [context?.data.length]);

  return (
    <div
      className={clsx(
        Var.style,
        "fixed z-50 w-full md:max-w-sm pointer-events-none",
        "p-4 md:p-4 max-h-screen overflow-hidde"
      )}
    >
      <motion.div
        variants={variants}
        animate={isOpen ? "open" : "closed"}
        initial="closed"
        className={clsx("flex-1 flex-col fade w-full mr-8 justify-end")}
      >
        {context?.data.map((toast) => {
          return (
            <motion.li
              key={toast.id}
              variants={variants_li}
              exit={{ opacity: 0 }}
              className="flex py-1 w-full pointer-events-auto"
            >
              <ToastMessage
                id={toast.id}
                message={toast.message}
                type={toast.type}
                header={toast.header}
                icon={toast.icon}
                truncate={toast.truncate}
                onRemove={handleRemove}
                lifetime={toast.lifetime}
              />
            </motion.li>
          );
        })}
      </motion.div>
    </div>
  );
}
