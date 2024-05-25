'use client';

import { FC } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { SessionProvider } from 'next-auth/react';
import { School } from '@/features/school';
import ScrollToTop from 'react-scroll-to-top';

type Props = {
  params: {
    id: string;
  };
};

const SchoolPage: FC<Props> = ({ params }) => {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <SessionProvider>
        <School id={params.id} />

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
};

export default SchoolPage;
