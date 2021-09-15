import clsx from "clsx";
import React from "react";
import useContextMenu from "../../Hooks/useContextMenu";

export type ContextMenuProps = {
  parrentref: React.RefObject<HTMLDivElement>;
  children: React.ReactNode;
};

function ContextMenu({ parrentref, children }: ContextMenuProps) {
  const menucontext = useContextMenu(parrentref);
  return (
    <>
      {menucontext?.menu === true && (
        <div
          className={clsx(
            "flex flex-col z-30 shadow-md bg-white",
            "position-fixed fixed rounded-md select-none w-40"
          )}
          style={{
            top: menucontext.yPos,
            left: menucontext.xPos,
          }}
        >
          <div className="flex flex-col py-2">{children}</div>
        </div>
      )}
    </>
  );
}

export default React.memo(ContextMenu);
