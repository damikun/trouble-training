import React, { useMemo } from "react";

export type CapitalizedTextProps = {
  children?: string | undefined | null | any;
  enable?: boolean;
  tagVariant?: "div" | "h1" | "h2" | "h3" | "p";
} & React.DetailedHTMLProps<
  React.HTMLAttributes<HTMLDivElement>,
  HTMLDivElement
>;

export function Capitalizer(value: string | undefined | null) {
  if (!value) return null;

  if (typeof value !== "string" || value.length < 2) {
    return value;
  }

  return value.charAt(0).toUpperCase() + value.slice(1);
}

export default React.memo(CapitalizedText);

function CapitalizedText({
  children,
  enable = true,
  tagVariant = "div",
  ...rest
}: CapitalizedTextProps) {
  const capitalisedValue = useMemo(() => {
    if (enable) {
      return Capitalizer(children);
    } else {
      return children;
    }
  }, [children, enable]);

  if (tagVariant === "div") {
    return <div {...rest}>{capitalisedValue}</div>;
  } else if (tagVariant === "p") {
    return <p {...rest}>{capitalisedValue}</p>;
  } else if (tagVariant === "h1") {
    return <h1 {...rest}>{capitalisedValue}</h1>;
  } else if (tagVariant === "h2") {
    return <h2 {...rest}>{capitalisedValue}</h2>;
  } else {
    return <h3 {...rest}>{capitalisedValue}</h3>;
  }
}
