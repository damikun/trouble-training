export type IsLoadingProps = {
  isloading: boolean;
};
export default function IsLoading({ isloading }: IsLoadingProps) {
  return (
    <div
      className={
        "flex w-full mx-auto text-center justify-center my-2 pb-2 font-semibold text-blue-500 " +
        (isloading ? "" : "invisible")
      }
    >
      Loading more...
    </div>
  );
}
