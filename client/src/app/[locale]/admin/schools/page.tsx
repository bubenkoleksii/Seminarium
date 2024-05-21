'use client';

import { FC } from 'react';
import { Schools } from '@/features/admin';
import { SessionProvider } from 'next-auth/react';

type Props = {
  searchParams: {
    region: string;
    sortByDateAsc: string;
    schoolName: string;
    take: string;
    page: string;
  };
};

const SchoolsPage: FC<Props> = ({ searchParams }) => {
  return (
    <div className="p-3">
      <SessionProvider>
        <Schools
          regionParameter={searchParams.region}
          sortByDateAscParameter={searchParams.sortByDateAsc}
          searchParameter={searchParams.schoolName}
          limitParameter={searchParams.take ? Number(searchParams.take) : null}
          pageParameter={searchParams.page ? Number(searchParams.page) : null}
        />
      </SessionProvider>
    </div>
  );
};

export default SchoolsPage;
