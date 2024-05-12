'use client'

import { FC } from 'react';
import { useTranslations } from 'next-intl';
import { useQuery } from '@tanstack/react-query';
import { getOne } from '@/features/admin/api/joiningRequestApi';
import { Loader } from '@/components/loader';
import { Error } from '@/components/error';
import { useSetCurrentTab } from '@/features/admin/hooks';
import { CurrentTab } from '@/features/admin/constants';

interface JoiningRequestProps {
  id: string;
}

const JoiningRequest: FC<JoiningRequestProps> = ({ id }) => {
  const t = useTranslations('JoiningRequest');

  const { data, isLoading } = useQuery({
    queryKey: ['joiningRequest', id],
    queryFn: () => getOne(id),
  });

  useSetCurrentTab(CurrentTab.JoiningRequest);

  if (data && data.error) {
    return <Error error={data.error} />
  }

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
