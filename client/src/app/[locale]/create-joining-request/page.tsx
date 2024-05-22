'use client';

import { JoiningRequestForm } from '@/features/joining-request';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import ScrollToTop from 'react-scroll-to-top';

export default function CreateJoiningRequestPage() {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <JoiningRequestForm />

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
