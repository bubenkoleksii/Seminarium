'use client';

import type { PropsWithChildren } from 'react';
import { AdminSidebar } from '@/features/admin';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import ScrollToTop from 'react-scroll-to-top';

export default function AdminLayout({ children }: PropsWithChildren) {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: 1000 * 10,
      },
    },
  });

  return (
    <QueryClientProvider client={queryClient}>
      <div className="flex w-screen">
        <AdminSidebar />
        <div className="w-[100%]">{children}</div>
      </div>

      <ScrollToTop
        style={{ backgroundColor: '#3b0764' }}
        color="white"
        smooth
        className="flex items-center justify-center rounded-full
                   shadow-lg"
      />
    </QueryClientProvider>
  );
}
