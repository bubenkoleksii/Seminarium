export interface CurrentUser {
  id: string;
  role: string;
  name?: string | null;
  username: string;
  img?: string | null;
  email?: string | null;
}
