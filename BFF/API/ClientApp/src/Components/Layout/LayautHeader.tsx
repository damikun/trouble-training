import { AnimatePresence, motion } from "framer-motion";
import LayoutHeaderMd from "./LayoutHeaderContent";

export default function LayoutHeader() {
  return (
    <div className="w-full md:relative ">
      <AnimatePresence>
        {
          <motion.div
            className="z-50"
            initial={{ y: -60 }}
            animate={{ y: 0 }}
            exit={{ y: -60 }}
            transition={{ type: "tween", duration: 0.3, delay: 0 }}
          >
            <LayoutHeaderMd />
          </motion.div>
        }
      </AnimatePresence>
    </div>
  );
}
