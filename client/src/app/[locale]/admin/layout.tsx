'use client'

import type { PropsWithChildren } from 'react';
import { AdminSidebar } from '@/features/admin';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

export default function AdminLayout({ children }: PropsWithChildren) {
  const queryClient = new QueryClient()

  return (
    <QueryClientProvider client={queryClient}>
      <div className="flex w-screen">
        <AdminSidebar />
        <div>
          {children}
        </div>
      </div>
    </QueryClientProvider>
  );
}
