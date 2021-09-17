import clsx from "clsx";
import React from "react";

export type SectionProps = {
  children?: React.ReactNode | string;
  ref?: React.RefObject<HTMLInputElement>;
  className?: string;
  variant?: keyof typeof VARIANTS;
  padding?: keyof typeof PADDINGVARIANTS;
  id?: string;
};

const VARIANTS = {
  row: {
    base: "flex-row space-x-2",
  },

  col: {
    base: "flex-col space-y-2",
  },
};

const PADDINGVARIANTS = {
  toponly: {
    padding: "pt-2",
  },

  bottomonly: {
    padding: "pb-2",
  },

  topandbottom: {
    padding: "py-2",
  },

  none: {
    padding: "py-0",
  },
};

export default function Section({
  children,
  ref,
  id,
  className,
  variant = "col",
  padding = "topandbottom",
}: SectionProps) {
  const Var = VARIANTS[variant] || VARIANTS.col;

  const PaddingVar = PADDINGVARIANTS[padding] || PADDINGVARIANTS.topandbottom;

  return (
    <div
      id={id}
      ref={ref}
      className={clsx(
        "flex break-normal w-full max-w-full",
        Var.base,
        PaddingVar.padding,
        className
      )}
    >
      {children}
    </div>
  );
}
