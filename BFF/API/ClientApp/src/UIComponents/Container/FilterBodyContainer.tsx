import React, { useCallback, useEffect, useMemo, useState } from "react";
import { faSearch, faTimes } from "@fortawesome/free-solid-svg-icons";
import clsx from "clsx";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import useDebounce from "../../Hooks/useDebounce";
import useDidMountEffect from "../../Hooks/useDidMountEffect";
import StayledButton from "../Buttons/StayledButton";

export type FilterBodyContainerProps = {
  name?: string | React.ReactNode;
  search?: boolean;
  children?: React.ReactNode;
  className?: string;
  searchValue?: string | number | readonly string[] | undefined;
  onSearchChange?: ((value: string) => void) | undefined;
  mobileSecondLine?: boolean;
};

export default React.memo(FilterBodyContainer);

function FilterBodyContainer({
  name,
  search,
  children,
  className,
  searchValue,
  onSearchChange,
  mobileSecondLine,
}: FilterBodyContainerProps) {
  const searchterm = useMemo(() => {
    return searchValue ? searchValue : "";
  }, [searchValue]);

  const HandleOnChange = useCallback(
    (value: string) => onSearchChange && onSearchChange(value),
    [onSearchChange]
  );

  return (
    <div
      className={clsx(
        "flex w-full",
        mobileSecondLine
          ? "flex-col space-y-1 md:space-y-0 md:flex-row md:justify-between"
          : "flex-row justify-between",
        className
      )}
    >
      <div
        className={clsx(
          "w-full space-x-2 items-center",
          name && search && "justify-between",
          mobileSecondLine ? "flex-1 w-full md:flex" : "flex"
        )}
      >
        {name && (
          <div className="flex flex-no-wrap break-normal font-bold tracking-wide">
            {name}
          </div>
        )}
        {search && (
          <SearchDebounc
            // onKeyDown={(e) => e.preventDefault()}
            value={searchterm}
            placeholder="Search"
            className="w-full"
            // type="Search"
            debouncingTimeout={500}
            onDebouncingChange={HandleOnChange}
          />
        )}
      </div>

      <div
        className={clsx(
          "flex flex-row space-x-2 whitespace-pre relative",
          children ? (mobileSecondLine ? "md:ml-2" : "ml-2") : "",
          mobileSecondLine ? "justify-end md:justify-start" : "justify-start"
        )}
      >
        {children}
      </div>
    </div>
  );
}

//////////////////////////////////////

export type SearchDebouncProps = {
  debouncingTimeout?: number;
  startValue?: string | number;
  loader?: React.ReactNode;
  capitalize?: boolean;
  onDebouncingChange?: (value: string) => void;
} & React.DetailedHTMLProps<
  React.InputHTMLAttributes<HTMLInputElement>,
  HTMLInputElement
>;

function SearchDebounc({
  capitalize = true,
  debouncingTimeout,
  startValue,
  loader,
  onDebouncingChange,
  ...rest
}: SearchDebouncProps) {
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
      if (rest && rest.onChange) {
        rest.onChange(e);
      }
      setSearchTerm(e.target.value);
    },
    [setSearchTerm, rest, rest.onChange]
  );

  const inputRef = React.useRef<HTMLInputElement>(null);

  const handleInputFocus = useCallback(
    (e) => {
      inputRef?.current?.focus();
    },
    [inputRef]
  );

  const hanldeReset = useCallback(() => {
    setSearchTerm("");
  }, [setSearchTerm]);

  return (
    <div className={"flex flex-col flex-no-wrap h-full w-auto"}>
      <form
        onSubmit={(e) => {
          e.preventDefault();
        }}
        onClick={handleInputFocus}
        className={clsx(
          "flex flex-row justify-start align-middle",
          "outline content-center items-center max-w-full",
          "border-2 border-transparent rounded-md",
          "transition duration-200 px-1 space-x-1",
          "focus-within:border-blue-500 group",
          "hover:border-blue-500 text-xs md:text-sm",
          "h-full transition duration-200",
          searchTerm && searchTerm !== ""
            ? "focus-within:w-auto bg-white border-gray-400"
            : "focus-within:w-auto w-20 focus-within:bg-white bg-transparent"
        )}
      >
        <div
          className={clsx(
            "font-normal text-xs my-auto",
            "align-content-center"
          )}
        >
          <FontAwesomeIcon
            className="align-content-center text-sm align-content-center ml-1"
            icon={faSearch}
          />
        </div>
        <input
          {...rest}
          ref={inputRef}
          value={searchTerm}
          className={clsx(
            "w-full justify-start placeholder-gray-600",
            "my-auto font-semibold bg-transparent",
            "outline-none border-transparent",
            !capitalize && "lowercase"
          )}
          onChange={onInputChange}
        />
        {loader && <div className="flex mx-1 items-center">{loader}</div>}

        <StayledButton
          onClick={hanldeReset}
          className={clsx(
            searchTerm && searchTerm !== ""
              ? "visible"
              : "hidden group-focus:flex"
          )}
          variant="primarygray"
          iconLeft={faTimes}
          iconOnly
          type="button"
          size="tiny"
        />
      </form>
    </div>
  );
}
