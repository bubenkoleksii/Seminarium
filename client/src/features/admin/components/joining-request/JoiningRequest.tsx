'use client'

import { FC } from 'react';
import { useTranslations } from 'next-intl';
import { useQuery } from '@tanstack/react-query';
import { getOne } from '@/features/admin/api/joiningRequestApi';
import { Loader } from '@/components/loader';
import { PagedJoiningRequests } from '@/features/admin/types/joiningRequestTypes';
import { ApiError } from '@/shared/types';

interface JoiningRequestProps {
  id: string;
}

const JoiningRequest: FC<JoiningRequestProps> = ({ id }) => {
  const t = useTranslations('JoiningRequest');

  const { data, isLoading, error, isError, status } = useQuery({
    queryKey: ['joiningRequest', id],
    queryFn: () => getOne(id),
  });

  if (isLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('listTitle')}</h2>
        <Loader />
      </>
    );
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  }

  return (
    <div className="p-3">
      <h2 className="mb-4 text-center text-xl font-bold">{t('oneTitle')}</h2>
      {data && data.id}
    </div>
  );
};

export { JoiningRequest };
