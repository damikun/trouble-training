import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import clsx from "clsx";

export type ContextMenuProps = {
  name: string;
  icon?: IconProp;
  onClick?: () => void;
};

export default function contextMenuItem({
  name,
  icon,
  onClick,
}: ContextMenuProps) {
  return (
    <div
      onClick={(event) => {
        event.preventDefault();
        onClick && onClick();
      }}
      className="flex flex-row p-1 hover:bg-gray-100 text-xs"
    >
      {icon && (
        <div
          className={clsx(
            "flex w-10 my-auto mx-auto justify-center",
            "align-middle ml-2"
          )}
        >
          <FontAwesomeIcon
            className={clsx(
              "align-content-center align-content-center",
              "mx-auto my-auto justify-center align-middle "
            )}
            icon={icon}
          />
        </div>
      )}
      <div className={"flex my-auto px-2 mx-1 capitalize w-full"}>{name}</div>
    </div>
  );
}
