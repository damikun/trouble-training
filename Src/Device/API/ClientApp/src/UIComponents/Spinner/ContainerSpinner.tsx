import clsx from "clsx";
import React from "react";
import StyledSpinner from "../../UIComponents/Spinner/Spinner";
import Container from "../Container/Container";
import { motion } from "framer-motion";

export type ContainerSpinnerProps = {
  message?: string;
  render_animation_delay?:number 
};

export default React.memo(ContainerSpinner);

function ContainerSpinner({ message, render_animation_delay=0.5 }: ContainerSpinnerProps) {
  return (
    <motion.div
    className={clsx("flex-1 h-full max-h-full")}
    initial={{ opacity: 0 }}
    animate={{ opacity: 1 }}
    transition={{ type: "tween", duration: 0.3, delay: render_animation_delay}}
    >
      <Container
        flextype="flex"
        className={clsx(
          "flex relative w-full md:w-auto h-full",
          "max-h-full overflow-hidden items-center",
          "bg-transparent w-full"
        )}
      >
        <div className=" w-full items-center text-blue-500">
          <div className="flex items-center">
            <StyledSpinner
              flex="col"
              label={message ? message : "Loading..."}
              size="extralarge"
            />
          </div>
        </div>
      </Container>
    </motion.div>
  );
}
