import NextAuth, { type NextAuthOptions } from 'next-auth';
import DuendeIdentityServer6 from 'next-auth/providers/duende-identity-server6';

export const authOptions: NextAuthOptions = {
  session: {
    strategy: 'jwt',
  },
  providers: [
    DuendeIdentityServer6({
      id: 'id-server',
      clientId: process.env.AUTH_CLIENT_ID,
      clientSecret: process.env.AUTH_CLIENT_SECRET,
      issuer: process.env.AUTH_ISSUER,
      authorization: {
        params: {
          scope: 'openid profile SeminariumApp'
        }
      },
      idToken: true,
    }),
  ],
};

const handler = NextAuth(authOptions);

export { handler as GET, handler as POST }
