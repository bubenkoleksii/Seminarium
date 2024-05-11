export type ApiError = {
  type: string;
  title: string;
  detail: string;
  status: number;
  message?: string;
  params?: string[];
}
