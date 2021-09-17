import React, { useCallback, useMemo } from "react";
import clsx from "clsx";
import StayledButton from "./Buttons/StayledButton";
import { useModalContext } from "./Modal/Modal";

type variants =
  | "error"
  | "primarygray"
  | "primaryblue"
  | "secondaryblue"
  | "secondarygray"
  | "secondarylightgray"
  | "ternarygray"
  | "primaryred";
type ModalFormControlProps = {
  isInFlight?: boolean;
  variantSubmit?: variants;
  variantCancle?: variants;
  buttonTitle?: string;
  onSubmit?: () => void;
  onCancle?: () => void;
  ignoreModalContext?: boolean;
  position?:
    | "justify-start"
    | "justify-end"
    | "justify-center"
    | "justify-around";
  positionMobile?:
    | "justify-start"
    | "justify-end"
    | "justify-center"
    | "justify-around";
  minWidth?:
    | "min-w-5"
    | "min-w-10"
    | "min-w-16"
    | "min-w-24"
    | "min-w-32"
    | "min-w-40";
  className?: string;
  reverse?: boolean;
  cancleOnly?: boolean;
  margingTop?: number;
  margingBottom?: number;
};

export default function ModalFormControl({
  isInFlight,
  variantSubmit,
  ignoreModalContext = false,
  variantCancle,
  positionMobile = "justify-end",
  reverse = false,
  margingTop,
  margingBottom,
  className,
  minWidth,
  position = "justify-end",
  buttonTitle,
  onSubmit,
  onCancle,
  cancleOnly,
}: ModalFormControlProps) {
  const modalCtx = useModalContext();

  const handleModalSubmit = useCallback(() => {
    onSubmit && onSubmit();

    if (modalCtx && !ignoreModalContext) modalCtx.confirm();
  }, [modalCtx, onSubmit]);

  const handleModalClose = useCallback(() => {
    onCancle && onCancle();
    if (modalCtx && !ignoreModalContext) modalCtx.close();
  }, [modalCtx, onCancle]);

  const buttonText = useMemo(() => {
    return buttonTitle ? buttonTitle : "Confirm";
  }, [buttonTitle]);

  const submitVariant = useMemo(
    () => (variantSubmit ? variantSubmit : "secondaryblue"),
    [variantSubmit]
  );

  const closeVariant = useMemo(
    () => (variantCancle ? variantCancle : "secondarygray"),
    [variantCancle]
  );

  const justify_pos = useMemo(() => {
    return `sm:${position} ${positionMobile}`;
  }, [position]);

  return (
    <div
      style={{ marginTop: margingTop, marginBottom: margingBottom }}
      className={clsx(
        reverse === true ? "flex-row-reverse space-x-reverse" : "flex-row",
        "flex h-8 space-x-3 px-1",
        !margingTop && "mt-5",
        !margingBottom && "mb-1",
        justify_pos,
        className
      )}
    >
      {!cancleOnly && (
        <StayledButton
          isloading={isInFlight}
          variant={submitVariant}
          className={clsx("flex my-auto", minWidth)}
          type="submit"
          onClick={handleModalSubmit}
        >
          {buttonText}
        </StayledButton>
      )}

      <StayledButton
        transitionTime={0}
        variant={closeVariant}
        className={clsx("flex my-auto", minWidth)}
        type="reset"
        onClick={handleModalClose}
      >
        Cancle
      </StayledButton>
    </div>
  );
}
