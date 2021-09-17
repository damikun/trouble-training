import clsx from "clsx";
import React, { useCallback, useEffect } from "react";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { faCheck, faCopy } from "@fortawesome/free-solid-svg-icons";
import useCopyClipboard from "../../Hooks/useCopyToClip";
import StayledButton from "../Buttons/StayledButton";

type styleVariant =
  | "primarygray"
  | "primaryblue"
  | "primarygreen"
  | "primaryred"
  | "secondaryblue"
  | "secondarygray"
  | "secondarylightgray"
  | "secondarygreen"
  | "secondaryyellow"
  | "ternarygray"
  | "error"
  | "errorInverted"
  | "invisible"
  | undefined;

const VARIANTS = {
  primary: {
    base: "",
    onSuccess: "primarygreen" as styleVariant,
    onNormal: "primarygray" as styleVariant,
  },
  secondary: {
    base: "",
    onSuccess: "secondarygreen" as styleVariant,
    onNormal: "secondarylightgray" as styleVariant,
  },
  secondarygray: {
    base: "",
    onSuccess: "secondarygreen" as styleVariant,
    onNormal: "secondarygray" as styleVariant,
  },
  classic: {
    base: "",
    onSuccess: "secondarygray" as styleVariant,
    onNormal: "secondarygray" as styleVariant,
  },
};

const SIZE = {
  standard: "normal" as "normal",
  small: "small" as "small",
};

export type CopyToClipboardProps = {
  icon?: IconProp;
  iconSuccess?: IconProp;
  name?: string;
  className?: string;
  nameSuccess?: string;
  variant?: keyof typeof VARIANTS;
  size?: keyof typeof SIZE;
  dataSource: string | null | undefined;
  copyAutoResetDuration?: number;
  grouphover?: boolean;
  iconOnly?: boolean;
  onCopy?: () => void;
};

export default function CopyToClipboard({
  icon = faCopy,
  iconSuccess = faCheck,
  name = "Copy",
  nameSuccess = "Done",
  variant = "secondary",
  size = "standard",
  className,
  grouphover = true,
  iconOnly,
  dataSource,
  onCopy,
  copyAutoResetDuration = 2000,
}: CopyToClipboardProps) {
  const [isCopied, setIsCopied] = useCopyClipboard(
    dataSource ? dataSource : "",
    {
      successDuration: copyAutoResetDuration,
    }
  );

  //@ts-ignore
  const resolved_icon = !isCopied ? icon : iconSuccess;

  useEffect(() => {
    if (isCopied && onCopy) {
      onCopy();
    }
  }, [isCopied]);

  const Var = VARIANTS[variant];

  const VarSize: "normal" | "small" = size
    ? SIZE[size] || SIZE.standard
    : SIZE.standard;

  const handleClick = useCallback(
    (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
      event.preventDefault();
      event.stopPropagation();
      setIsCopied();
    },
    [setIsCopied]
  );
  return (
    <StayledButton
      onClick={handleClick}
      type="button"
      iconOnly={iconOnly}
      size={VarSize}
      variant={isCopied ? Var.onSuccess : Var.onNormal}
      iconLeft={resolved_icon}
      className={clsx(
        className,
        grouphover && "opacity-0 group-hover:opacity-100"
      )}
    >
      {!isCopied ? name : nameSuccess}
    </StayledButton>

  );
}
