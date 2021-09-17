import React, {
  useState,
  useRef,
  useCallback,
  useEffect,
  useMemo,
} from "react";
import CapitalizedText, {
  Capitalizer,
} from "../../UIComponents/CapitalizedText/CapitalizedText";
import "react-quill/dist/quill.snow.css";
import DebouncingInput from "../../UIComponents/Inputs/DebouncingInput";
import useOnClickOutside from "../../Hooks/useOnOutsideElementClick";
import StyledSpinner from "../../UIComponents/Spinner/Spinner";
import clsx from "clsx";
import StayledLabel from "../Label/StayledLabel";

export type EditableInputProps = {
  name: string | undefined;
  onCommit: (value: string | undefined) => void;
  inFlight: boolean;
  leftPadding?: number;
  placeholder?: string;
  className?: string;
  disabled?: boolean;
  border?: boolean;
  autoFocus?: boolean;
  label?: string;
  tagVariant?: "div" | "h1" | "h2";
  capitalize?: boolean;
  labelstyle?: string;
  height?: string;
  type?: string | undefined;
  float?: "right" | "left";
};

type internallStateType = {
  name: string | undefined;
  editing: boolean;
};

export default function EditableInput({
  name,
  onCommit,
  inFlight,
  leftPadding = 0,
  placeholder,
  className,
  disabled = false,
  border = false,
  tagVariant,
  autoFocus = true,
  labelstyle,
  capitalize = true,
  label,
  height = "h-10",
  type,
  float = "left",
}: EditableInputProps) {
  const [internalState, setInternalState] = useState<internallStateType>({
    name: name,
    editing: false,
  });

  const $divRef = useRef<HTMLInputElement>(null);

  const handleNameValueChange = useCallback(
    (event: React.ChangeEvent<HTMLInputElement>) => {
      setInternalState((prevState) => {
        return { name: event.target.value, editing: prevState.editing };
      });
    },
    []
  );

  const handleSave = useCallback(() => {
    setInternalState((prevState) => {
      return { name: prevState.name, editing: false };
    });

    if (name !== internalState.name) {
      onCommit(internalState.name);
    }
  }, [onCommit, internalState.name, name]);

  useOnClickOutside($divRef, () => internalState.editing && handleSave());

  useEffect(() => {
    setInternalState((prevState) => {
      return { name: name, editing: prevState.editing };
    });
  }, [name]);

  const name_valid = name && name !== null && name !== "" && name !== undefined;

  const handleKeyPress = useCallback(
    (event: React.KeyboardEvent<HTMLInputElement>) => {
      if (event.key === "Enter") {
        event.preventDefault();
        handleSave();
      }
    },
    [handleSave]
  );

  const handlePreventDefault = useCallback(
    (event: React.FormEvent<HTMLInputElement>) => {
      event.preventDefault();
    },
    []
  );

  const handleStartEditing = useCallback(() => {
    !internalState.editing &&
      !inFlight &&
      !disabled &&
      setInternalState((prevState) => {
        return { name: name, editing: true };
      });
  }, [internalState.editing, inFlight, disabled, name]);

  const memorisedStyle = useMemo(() => {
    return { paddingLeft: leftPadding ? leftPadding : 0 };
  }, [leftPadding]);

  return (
    <div
      className={clsx(
        "flex-1 w-full max-w-full",
        !disabled ? "cursor-pointer" : "cursor-not-allowed"
      )}
      ref={$divRef}
    >
      {!internalState.editing ? (
        <div className="flex flex-col flex-no-wrap">
          {label && (
            <StayledLabel
              className={clsx(
                "flex",
                labelstyle
                  ? labelstyle
                  : clsx(
                      "text-sm sm:text-base",
                      "align-items-center pb-2 flex-no-wrap",
                      "flex-grow-0 break-normal"
                    )
              )}
            >
              <CapitalizedText>{label}</CapitalizedText>
            </StayledLabel>
          )}
          <div
            style={memorisedStyle}
            onClick={handleStartEditing}
            className={clsx(
              "flex flex-row items-center my-auto",
              "hover:bg-gray-100 rounded-md flex-no-wrap",
              border &&
                clsx(
                  "hover:border-blue-500 border-gray-100",
                  "border-2 transition duration-200"
                ),
              className,
              height
            )}
          >
            <CapitalizedText
              tagVariant={tagVariant}
              enable={capitalize}
              className={clsx(
                "truncate-2-lines w-full",
                !name_valid && "text-gray-400",
                border && "ml-3",
                float === "right" && " text-right"
              )}
            >
              {name_valid ? name : placeholder}
            </CapitalizedText>
            {inFlight && (
              <div className="flex mr-1">
                <StyledSpinner size="small" />
              </div>
            )}
          </div>
        </div>
      ) : (
        <DebouncingInput
          autoFocus={autoFocus}
          label={label}
          capitalize={capitalize}
          labelstyle={labelstyle}
          className={clsx(
            `border-2 w-full mr-1 
            rounded-md
            border-blue-400"
            focus:bg-white 
            focus-within:border-blue-500 
            hover:border-blue-500
            text-xs md:text-sm min-w-5rem`,
            height,
            className
          )}
          float={float}
          onSubmit={handlePreventDefault}
          onChange={handleNameValueChange}
          onKeyPress={handleKeyPress}
          disabled={!internalState.editing || inFlight || disabled}
          value={Capitalizer(internalState.name)!}
          placeholder={placeholder ? placeholder : "Enter name"}
          type={type}
        />
      )}
    </div>
  );
}
