'use client';

import { JoiningRequests } from '@/features/admin';
import { SessionProvider } from 'next-auth/react';

type Props = {
  searchParams: {
    region: string;
    sortByDateAsc: string;
    schoolName: string;
    status: string;
    take: string;
    page: string;
  };
};

export default function JoiningRequestsPage({ searchParams }: Props) {
  return (
    <div className="p-3">
      <SessionProvider>
        <JoiningRequests
          sortByDateAscParameter={searchParams.sortByDateAsc}
          regionParameter={searchParams.region}
          searchParameter={searchParams.schoolName}
          statusParameter={searchParams.status}
          limitParameter={searchParams.take ? Number(searchParams.take) : null}
          pageParameter={searchParams.page ? Number(searchParams.page) : null}
        />
      </SessionProvider>
    </div>
  );
}
