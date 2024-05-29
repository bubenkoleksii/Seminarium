'use client';

import type { PropsWithChildren } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import ScrollToTop from 'react-scroll-to-top';
import { SessionProvider } from 'next-auth/react';

export default function SchoolProfileLayout({ children }: PropsWithChildren) {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <SessionProvider>
        {children}

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