import type { NextApiRequest } from "next";
import { getServerSession } from 'next-auth';
import { authOptions } from '@/app/api/auth/[...nextauth]/route';
import { getToken } from 'next-auth/jwt';
import { cookies, headers } from 'next/headers';

export const getSession = async () => {
  return await getServerSession(authOptions as any);
};

export const getCurrentUser = async () => {
  try {
    const session = await getSession();

    if (!session) return null;

    return session.user;
  } catch {
    return null;
  }
};

export const getCurrentUserToken = async () => {
  const req = {
    headers: Object.fromEntries(headers() as Headers),
    cookies: Object.fromEntries(
      cookies()
        .getAll()
        .map(c => [c.name, c.value])
    )
  } as NextApiRequest;

  return await getToken({ req });
};
