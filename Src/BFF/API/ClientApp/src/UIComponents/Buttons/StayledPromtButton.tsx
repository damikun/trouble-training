import React, { useTransition, useCallback, useState } from "react";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import UserPromt from "../DeletePromt/UserPromt";
import Modal from "../Modal/Modal";
import StayledButton, { SIZE, VARIANTS } from "./StayledButton";

export type StayledPromtButtonProps = {
  children?: React.ReactNode;
  selected?: boolean;
  icon?: IconProp;
  isloading?: boolean;
  loadingPlaceholder?: React.ReactNode;
  variant?: keyof typeof VARIANTS;
  promt_callback: () => any;
  promt_buttonTitle: string;
  promt_isProcessing: boolean;
  promt_title: string;
  promt_content: React.ReactNode;
  promt_className?: string;
  className?: string;
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
  promt_reverse?: boolean;
  promt_variant?:
    | "error"
    | "primarygray"
    | "primaryblue"
    | "secondaryblue"
    | "secondarygray"
    | "ternarygray"
    | "primaryred";
  button_size?: keyof typeof SIZE;
};

export default function StayledPromtButton({
  children,
  selected,
  variant = "primarygray",
  isloading,
  loadingPlaceholder,
  icon,
  promt_callback,
  promt_buttonTitle,
  promt_className,
  promt_content,
  promt_title,
  className,
  promt_isProcessing,
  promt_position,
  promt_minWidth,
  promt_reverse = false,
  promt_variant,
  button_size,
  ...rest
}: StayledPromtButtonProps) {
  const Var = VARIANTS[variant] || VARIANTS.primaryblue;

  const [isPending, startTransition] = useTransition();

  const [userPrompVisible, setuserPrompVisible] = useState(false);

  const HandleConfirm = useCallback(() => {
    setuserPrompVisible(false);
    startTransition(() => {
      promt_callback && promt_callback();
    });
  }, [setuserPrompVisible, startTransition, promt_callback]);

  const handleModalClose = useCallback(() => {
    setuserPrompVisible(false);
  }, [setuserPrompVisible]);

  const handleClick = useCallback(
    (e) => {
      e.preventDefault();
      e.stopPropagation();

      setuserPrompVisible(true);
    },
    [setuserPrompVisible]
  );

  return (
    <>
      <Modal
        className="visible"
        position="top"
        isOpen={userPrompVisible}
        OnClose={handleModalClose}
        OnConfirm={HandleConfirm}
        component={
          <UserPromt
            promt_reverse={promt_reverse}
            promt_position={promt_position}
            promt_variant={promt_variant}
            promt_minWidth={promt_minWidth}
            className={promt_className}
            buttonTitle={promt_buttonTitle}
            isLoading={promt_isProcessing}
            title={promt_title}
            content={promt_content}
          />
        }
      />

      <StayledButton
        onClick={handleClick}
        disabled={userPrompVisible || (isPending as boolean)}
        isloading={isloading}
        variant={"primaryred"}
        iconLeft={icon}
        size={button_size}
        type="button"
      >
        {children}
      </StayledButton>
      {/* <button
        onClick={handleClick}
        disabled={userPrompVisible || (isPending as boolean)}
        {...rest}
        type="button"
        className={clsx(
          `flex-no-wrap 
          bg-transparent 
          px-1 my-auto
          h-full  
          font-semibold
          border
          rounded-md 
          outline-none focus:outline-none 
          transition 
          border-transparent 
          duration-200 `,
          selected ? Var.selected : Var.base,
          className
        )}
      >
        <div className="p-1 text-xs md:text-sm w-full">
          {isloading ? (
            <div className="flex flex-row my-auto mx-auto">
              <Spinner
                size="small"
                className="flex flex-row mx-auto my-auto justify-center align-middle"
              />

              {loadingPlaceholder && (
                <div className="ml-2 flex mx-auto">{loadingPlaceholder}</div>
              )}
            </div>
          ) : icon ? (
            <div className="flex my-auto ">
              <div className="flex mx-auto my-auto">
                <FontAwesomeIcon
                  className={clsx(
                    "flex mx-auto justify-center",
                    children !== undefined && "mr-2"
                  )}
                  icon={icon}
                />
              </div>
              {children && (
                <div className="flex mx-auto my-auto text-center">
                  {children ? children : null}
                </div>
              )}
            </div>
          ) : (
            children
          )}
        </div>
      </button> */}
    </>
  );
}
