'use client';

import { JoiningRequestForm } from '@/features/joining-request';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

export default function CreateJoiningRequestPage() {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <JoiningRequestForm />
    </QueryClientProvider>
  );
}
