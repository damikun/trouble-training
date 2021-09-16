import { useEffect } from "react";

export default function useOnClickOutside(
  elemetref: React.RefObject<
    HTMLDivElement | HTMLTextAreaElement | null | undefined
  >,
  handler: () => void
) {
  useEffect(() => {
    const listener = (event: any) => {
      // event.preventDefault();
      // event.stopPropagation();
      if (
        !elemetref ||
        elemetref === null ||
        elemetref?.current?.contains(event.target)
      ) {
        return;
      }

      handler();
    };

    document.addEventListener("mousedown", listener);
    document.addEventListener("touchstart", listener);

    return () => {
      document.removeEventListener("mousedown", listener);
      document.removeEventListener("touchstart", listener);
    };
  }, [elemetref, handler]);
}
