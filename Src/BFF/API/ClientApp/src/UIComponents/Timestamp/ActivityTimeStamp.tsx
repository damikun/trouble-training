import clsx from "clsx";
import React from "react";

export type ActivityTimeStampProps = {
  date: Date | undefined;
  currentDT: Date;
  textOnly?: boolean;
  recentLimit?: number;
  className?: string;
};

export default React.memo(ActivityTimeStamp);

const style =
  "truncate-1-lines break-all bg-white flex rounded-md px-2 font-semibold py-0.5 items-center";

function ActivityTimeStamp({
  date,
  currentDT,
  textOnly,
  recentLimit,
  className,
}: ActivityTimeStampProps) {
  // Until this time it will render user online indicator
  const ONLINE_INDICATOR_MINUTES = 5;

  const Mins_IN_HOUR = 60;
  const Mins_IN_DAY = 1440;
  const Mins_IN_YEAR = 525600;

  const ToMins = function ToMins(num: number) {
    return num / 1000 / 60;
  };

  // 2000 only for check if there is no default date 1970
  if (date === undefined || date === null || currentDT.getFullYear() < 2000)
    return <div>Unknown</div>;

  const TimeSpan_mins = ToMins(Math.abs((date as any) - (currentDT as any)));

  let value = 0;

  const recent_limit = recentLimit ? recentLimit : ONLINE_INDICATOR_MINUTES;

  if (TimeSpan_mins >= 0 && TimeSpan_mins <= recent_limit) {
    return (
      <div
        className={clsx(
          !textOnly && !className && "bg-green-500 text-white",
          className ? className : style
        )}
      >
        <div className="truncate-1-lines break-all">Recent</div>
      </div>
    );
  } else if (TimeSpan_mins > recent_limit && TimeSpan_mins < Mins_IN_HOUR) {
    value = Math.floor(TimeSpan_mins);
    if (value <= 1) {
      // Just in case the ONLINE_INDICATOR_MINUTES < 1
      return (
        <div className={clsx(className ? className : style)}>
          {value + " min ago"}
        </div>
      );
    } else {
      return (
        <div className={clsx(className ? className : style)}>
          {value + " min ago"}
        </div>
      );
    }
  } else if (TimeSpan_mins >= Mins_IN_HOUR && TimeSpan_mins < Mins_IN_DAY) {
    value = Math.floor(TimeSpan_mins / Mins_IN_HOUR);
    if (value === 1) {
      return (
        <div className={clsx(className ? className : style)}>
          {value + " hour ago"}
        </div>
      );
    } else {
      return (
        <div className={clsx(className ? className : style)}>
          {value + " hours ago"}
        </div>
      );
    }
  } else if (TimeSpan_mins >= Mins_IN_DAY && TimeSpan_mins < Mins_IN_YEAR) {
    value = Math.floor(TimeSpan_mins / Mins_IN_DAY);
    if (value === 1) {
      return (
        <div className={clsx(className ? className : style)}>
          {value + " day ago"}
        </div>
      );
    } else {
      return (
        <div className={clsx(className ? className : style)}>
          {value + " days ago"}
        </div>
      );
    }
  } else if (TimeSpan_mins >= Mins_IN_YEAR && TimeSpan_mins < Mins_IN_YEAR) {
    value = Math.floor(TimeSpan_mins / Mins_IN_YEAR);
    if (value === 1) {
      return (
        <div className={clsx(className ? className : style)}>
          {value + " year ago"}
        </div>
      );
    } else {
      return (
        <div className={clsx(className ? className : style)}>
          {value + " years ago"}
        </div>
      );
    }
  }

  return <div className={clsx(className ? className : style)}>N/A</div>;
}
