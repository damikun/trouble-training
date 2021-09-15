import clsx from "clsx";
import React, { useMemo, useState } from "react";
import useDidMountEffect from "../../Hooks/useDidMountEffect";

export type LoadingBarProps = {
  isloading: boolean;
  loading_length?: number;
  color?: "bg-gray-400" | "bg-gray-300" | "bg-blue-400" | "bg-blue-500";
};

export default React.memo(LoadingBar);

function LoadingBar({
  isloading,
  loading_length = 100,
  color,
}: LoadingBarProps) {
  const [state, setstate] = useState(isloading ? loading_length : 0);

  useDidMountEffect(() => {
    if (isloading) {
      setstate(loading_length);
    } else {
      setstate(0);
    }
  }, [isloading]);

  const style = useMemo(() => {
    return { width: `${state}%` };
  }, [state]);

  return (
    <div className="relative">
      <div className="overflow-hidden h-1 flex rounded">
        <div
          style={style}
          className={clsx(
            "shadow-none flex justify-center delay-100 duration-2000",
            color ? color : "bg-gray-400",
            state !== 0
              ? " transition-all opacity-100  "
              : " invisible transition-none opacity-0"
          )}
        />
      </div>
    </div>
  );
}
