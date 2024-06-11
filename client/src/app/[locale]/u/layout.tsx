'use client';

import { UserSidebar } from '@/features/user';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { SessionProvider } from 'next-auth/react';
import { PropsWithChildren } from 'react';
import ScrollToTop from 'react-scroll-to-top';

const queryClient = new QueryClient();

export default function AdminLayout({ children }: PropsWithChildren) {
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
