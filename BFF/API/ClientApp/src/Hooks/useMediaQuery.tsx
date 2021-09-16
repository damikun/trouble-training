import { useState, useEffect } from "react";

function useMediaQuery(query: string) {
  const [match, setMatch] = useState(() => window.matchMedia(query).matches);

  useEffect(() => {
    const updateMatch = () => setMatch(window.matchMedia(query).matches);

    updateMatch();

    window.matchMedia(query).addEventListener("change", updateMatch);
    return () => {
      window.matchMedia(query).removeEventListener("change", updateMatch);
    };
  }, [query]);

  return match;
}

export default useMediaQuery;
