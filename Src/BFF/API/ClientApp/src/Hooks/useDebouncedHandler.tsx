import { useCallback } from "react";

export default function useDebouncedHandler(
  onDebouncedCallback: ((value?: any) => void) | undefined,
  time_ms: number
) {
  let timer: undefined | NodeJS.Timeout = undefined;

  const handler = useCallback(
    (value?: any) => {
      if (timer) {
        clearTimeout(timer);
      }
      timer = setTimeout(() => {
        onDebouncedCallback && onDebouncedCallback(value);
      }, time_ms);
    },
    [timer, onDebouncedCallback, time_ms]
  );

  return handler;
}
