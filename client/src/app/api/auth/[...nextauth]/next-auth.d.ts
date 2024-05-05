import { DefaultSession } from 'next-auth';

declare module 'next-auth' {
  interface Session {
    user: User & DefaultSession['user'];
  }

  interface User {
    id: string;
    username: string;
    role: string;
  }
}

declare module 'next-auth/jwt' {
  interface JWT {
    username: string;
    access_token?: string;
  }
}
