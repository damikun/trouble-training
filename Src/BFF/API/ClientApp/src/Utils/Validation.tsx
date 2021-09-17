export const is = {
  match: (testFn: Function, message = "") => (value: any, fieldValues: any) =>
    !testFn(value, fieldValues) && message,

  required: (message: string = "Field is required") => (value: any) =>
    isNullOrEmptyString(value) && message,

  minLength: (
    min: number,
    message: string = `Must have min ${min} characters`
  ) => (value: any) => value && value.length < min && message,
  maxLength: (
    max: number,
    message: string = `Must have max ${max} characters`
  ) => (value: any) => value && value.length > max && message,

  email: () => (value: any, message: string = "Is not valid email") =>
    !!value && !/.+@.+\..+/.test(value) && message,

  notEmptyArray: () => (
    value: any,
    message: string = "Add at least one item"
  ) => Array.isArray(value) && value.length === 0 && message,

  isString: () => (value: any) =>
    typeof value === "string" || value instanceof String,
};

const isNullOrEmptyString = (value: any) =>
  value === undefined || value === null || value === "";

export const generateErrors = (fieldValues: any, fieldValidators: {}) => {
  const errors = {};

  Object.entries(fieldValidators).forEach(([fieldName, validators]) => {
    //@ts-ignore
    [validators].flat().forEach((validator: Function) => {
      const errorMessage = validator(fieldValues[fieldName], fieldValues);
      //@ts-ignore
      if (errorMessage && !errors[fieldName]) {
          //@ts-ignore
        errors[fieldName] = errorMessage;
      }
    });
  });
  return errors;
};
