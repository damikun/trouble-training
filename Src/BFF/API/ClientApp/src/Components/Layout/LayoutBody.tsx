import clsx from "clsx";
import { motion } from "framer-motion";
import React from "react";

type Props = {
  children?: React.ReactNode;
};
export default React.memo(LayoutBody);

function LayoutBody({ children }: Props) {
  return (
    <motion.div
      className={clsx("flex-1 h-full max-h-full")}
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      transition={{ type: "tween", duration: 0.3, delay: 0.2 }}
    >
      {children}
    </motion.div>
  );
}
