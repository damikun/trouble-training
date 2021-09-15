import { useState, useEffect } from "react";

export default function useDebounce(value: string | any, delay: number): any {
  const [DebouncedValue, setDebouncedValue] = useState(value);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    return () => {
      clearTimeout(handler);
    };
  }, [value, delay]);

  return DebouncedValue;
}
