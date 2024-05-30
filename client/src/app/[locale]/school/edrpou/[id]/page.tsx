'use client';

import { FC } from 'react';
import { SessionProvider } from 'next-auth/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { InRegisterSchool } from '@/features/school';
import ScrollToTop from 'react-scroll-to-top';

type Props = {
  params: {
    id: string;
  };
};

const InRegisterSchoolPage: FC<Props> = ({ params }) => {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <SessionProvider>
        <InRegisterSchool id={params.id} />

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

export default InRegisterSchoolPage;
