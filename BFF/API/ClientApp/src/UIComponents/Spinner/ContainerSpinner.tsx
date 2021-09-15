import clsx from "clsx";
import React from "react";
import StyledSpinner from "../../UIComponents/Spinner/Spinner";
import Container from "../Container/Container";

export type ContainerSpinnerProps = {
  message?: string;
};

export default React.memo(ContainerSpinner);

function ContainerSpinner({ message }: ContainerSpinnerProps) {
  return (
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
  );
}
