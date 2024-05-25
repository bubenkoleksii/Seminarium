import NextAuth, { type NextAuthOptions, Session } from 'next-auth';
import DuendeIdentityServer6 from 'next-auth/providers/duende-identity-server6';
import jwt from 'jsonwebtoken';
import axios from 'axios';
import { type JWT } from 'next-auth/jwt';

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
    async jwt({ token, profile, account }) {
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
  events: {
    async signOut({ session, token }) {
      await axios.get(process.env.AUTH_ISSUER_LOGOT);

      token = {} as JWT;
      session = {} as Session;
    },
  },
};

const handler = NextAuth(authOptions);

export { handler as GET, handler as POST };
