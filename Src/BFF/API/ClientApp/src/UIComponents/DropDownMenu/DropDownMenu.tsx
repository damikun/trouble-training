import clsx from "clsx";
import { AnimatePresence, motion } from "framer-motion";
import React, {
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useRef,
  useState,
} from "react";
import useOnClickOutside from "../../Hooks/useOnOutsideElementClick";

export const POSITION_VARIANTS = {
  topright: {
    base: " top-0 right-0",
  },
  topleft: {
    base: " top-0 left-0 ",
  },
  bottomright: {
    base: " bottom-0 right-0",
  },
  bottomleft: {
    base: " bottom-0 left-0 ",
  },
};

export const ORIENTATION_VARIANTS = {
  upleft: {
    base: "-translate-y-full -translate-x-full",
  },
  upright: {
    base: "-translate-y-full",
  },
  downleft: {
    base: "-translate-x-full",
  },
  downright: {
    base: "",
  },
};

export type DropDownMenuProps = {
  children: React.ReactElement;
  menu?: React.ReactElement;
  enabled?: boolean;
  position?: keyof typeof POSITION_VARIANTS;
  orientation?: keyof typeof ORIENTATION_VARIANTS;
  preventCloseOnClick?: boolean;
};

export type DropDownContextType = {
  close: () => void;
  setVisibility: (value: React.SetStateAction<boolean>) => void;
  open: () => void;
  disabled: boolean;
  isOpen: boolean;
};

export const DropDownContext = React.createContext<DropDownContextType>({
  close: () => {},
  open: () => {},
  setVisibility: (value: React.SetStateAction<boolean>) => {},
  disabled: false,
  isOpen: false,
});

export const useDropDownContext = () => useContext(DropDownContext);

export default React.memo(DropDownMenu);

function DropDownMenu({
  children,
  menu,
  enabled,
  position = "bottomleft",
  orientation = "downright",
  preventCloseOnClick = false,
}: DropDownMenuProps) {
  const [visible, setVisible] = useState(false);

  const parrent = useRef<HTMLDivElement>(null);
  const selected = useRef<HTMLDivElement>(null);

  useOnClickOutside(parrent, () => setVisible(false));

  const isVisible =
    visible && enabled !== undefined && enabled !== null ? enabled : false;

  const Var_position = POSITION_VARIANTS[position];

  const Var_orientation = ORIENTATION_VARIANTS[orientation];

  useEffect(() => {
    setVisible(false);
  }, [enabled]);

  const dropDownContext = useCallback(
    (enabled: boolean) => {
      return {
        close() {
          setVisible(false);
        },
        open() {
          setVisible(true);
        },
        setVisibility(value: React.SetStateAction<boolean>) {
          return setVisible(value);
        },
        disabled: !enabled,
        isOpen: visible,
      };
    },
    [setVisible, visible]
  );

  const handleClick = useCallback(
    (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => {
      event.preventDefault();
      event.stopPropagation();

      if (preventCloseOnClick) {
        //@ts-ignore
        if (!selected.current?.contains(event.target)) {
          setVisible((state) => !state);
        }
      } else {
        setVisible((state) => !state);
      }
    },
    [preventCloseOnClick, selected.current]
  );

  const motionInitial = useMemo(() => {
    return { opacity: 0 };
  }, []);

  const motionAnimate = useMemo(() => {
    return { opacity: 1 };
  }, []);
  const motionExit = useMemo(() => {
    return { opacity: 0 };
  }, []);
  const motionTransition = useMemo(() => {
    return { duration: 0.2 };
  }, []);

  return (
    <DropDownContext.Provider
      value={dropDownContext(enabled ? enabled : false)}
    >
      <div
        onClick={handleClick}
        ref={parrent}
        className={clsx(
          "flex relative",
          "align-middle cursor-pointer items-center"
        )}
      >
        {children}

        <AnimatePresence>
          {isVisible && (
            <motion.div
              initial={motionInitial}
              animate={motionAnimate}
              exit={motionExit}
              transition={motionTransition}
              ref={selected}
              className={clsx(
                "flex z-30 absolute rounded-md shadow-lg bg-white ",
                Var_position.base
              )}
            >
              <div
                className={clsx(
                  "flex z-30 object-top mt-1 absolute translate transform",
                  Var_orientation.base
                )}
              >
                {menu}
              </div>
            </motion.div>
          )}
        </AnimatePresence>
      </div>
    </DropDownContext.Provider>
  );
}
