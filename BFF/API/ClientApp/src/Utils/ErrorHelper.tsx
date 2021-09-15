import { ToastContextType } from "../UIComponents/Toast/ToastProvider";

export function HandleErrors(
  toast: ToastContextType | undefined,
  errors:
    | ReadonlyArray<{
        readonly message?: string | null;
      } | null>
    | null
    | undefined
) {
  if (errors) {
    errors?.forEach((element) => {
      if (element?.message) {
        toast?.pushError(element?.message);
      }
    });
  }
}
