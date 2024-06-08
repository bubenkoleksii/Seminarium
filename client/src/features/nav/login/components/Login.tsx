'use client';

import { FC } from 'react';
import { LoginButton } from './LoginButton';
import { useSession } from 'next-auth/react';
import { CurrentUserPopover } from '@/features/nav/login/components/CurrentUserPopover';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const Login: FC = () => {
  const { status, data } = useSession();
  const currentUser = data?.user;

  if (status === 'loading') {
    return null;
  }

  const queryClient = new QueryClient();

  return (
    <>
      <QueryClientProvider client={queryClient}>
        {status === 'unauthenticated' ? (
          <LoginButton />
        ) : (
          currentUser && <CurrentUserPopover user={currentUser} />
        )}
      </QueryClientProvider>
    </>
  );
};

export { Login };
