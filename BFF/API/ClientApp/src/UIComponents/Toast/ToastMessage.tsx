/* Author: Dalibor Kundrat  https://github.com/damikun */

import React, { useEffect, useMemo } from "react";
import { Toast } from "./ToastProvider";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faTimes,
  faExclamationCircle,
  faCheck,
  faInfoCircle,
} from "@fortawesome/free-solid-svg-icons";
import clsx from "clsx";
import { motion } from "framer-motion";

const VARIANTS = {
  Info: {
    base: "bg-white border-blue-500",
    iconstyle: "text-blue-500 ",
    icon: faInfoCircle,
    name: "Info",
  },

  Error: {
    base: "bg-white border-red-500 ",
    iconstyle: "text-red-500 ",
    icon: faExclamationCircle,
    name: "Error",
  },

  Warning: {
    base: "bg-white border-yellow-500",
    iconstyle: "text-yellow-500 ",
    icon: faExclamationCircle,
    name: "Warning",
  },

  Success: {
    base: "bg-white border-green-500",
    iconstyle: "text-green-500 ",
    icon: faCheck,
    name: "Success",
  },
};

export type Truncate =
  | "truncate-1-lines"
  | "truncate-2-lines"
  | "truncate-3-lines";

export type ToastMessage = {
  id: string;
  lifetime?: number;
  variant?: keyof typeof VARIANTS | undefined;
  onRemove?: (id: string) => void;
  truncate?: Truncate;
} & Toast;

export default function ToastMessage({
  id,
  header,
  message,
  lifetime,
  onRemove,
  truncate = "truncate-1-lines",
  icon,
  type,
}: ToastMessage) {
  const Var = useMemo(
    () =>
      type
        ? VARIANTS[type]
        : {
            base: "bg-white border-gray-600 ",
            iconstyle: "",
            icon: icon,
            name: header,
          },
    [VARIANTS, type, icon, header]
  );

  useEffect(() => {
    if (lifetime && onRemove) {
      setTimeout(() => {
        onRemove(id);
      }, lifetime);
    }
  }, [lifetime]);

  return (
    <div
      className={clsx(
        "flex w-full visible flex-row shadow-lg",
        "border-l-4 rounded-md duration-100 cursor-pointer",
        "hover:bg-gray-50",
        Var.base,
        type && "max-h-40"
      )}
    >
      <div className="flex flex-row p-2 flex-no-wrap w-full">
        {Var.icon && (
          <div
            className={clsx(
              "flex items-center h-12 w-12",
              "mx-auto text-xl select-none"
            )}
          >
            <FontAwesomeIcon
              className={clsx("mx-auto", Var.iconstyle)}
              icon={Var.icon}
            />
          </div>
        )}

        <div className="flex flex-col flex-no-wrap px-1 w-full">
          <div className="flex my-auto font-bold select-none">{Var.name}</div>
          <p
            className={clsx(
              "-mt-0.5 my-auto break-all flex",
              "text-gray-600 text-sm",
              typeof message === "string" && truncate
            )}
          >
            {message}
          </p>
        </div>
        <motion.div
          whileHover={{ scale: 1.2 }}
          onClick={() => onRemove && onRemove(id)}
          className={clsx(
            "w-10 h-12 mr-2 items-center mx-auto",
            "text-center leading-none text-lg"
          )}
        >
          <FontAwesomeIcon
            className={clsx(
              "mx-auto my-auto h-full text-center text-gray-600",
              "cursor-pointer  "
            )}
            icon={faTimes}
          />
        </motion.div>
      </div>
    </div>
  );
}
