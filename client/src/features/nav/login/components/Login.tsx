'use client'

import { FC } from 'react';
import { LoginButton } from './LoginButton';
import { useSession } from 'next-auth/react';
import { CurrentUserPopover } from '@/features/nav/login/components/CurrentUserPopover';

const Login: FC = () => {
  const { status, data } = useSession();
  const currentUser = data?.user;

  if (status === 'loading') {
    return null;
  }

  return (
    <>
      {status === 'unauthenticated'
        ? <LoginButton />
        : currentUser && <CurrentUserPopover user={currentUser} />
      }
    </>
  );
};

export { Login };
