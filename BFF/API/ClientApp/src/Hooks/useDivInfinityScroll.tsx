import React, { useEffect, useState } from "react";
import useDebouncedHandler from "../Hooks/useDebouncedHandler";
import useDidMountEffect from "../Hooks/useDidMountEffect";

export type useDivInfinityScrollProps = {
  ref: React.RefObject<HTMLDivElement>;
  fromEnd_px?: number;
  handleOnEnd?: () => void;
};

export default function useDivInfinityScroll({
  ref,
  handleOnEnd,
  fromEnd_px,
}: useDivInfinityScrollProps) {
  const [isEndArea, setisEndArea] = useState(false);

  const onHandle = useDebouncedHandler(handleScroll, 10);

  const onResolutionChanged = useDebouncedHandler(handleOnEnd, 100);

  useEffect(() => {
    if (ref?.current) {
      ref.current.addEventListener("scroll", onHandle);
    }

    return () => {
      ref.current?.removeEventListener("scroll", onHandle);
    };
  }, [ref, ref?.current, onHandle]);

  useDidMountEffect(() => {
    if (ref.current?.scrollHeight === 0 || ref.current?.scrollTop === 0) {
      onResolutionChanged();
    } else if (ref.current?.scrollHeight && ref.current?.offsetHeight) {
      if (ref.current?.scrollHeight - ref.current?.offsetHeight === 0) {
        onResolutionChanged();
      }
    }
  }, [
    ref.current?.scrollHeight,
    ref.current?.offsetHeight,
    ref.current?.scrollTop,
  ]);

  function handleScroll() {
    if (ref.current) {
      if (
        ref.current.scrollTop! + ref.current.offsetHeight! >=
        ref.current.scrollHeight! -
          (fromEnd_px && fromEnd_px > 0 ? fromEnd_px : 0)
      ) {
        if (!isEndArea) {
          setisEndArea(true);

          handleOnEnd && handleOnEnd();
        }
      } else {
        if (isEndArea) {
          setisEndArea(false);
        }
      }
    }
  }

  return [isEndArea];
}
