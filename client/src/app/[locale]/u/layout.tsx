'use client';

import { PropsWithChildren } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import ScrollToTop from 'react-scroll-to-top';
import { SessionProvider } from 'next-auth/react';
import { UserSidebar } from '@/features/user';

export default function AdminLayout({ children }: PropsWithChildren) {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <SessionProvider>
        <div className="flex w-screen">
          <UserSidebar />
          <div className="w-[100%]">{children}</div>
        </div>

        <ScrollToTop
          style={{ backgroundColor: '#3b0764' }}
          color="white"
          smooth
          className="flex items-center justify-center rounded-full
                   shadow-lg"
        />
      </SessionProvider>
    </QueryClientProvider>
  );
}
