import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { faAngleDown, faTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import clsx from "clsx";
import { motion } from "framer-motion";
import React, { useCallback, useMemo } from "react";
import CapitalizedText from "../CapitalizedText/CapitalizedText";
import DropDownMenu, {
  ORIENTATION_VARIANTS,
  POSITION_VARIANTS,
  useDropDownContext,
} from "./DropDownMenu";

const STYLE_VARIANTS = {
  purple:
    "text-white bg-purple-400 hover:bg-purple-500 focus:ring-purple-500 focus-within:ring-purple-500 font-semibold ring-transparent focus:ring-2 focus-within:ring-2 border-transparent border-2 focus:border-white focus-within:border-white",
  yellow:
    "text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-yellow-500 focus-within:ring-yellow-500  font-semibold ring-transparent focus:ring-2 focus-within:ring-2 border-transparent border-2 focus:border-white focus-within:border-white",
  blue:
    "text-white bg-blue-500 hover:bg-blue-600 focus:ring-blue-500 focus-within:ring-blue-500 focus:ring-2 focus-within:ring-2 font-semibold ring-transparent border-transparent border-2 focus:border-white focus-within:border-white",
  gray:
    "text-gray-600 bg-gray-200 hover:bg-gray-300 focus:ring-gray-400 focus-within:ring-gray-400 font-semibold focus:ring-2 focus-within:ring-2 ring-transparent border-transparent border-2 focus:border-white focus-within:border-white",
  lightgray:
    "text-gray-700 bg-gray-100 hover:bg-gray-200 focus:ring-gray-200 focus-within:ring-gray-200 font-semibold focus:ring-2 focus-within:ring-2 ring-transparent border-transparent border-2 focus:border-white focus-within:border-white",
  transparent:
    "text-gray-700 bg-transparent font-semibold ring-transparent border-transparent",
  white:
    "text-gray-700 bg-white hover:bg-gray-50 focus:ring-white focus-within:ring-white font-semibold focus:ring-2 focus-within:ring-2 ring-transparent border-transparent border-2 focus:border-gray-800 focus-within:border-gray-800",
};

const ROUNDING = {
  none: "rounded-none",
  small: "rounded-sm",
  normal: "rounded",
  medium: "rounded-md",
  large: "rounded-lg",
  extra: "rounded-xl",
  full: "rounded-full",
};

export type DropDownDataItem = {
  value?: any;
  content?: React.ReactNode | null | undefined | string;
  iconLeft?: IconProp;
  iconRight?: IconProp;
  style?: String;
  styleOption?: String;
  styleVariant?: keyof typeof STYLE_VARIANTS;
};

export const SIZE_VARIANTS = {
  small: {
    size: "h-6",
  },
  medium: {
    size: "h-8",
  },
  big: {
    size: "h-10",
  },
  auto: {
    size: "",
  },
};

export type StayledDropDownMenuProps = {
  data?: Array<DropDownDataItem> | undefined;
  selected?: DropDownDataItem;
  init: any;
  preventCloseOnClick?: boolean;
  onSelect?: (value: any) => void;
  enabled?: boolean;
  iconPosition?: "left" | "right" | "none";
  icon?: IconProp | null;
  onClickBehaviour?: "open" | "open-close";
  children?: React.ReactNode;
  deletable?: boolean;
  isError?: boolean;
  padding?: boolean;
  rounding?: keyof typeof ROUNDING;
  variant?: keyof typeof STYLE_VARIANTS;
  position?: keyof typeof POSITION_VARIANTS;
  orientation?: keyof typeof ORIENTATION_VARIANTS;
  size?: keyof typeof SIZE_VARIANTS;
  maxHeight?: "max-h-96" | "max-h-132" | "max-h-80" | "max-h-60";
};

export default React.memo(StayledDropDownMenu);

function StayledDropDownMenu({
  data,
  selected,
  onSelect,
  init,
  onClickBehaviour = "open-close",
  preventCloseOnClick,
  children,
  variant = "blue",
  isError,
  icon = faAngleDown,
  padding = true,
  rounding = "large",
  iconPosition = "right",
  maxHeight,
  position,
  orientation,
  enabled = false,
  deletable = true,
  size = "auto",
}: StayledDropDownMenuProps) {
  const onItemSelected = useCallback(
    (value: any) => {
      onSelect && onSelect(value);
    },
    [onSelect]
  );

  const onDelete = useCallback(() => {
    onSelect && onSelect("");
  }, [onSelect]);

  const options = useMemo(() => data?.filter((e) => e !== selected), [
    selected,
    data,
  ]);

  return (
    <DropDownMenu
      preventCloseOnClick={preventCloseOnClick}
      enabled={enabled}
      position={position}
      orientation={orientation}
      menu={
        <div
          className={clsx(
            "flex border",
            "border-gray-300 bg-white shadow-lg",
            "text-gray-800 h-full rounded-lg"
          )}
        >
          {children ? (
            <div
              className={clsx(
                "flex flex-col py-2 items-center min-w-24",
                "overflow-hidden overflow-y-auto px-0 m-0",
                "scrollbarhide scrollbarhide2 scrollbarwidth",
                maxHeight
              )}
            >
              {children}
            </div>
          ) : (
            <motion.ul
              className={clsx(
                "flex flex-col py-2 items-center min-w-24",
                "overflow-hidden overflow-y-auto px-0 m-0",
                "scrollbarhide scrollbarhide2 scrollbarwidth",
                maxHeight
              )}
            >
              {options?.length !== 0 ? (
                options?.map((enity, index) => {
                  if (enity && data) {
                    return (
                      <MemorisedRenderSelectTypeOption
                        id={data?.indexOf(enity)}
                        key={index}
                        value={enity}
                        OnClick={onItemSelected}
                      />
                    );
                  }
                  return null;
                })
              ) : (
                <MemorisedRenderSelectTypeOption
                  id={"norec"}
                  key={"norec"}
                  value={{ value: "", content: "No-Options" }}
                  OnClick={onItemSelected}
                />
              )}
            </motion.ul>
          )}
        </div>
      }
    >
      <div className="font-semibold items-center flex">
        {selected && (selected.value || selected.content) && selected !== "" ? (
          <MemorisedRenderSelected
            padding={padding}
            onClickBehaviour={onClickBehaviour}
            iconPosition={iconPosition}
            icon={icon}
            isError={isError}
            deletable={deletable}
            variant={variant}
            rounding={rounding}
            value={selected}
            OnDelete={onDelete}
            size={size}
          />
        ) : (
          <MemorisedRenderSelectRequest
            padding={padding}
            icon={icon}
            onClickBehaviour={onClickBehaviour}
            iconPosition={iconPosition}
            isError={isError}
            size={size}
            rounding={rounding}
            variant={variant}
            name={init}
          />
        )}
      </div>
    </DropDownMenu>
  );
}

const default_style = `text-white bg-blue-500 hover:bg-blue-600 
  focus:ring-blue-500 focus:ring-2 focus:border-white 
  focus-within:ring-blue-500 focus-within:ring-2 focus-within:border-white 
  font-semibold ring-transparent 
  border-transparent border-2 `;

/////////////////////////////
// Helpers subcomponents
/////////////////////////////

type RenderSelectRequestProps = {
  name: string;
  padding: boolean;
  size: keyof typeof SIZE_VARIANTS;
  iconPosition?: "left" | "right" | "none";
  isError?: boolean;
  icon?: IconProp | null;
  onClickBehaviour?: "open" | "open-close";
  variant?: keyof typeof STYLE_VARIANTS;
  rounding?: keyof typeof ROUNDING;
};

const MemorisedRenderSelectRequest = React.memo(RenderSelectRequest);

function RenderSelectRequest({
  name,
  size,
  padding = true,
  iconPosition = "right",
  isError,
  variant,
  onClickBehaviour,
  icon = faAngleDown,
  rounding = "large",
}: RenderSelectRequestProps) {
  const dropDown = useDropDownContext();

  const rotate: number = useMemo(() => (dropDown.isOpen ? 180 : 0), [
    dropDown.isOpen,
  ]);

  if (!iconPosition) {
    iconPosition = "right";
  }

  const Rounding_Style = rounding ? ROUNDING[rounding] : ROUNDING.large;

  const dropdownCtx = useDropDownContext();

  const handleClick = useCallback(
    (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
      event.preventDefault();
      event.stopPropagation();

      if (onClickBehaviour === "open-close") {
        dropdownCtx.setVisibility((e) => !e);
      } else {
        dropdownCtx.setVisibility(true);
      }
    },

    [dropdownCtx, dropdownCtx.setVisibility]
  );

  const MemorisedIcon = useMemo(
    () => (
      <>
        {icon !== null && (
          <motion.div
            animate={{ rotate: rotate }}
            className={clsx(
              "leading-none font-semibold",
              "py-0.5 px-1 items-center"
            )}
          >
            <FontAwesomeIcon icon={icon} />
          </motion.div>
        )}
      </>
    ),
    [rotate]
  );

  return (
    <button
      onClick={handleClick}
      type="button"
      className={clsx(
        "flex flex-row w-full items-center flex-nowrap",
        "select-none outline-none focus:outline-none",
        "justify-end space-x-1 transition duration-200",
        "space-x-1",
        padding ? "p-1 px-2" : "p-0.5 px-0.5",
        Rounding_Style,
        SIZE_VARIANTS[size].size,
        variant ? STYLE_VARIANTS[variant] : default_style,
        dropDown.disabled ? "cursor-not-allowed" : "cursor-pointer",
        isError && "ring-red-500 ring-2 border-2 border-white"
      )}
    >
      {!dropDown.disabled && iconPosition === "left" && MemorisedIcon}
      <CapitalizedText
        className={clsx(
          "flex items-center tracking-wide",
          "truncate-1-lines select-none"
        )}
      >
        {name}
      </CapitalizedText>
      {!dropDown.disabled && iconPosition === "right" && MemorisedIcon}
    </button>
  );
}

///////////////////////////////

type RenderSelectTypeOptionProps = {
  value: DropDownDataItem;
  OnClick: (value: any) => void;
  id: number | string;
};

const MemorisedRenderSelectTypeOption = React.memo(RenderSelectTypeOption);

function RenderSelectTypeOption({
  value,
  OnClick,
  id,
}: RenderSelectTypeOptionProps) {
  const context = useDropDownContext();

  const handleKeyPress = useCallback(
    (e: React.KeyboardEvent<HTMLLIElement>) => {
      const { key } = e;

      e.preventDefault();
      e.stopPropagation();

      if (key === "Enter") {
        OnClick(value.value);
        context.close();
      }
    },
    []
  );

  const handleClick = useCallback(() => {
    OnClick(value.value);
  }, [OnClick, value.value]);

  return (
    <li
      data-key={id}
      tabIndex={0}
      className={clsx(
        "flex flex-row justify-between w-full py-1",
        "cursor-pointer px-2 items-center space-x-2",
        "focus:outline-none list-none",
        value.styleOption
          ? value.styleOption
          : "hover:bg-gray-200 focus:bg-gray-200 "
      )}
      onClick={handleClick}
      onKeyPress={handleKeyPress}
    >
      <>
        {value.iconLeft && (
          <FontAwesomeIcon
            className="align-content-center align-content-center"
            icon={value.iconLeft}
          />
        )}

        <CapitalizedText
          className={clsx(
            "flex items-center tracking-wide whitespace-nowrap",
            "truncate-1-lines select-none flex-nowrap"
          )}
        >
          {value.content ? value.content : value.value}
        </CapitalizedText>

        {value.iconRight && !value.iconLeft && (
          <FontAwesomeIcon
            className="align-content-center align-content-center"
            icon={value.iconRight}
          />
        )}
      </>
    </li>
  );
}

/////////////////////////////////

type RenderSelectedProps = {
  value: DropDownDataItem;
  deletable?: boolean;
  OnDelete: () => void;
  iconPosition?: "left" | "right" | "none";
  variant?: keyof typeof STYLE_VARIANTS;
  children?: React.ReactNode;
  padding: boolean;
  onClickBehaviour?: "open" | "open-close";
  icon?: IconProp | null;
  rounding?: keyof typeof ROUNDING;
  size: keyof typeof SIZE_VARIANTS;
  isError?: boolean;
};

const MemorisedRenderSelected = React.memo(RenderSelected);

function RenderSelected({
  value,
  OnDelete,
  deletable = true,
  size,
  onClickBehaviour,
  icon = faAngleDown,
  padding = true,
  rounding = "large",
  iconPosition = "right",
  variant,
  isError,
}: RenderSelectedProps) {
  const dropDown = useDropDownContext();

  const rotate = useMemo(() => (dropDown.isOpen ? 180 : 0), [dropDown.isOpen]);

  const style = useMemo(
    () => (value.style ? value.style : value.styleVariant),
    [value.style, value.styleVariant]
  );

  const handleDelete = useCallback(
    (e) => {
      e.preventDefault();
      e.stopPropagation();
      OnDelete();
    },
    [OnDelete]
  );

  if (!iconPosition) {
    iconPosition = "right";
  }

  const dropdownCtx = useDropDownContext();

  const Rounding_Style = rounding ? ROUNDING[rounding] : ROUNDING.large;

  const handleClick = useCallback(
    (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
      event.preventDefault();
      event.stopPropagation();

      if (onClickBehaviour === "open-close") {
        dropdownCtx.setVisibility((e) => !e);
      } else {
        dropdownCtx.setVisibility(true);
      }
    },

    [dropdownCtx, dropdownCtx.setVisibility]
  );

  return (
    <button
      onClick={handleClick}
      type="button"
      className={clsx(
        "flex flex-row w-full items-center flex-nowrap",
        "select-none outline-none focus:outline-none",
        "justify-end space-x-1 transition duration-200",
        "space-x-1",
        padding ? "p-1 px-2" : "p-0.5 px-0.5",
        Rounding_Style,
        SIZE_VARIANTS[size].size,
        style ? style : variant ? STYLE_VARIANTS[variant] : default_style,
        dropDown.disabled ? "cursor-not-allowed" : "cursor-pointer",
        isError && "ring-red-500 ring-2 border-2 border-white"
      )}
    >
      {!dropDown.disabled && iconPosition === "left" && icon !== null && (
        <motion.div
          animate={{ rotate: rotate }}
          className={clsx(
            "leading-none font-semibold",
            "py-0.5 px-1 items-center"
          )}
        >
          <FontAwesomeIcon icon={icon} />
        </motion.div>
      )}

      {value.iconLeft && (
        <FontAwesomeIcon
          className="align-content-center align-content-center"
          icon={value.iconLeft}
        />
      )}

      <CapitalizedText
        className={clsx(
          "flex items-center tracking-wide break-all truncate",
          "truncate-1-lines select-none flex-nowrap whitespace-nowrap"
        )}
      >
        {value.content ? value.content : value.value}
      </CapitalizedText>

      {value.iconRight && !value.iconLeft && (
        <FontAwesomeIcon
          className="align-content-center align-content-center"
          icon={value.iconRight}
        />
      )}

      {deletable && !dropDown.disabled && (
        <div
          onClick={handleDelete}
          className={clsx(
            "hover:bg-red-500 hover:text-white py-0.5 px-1",
            "rounded-md items-center",
            "text-center font-semibold leading-none",
            "transition duration-150"
          )}
        >
          <FontAwesomeIcon className="align-content-center" icon={faTimes} />
        </div>
      )}

      {!dropDown.disabled && iconPosition === "right" && icon !== null && (
        <motion.div
          animate={{ rotate: rotate }}
          className={clsx(
            "leading-none font-semibold",
            "py-0.5 px-1 items-center"
          )}
        >
          <FontAwesomeIcon icon={icon} />
        </motion.div>
      )}
    </button>
  );
}
