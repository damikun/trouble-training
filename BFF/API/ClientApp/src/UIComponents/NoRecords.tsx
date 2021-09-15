import clsx from "clsx";
//@ts-ignore
import { ReactComponent as Empty } from "../Images/TakenByAliens.svg";

const NOT_FOUND_SIZE = {
  small: "h-16 md:h-32",
  medium: "h-24 md:h-40 prose-sm",
  big: "h-32 md:h-48 prose",
  extra: "h-40 md:h-56 prose-lg",
};

const VISIBILITY = {
  always: "flex",
  medium: "hidden md:flex",
  large: "hidden lg:flex",
  extralarge: "hidden xl:flex",
};

type NoRecordsProps = {
  className?: String;
  size?: keyof typeof NOT_FOUND_SIZE;
  visibility?: keyof typeof VISIBILITY;
  message?: string;
};

export default function NoRecords({
  className,
  size = "medium",
  visibility = "always",
  message = "No records to display",
}: NoRecordsProps) {
  const Var = size
    ? NOT_FOUND_SIZE[size] || NOT_FOUND_SIZE.big
    : NOT_FOUND_SIZE.big;

  const Visibility = visibility
    ? VISIBILITY[visibility] || VISIBILITY.always
    : VISIBILITY.always;

  return (
    <div
      className={clsx(
        "flex-col w-full text-center p-2 overflow-hidden",
        "font-semibold items-center max-w-full space-y-2",
        className ? className : Var,
        Visibility
      )}
    >
      <Empty />

      <div className="my-auto w-full">{message}</div>
    </div>
  );
}
