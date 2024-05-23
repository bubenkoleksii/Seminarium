'use client';

import { FC } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { SessionProvider } from 'next-auth/react';
import ScrollToTop from 'react-scroll-to-top';
import { UpdateSchoolForm } from '@/features/school';
import type { UpdateSchoolRequest } from '@/features/school/types';

type UpdateSchoolPageProps = {
  params: {
    id: string;
  }
  searchParams: UpdateSchoolRequest
}

const UpdateSchoolPage: FC<UpdateSchoolPageProps> = ({ params, searchParams }) => {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <SessionProvider>
        <UpdateSchoolForm school={searchParams} id={params.id} />

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

export default UpdateSchoolPage;
