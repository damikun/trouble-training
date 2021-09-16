import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faClock } from "@fortawesome/free-solid-svg-icons";
import clsx from "clsx";

type TimestampProps = {
  dt: string | null | undefined | Date;
  classNameIcon?: string;
  classNameContent?: string;
  hideIcon?: boolean;
};

const monthNames = [
  "Jan",
  "Feb",
  "Mar",
  "Apr",
  "May",
  "Jun",
  "Jul",
  "Aug",
  "Sep",
  "Oct",
  "Nov",
  "Dec",
];

export default React.memo(Timestamp);

function Timestamp({
  dt,
  classNameIcon,
  classNameContent,
  hideIcon,
}: TimestampProps) {
  if (dt === null || dt === undefined) {
    return (
      <div className="flex">
        {!hideIcon && (
          <FontAwesomeIcon
            icon={faClock}
            className={clsx("my-auto", classNameIcon)}
          />
        )}
        <div className={clsx("mx-1 my-auto ", classNameContent)}>{`N/A`}</div>
      </div>
    );
  } else {
    const itemdate = dt instanceof Date ? dt : new Date(dt);
    return (
      <div className={clsx("flex flex-row items-center m-auto")}>
        {!hideIcon && (
          <FontAwesomeIcon
            icon={faClock}
            className={clsx("my-auto mx-auto mr-1", classNameIcon)}
          />
        )}

        <div
          className={clsx(
            "flex my-auto flex-no-wrap break-normal truncate",
            classNameContent
          )}
        >
          {`${monthNames[itemdate.getMonth()]} ${itemdate.getDate()} `}
        </div>
      </div>
    );
  }
}
