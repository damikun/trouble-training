import clsx from "clsx";
import { motion, PanInfo, useMotionValue } from "framer-motion";
import React, { useEffect, useRef } from "react";
import { useCallback } from "react";

export type ToggleProps = {
  sizeVariant?: keyof typeof SIZE_VARIANTS;
  styleVariant?: keyof typeof STYLE_VARIANTS;
  disabled?: boolean;
  duration?: "duration-150" | "duration-150" | "duration-150";
  checked?: boolean;
} & React.InputHTMLAttributes<HTMLInputElement>;

const SIZE_VARIANTS = {
  sm: {
    base: "w-12",
    body: "w-5 h-5",
    translate: "translate-x-5",
    x: 20,
  },

  md: {
    base: "w-16",
    body: "w-7 h-7",
    translate: "translate-x-7",
    x: 25,
  },

  xl: {
    base: "w-20",
    body: "w-10 h-10",
    translate: "translate-x-8",
    x: 30,
  },
};

const STYLE_VARIANTS = {
  gray: {
    on: "bg-gray-200",
    off: "bg-gray-200",
  },

  green: {
    on: "bg-green-500",
    off: "bg-gray-200",
  },

  red: {
    on: "bg-red-500",
    off: "bg-gray-200",
  },

  yellow: {
    on: "bg-yellow-500",
    off: "bg-gray-200",
  },
};

function Toggle({
  sizeVariant = "sm",
  styleVariant = "green",
  disabled = false,
  duration,
  ...rest
}: ToggleProps) {
  const x = useMotionValue(20);

  useEffect(() => {
    if (rest.checked) {
      x.set(20);
    } else {
      x.set(0);
    }
  }, [rest.checked]);

  const Size = SIZE_VARIANTS[sizeVariant] || SIZE_VARIANTS.sm;

  const Color = STYLE_VARIANTS[styleVariant] || STYLE_VARIANTS.gray;

  const inputRef = useRef<HTMLInputElement>(null);

  const handleChange = useCallback(() => {
    !disabled && inputRef.current?.click();
  }, [inputRef, disabled]);

  const handleDragEnd = useCallback(
    (event: MouseEvent | TouchEvent | PointerEvent, position: PanInfo) => {
      if (!disabled) {
        if (Math.abs(position.offset.x) / 3 > Size.x / 3) {
          if (position.offset.x > 0) {
            !rest.checked && handleChange();
          } else {
            rest.checked && handleChange();
          }
        }
      }
    },
    [disabled, Size, rest.checked, handleChange]
  );

  const handleDefault = useCallback(
    (e: React.MouseEvent<HTMLDivElement, MouseEvent>) => {
      e.stopPropagation();
    },
    []
  );

  return (
    <div className="flex">
      <div
        onMouseDown={handleChange}
        className={clsx(
          "flex bg-gray-200 rounded-full p-0.5 px-1",
          "transition duration-300 cursor-pointer m-1 overflow-hidden",
          "whitespace-pre select-none mx-auto center ",
          rest.checked ? Color.on : Color.off,
          Size.base,
          duration,
          rest.checked && "justify-end",
          disabled && "cursor-not-allowed"
        )}
      >
        <motion.div
          className={clsx(
            "rounded-full bg-white shadow-md z-10 ",
            Size.body,
            rest.checked && "justify-end"
          )}
          drag="x"
          animate={!disabled}
          aria-disabled={disabled}
          onMouseDown={handleDefault}
          whileHover={{ scale: 1.1 }}
          whileDrag={{ scale: 1.1 }}
          dragTransition={{ bounceStiffness: 500, bounceDamping: 20 }}
          dragConstraints={{ left: 0, right: 0 }}
          onDragEnd={handleDragEnd}
          dragElastic={0.2}
        >
          <input
            ref={inputRef}
            checked={rest.checked}
            {...rest}
            type="checkbox"
            className="opacity-0 absolute invisible"
          />
        </motion.div>
      </div>
    </div>
  );
}

export default Toggle;
