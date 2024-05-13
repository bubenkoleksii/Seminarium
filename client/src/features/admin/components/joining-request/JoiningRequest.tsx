'use client';

import { FC } from 'react';
import type { ApiResponse } from '@/shared/types';
import type { JoiningRequestResponse } from '@/features/admin/types/joiningRequestTypes';
import { useTranslations } from 'next-intl';
import { useQuery } from '@tanstack/react-query';
import { getOne } from '@/features/admin/api/joiningRequestApi';
import { Loader } from '@/components/loader';
import { Error } from '@/components/error';
import { useSetCurrentTab } from '@/shared/hooks';
import { CurrentTab } from '@/features/admin/constants';
import { getColorByStatus } from '@/shared/helpers';

interface JoiningRequestProps {
  id: string;
}

const JoiningRequest: FC<JoiningRequestProps> = ({ id }) => {
  const t = useTranslations('JoiningRequest');

  const { data, isLoading } = useQuery<ApiResponse<JoiningRequestResponse>>({
    queryKey: ['joiningRequest', id],
    queryFn: () => getOne(id),
  });

  useSetCurrentTab(CurrentTab.JoiningRequest);

  if (data && data.error) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('oneTitle')}</h2>
        <Error error={data.error} />
      </>
    );
  }

  if (isLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('oneTitle')}</h2>
        <Loader />
      </>
    );
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  }

  const statusColor = getColorByStatus(data.status);

  return (
    <div className="p-3">
      <h2 className="mb-4 text-center text-xl font-bold">{t('oneTitle')}</h2>

      <h6 className="py-2 text-center font-bold">
        <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
          {t('labels.name')}
        </p>
        {data.name}
      </h6>

      <h6 className="py-2 text-center font-bold">
        <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
          {t('labels.detailInfo')}
        </p>
      </h6>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.status.label')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span className={`${statusColor} font-bold`}>
            {t(`labels.status.${data.status}`)}
          </span>
        </div>
      </div>
    </div>
  );
};

export { JoiningRequest };
