import clsx from "clsx";
import { ReactComponent as NotFound } from "../Images/recordNotFound.svg";

type NoRecordsProps = {
  className?: String;
  message?: string;
};

export default function RecordNotFound({
  className,
  message = "Enity was not found",
}: NoRecordsProps) {
  return (
    <div
      className={clsx(
        "flex flex-col w-full text-center p-2 space-y-2",
        "font-semibold absolute items-center max-w-full",
        className ? className : "h-32 md:h-48"
      )}
    >
      <NotFound />
      <div className="h-6 w-full">{message}</div>
    </div>
  );
}
