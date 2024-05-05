import NextAuth, { type NextAuthOptions } from 'next-auth';
import DuendeIdentityServer6 from 'next-auth/providers/duende-identity-server6';
import jwt from 'jsonwebtoken';

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
          scope: 'openid profile SeminariumApp',
        },
      },
      idToken: true,
    }),
  ],
  callbacks: {
    async jwt({ token, profile, account, user }) {
      if (profile) {
        token.username = profile['preferred_username'];
      }
      if (account) {
        token.access_token = account.access_token;
      }

      const decodedToken = jwt.decode(token.access_token);
      token.role = decodedToken.role;
      token.id = decodedToken.sub;

      return token;
    },
    async session({ session, token }) {
      if (token) {
        session.user.id = token.id;
        session.user.role = token.role;
        session.user.username = token.username;
        session.user.email = token.username;
      }

      return session;
    },
  },
};

const handler = NextAuth(authOptions);

export { handler as GET, handler as POST };
