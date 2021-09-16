import clsx from "clsx";
import React, { useCallback } from "react";
import ContentContainer from "../Container/ContentContainer";
import ModalFormControl from "../FormControl";

export type UserPromtProps = {
  isLoading: boolean;
  buttonTitle: string;
  title?: string;
  content?: React.ReactNode;
  className?: string;
  onConfirm?: () => void;
  onClose?: () => void;
  promt_position?:
    | "justify-start"
    | "justify-end"
    | "justify-center"
    | "justify-around";
  promt_minWidth?:
    | "min-w-5"
    | "min-w-10"
    | "min-w-16"
    | "min-w-24"
    | "min-w-32"
    | "min-w-40";
  promt_className?: string;
  promt_reverse?: boolean;
  promt_variant?:
    | "error"
    | "primarygray"
    | "primaryblue"
    | "secondaryblue"
    | "secondarygray"
    | "ternarygray"
    | "primaryred";
};

export default function UserPromt({
  title,
  promt_position = "justify-end",
  promt_minWidth,
  promt_className,
  promt_reverse,
  promt_variant,
  onConfirm,
  onClose,
  content,
  buttonTitle,
  className,
}: UserPromtProps) {
  const handleSubmit = useCallback(() => {
    onConfirm && onConfirm();
  }, [onConfirm]);

  const handleClose = useCallback(() => {
    onClose && onClose();
  }, [onClose]);
  return (
    <ContentContainer
      className={className}
      header={
        <div
          className={clsx(
            "flex flex-row font-semibold flex-no-wrap",
            "text-nowrap justify-between align-content-center"
          )}
        >
          <div className="my-auto align-content-center select-none">
            {title}
          </div>
        </div>
      }
    >
      <div className="flex flex-col gap-y-2  w-full">
        <div className="flex py-5 overflow-hidden">
          <div className="flex flex-row flex-wrap font-semibold w-full">
            {content}
          </div>
        </div>

        <ModalFormControl
          onCancle={handleClose}
          onSubmit={handleSubmit}
          variantSubmit={promt_variant}
          buttonTitle={buttonTitle}
          isInFlight={false}
          reverse={promt_reverse}
          position={promt_position}
          className={promt_className}
          minWidth={promt_minWidth}
        />
      </div>
    </ContentContainer>
  );
}
