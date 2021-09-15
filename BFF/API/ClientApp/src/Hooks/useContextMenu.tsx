import { useEffect, useCallback, useState } from "react";

const useContextMenu = (outerRef: React.RefObject<HTMLDivElement>) => {
  const [xPos, setXPos] = useState("0px");
  const [yPos, setYPos] = useState("0px");
  const [menu, showMenu] = useState(false);

  const handleContextMenu = (event: any) => {
    if (outerRef !== null && outerRef.current?.contains(event.target)) {
      setXPos(`${event.pageX}px`);
      setYPos(`${event.pageY}px`);
      event.preventDefault();
      showMenu(true);
    } else {
      event.preventDefault();
      event.stopPropagation();
      showMenu(false);
    }
  };

  const handleClick = useCallback(
    (e) => {
      showMenu(false);
    },
    [showMenu]
  );

  useEffect(() => {
    document.addEventListener("click", handleClick);
    document.addEventListener("contextmenu", handleContextMenu);
    return () => {
      document.removeEventListener("click", handleClick);
      document.removeEventListener("contextmenu", handleContextMenu);
    };
    //@ts-ignore
  }, []);

  return { xPos, yPos, menu };
};

export default useContextMenu;
