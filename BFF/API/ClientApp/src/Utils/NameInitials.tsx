export default function NameInitials(
  firstname: string | null | undefined,
  lastname: string | null | undefined = undefined
): string {
  return (firstname ? firstname[0] : "") + (lastname ? lastname[0] : "");
}
