import { getServerSession } from 'next-auth';
import { authOptions } from '@/app/api/auth/[...nextauth]/route';

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
