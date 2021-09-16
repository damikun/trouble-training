import React, { useState, useEffect, useCallback } from "react";
import useDebounce from "../../Hooks/useDebounce";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import clsx from "clsx";
import StayledLabel from "../Label/StayledLabel";
import CapitalizedText from "../CapitalizedText/CapitalizedText";
import useDidMountEffect from "../../Hooks/useDidMountEffect";

export type DebouncingInputProps = {
  icon?: IconProp;
  className?: string;
  debouncingTimeout?: number;
  startValue?: string | number;
  loader?: React.ReactNode;
  capitalize?: boolean;
  label?: string;
  labelstyle?: string;
  onDebouncingChange?: (value: string) => void;
  float?: "right" | "left";
  textarea?: boolean;
  fullwidth?: "md:w-full" | "w-full" | "w-auto" | "w-full md:w-auto";
} & React.DetailedHTMLProps<
  React.InputHTMLAttributes<HTMLInputElement>,
  HTMLInputElement
>;

export default React.memo(DebouncingInput);

function DebouncingInput({
  icon,
  className,
  capitalize = true,
  debouncingTimeout,
  startValue,
  loader,
  label,
  labelstyle,
  float = "left",
  textarea,
  fullwidth = "w-auto",
  onDebouncingChange,
  ...rest
}: DebouncingInputProps) {
  const [searchTerm, setSearchTerm] = useState(
    rest.value && typeof rest.value === "string" ? rest.value : ""
  );

  const debouncedSearchTerm = useDebounce(
    searchTerm,
    debouncingTimeout ? debouncingTimeout : 500
  );

  useEffect(() => {
    if (onDebouncingChange && rest.value !== debouncedSearchTerm) {
      onDebouncingChange(debouncedSearchTerm);
    }
  }, [debouncedSearchTerm, onDebouncingChange]);

  useDidMountEffect(() => {
    if (rest.value !== searchTerm) {
      setSearchTerm(
        rest.value && typeof rest.value === "string" ? rest.value : ""
      );
    }
  }, [rest.value, setSearchTerm]);

  const onInputChange = useCallback(
    (e) => {
      e.preventDefault();

      if (rest && rest.onChange) {
        rest.onChange(e);
      }
      setSearchTerm(e.target.value);
    },
    [setSearchTerm, rest, rest.onChange]
  );

  return (
    <form
      onSubmit={(e) => e.preventDefault()}
      className={clsx("flex flex-col flex-no-wrap h-full", fullwidth)}
    >
      {label && (
        <StayledLabel
          className={clsx(
            "flex",
            labelstyle
              ? labelstyle
              : clsx(
                  "text-sm sm:text-base font-semibold",
                  "align-items-center pb-2 flex-no-wrap",
                  "flex-grow-0 break-normal"
                )
          )}
          htmlFor={rest.id ? rest.id : rest.name ? rest.name : ""}
        >
          <label>
            <CapitalizedText>{label}</CapitalizedText>
          </label>
        </StayledLabel>
      )}

      <div
        className={clsx(
          "flex flex-row justify-start align-middle",
          "outline content-center",
          className
        )}
      >
        <div
          className={clsx(
            "mx-1 font-normal text-xs my-auto",
            "align-content-center"
          )}
        >
          {icon ? (
            <FontAwesomeIcon
              className="align-content-center align-content-center ml-1"
              icon={icon}
            />
          ) : null}
        </div>
        <input
          {...rest}
          value={searchTerm}
          className={clsx(
            "mx-1 w-full justify-start placeholder-gray-600",
            "my-auto font-semibold bg-transparent",
            "outline-none border-transparent",
            float === "right" && " text-right",
            !capitalize && "lowercase"
          )}
          onChange={onInputChange}
        />
        {loader && <div className="flex mx-1 items-center">{loader}</div>}
      </div>
    </form>
  );
}
