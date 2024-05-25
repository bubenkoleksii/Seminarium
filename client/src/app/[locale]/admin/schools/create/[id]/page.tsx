'use client';

import { FC } from 'react';
import type { CreateSchoolRequest } from '@/features/admin/types/schoolTypes';
import { SessionProvider } from 'next-auth/react';
import { CreateSchoolForm } from '@/features/admin';

type CreateSchoolPageProps = {
  params: {
    id: string;
  };
  searchParams: CreateSchoolRequest;
};

const CreateSchoolPage: FC<CreateSchoolPageProps> = ({
  params,
  searchParams,
}) => {
  return (
    <div className="min-w-screen flex justify-center p-3">
      <SessionProvider>
        <CreateSchoolForm joiningRequestId={params.id} school={searchParams} />
      </SessionProvider>
    </div>
  );
};

export default CreateSchoolPage;
