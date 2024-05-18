'use client';

import { FC } from 'react';
import type { ApiResponse } from '@/shared/types';
import type { JoiningRequestResponse } from '@/features/admin/types/joiningRequestTypes';
import { useLocale, useTranslations } from 'next-intl';
import { useQuery } from '@tanstack/react-query';
import { getOne } from '@/features/admin/api/joiningRequestApi';
import { Loader } from '@/components/loader';
import { Error } from '@/components/error';
import { useSetCurrentTab } from '@/shared/hooks';
import { AdminClientPaths, adminQueries, CurrentTab } from '@/features/admin/constants';
import { getColorByStatus } from '@/shared/helpers';
import { DateTime } from '@/components/date-time';
import { Button } from 'flowbite-react';
import { useRouter } from 'next/navigation';

interface JoiningRequestProps {
  id: string;
}

const JoiningRequest: FC<JoiningRequestProps> = ({ id }) => {
  const t = useTranslations('JoiningRequest');
  const { replace } = useRouter();
  const activeLocale = useLocale();

  const { data, isLoading } = useQuery<ApiResponse<JoiningRequestResponse>>({
    queryKey: [adminQueries.getOneJoiningRequest, id],
    queryFn: () => getOne(id),
    enabled: !!id,
    retry: adminQueries.options.retry
  });

  useSetCurrentTab(CurrentTab.JoiningRequest);

  if (data && data.error) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('oneTitle')}
          <span onClick={() => replace(`/${activeLocale}/${AdminClientPaths.JoiningRequests}/`)}
                className="text-sm pt-1 ml-2 text-purple-700 cursor-pointer hover:text-red-700"
          >
              {t('labels.toMain')}
        </span>
        </h2>
        <Error error={data.error} />
      </>
    );
  }

  if (isLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('oneTitle')}
          <span onClick={() => replace(`/${activeLocale}/${AdminClientPaths.JoiningRequests}/`)}
                className="text-sm pt-1 ml-2 text-purple-700 cursor-pointer hover:text-red-700"
          >
              {t('labels.toMain')}
        </span>
        </h2>
        <Loader />
      </>
    );
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  }

  const statusColor = getColorByStatus(data.status);
  const occupiedColor = getColorByStatus(data.areOccupied ? 'danger' : 'ok');

  return (
    <div className="p-3">
      <h2 className="mb-4 text-center text-xl font-bold">
        {t('oneTitle')}
        <span onClick={() => replace(`/${activeLocale}/${AdminClientPaths.JoiningRequests}/`)}
              className="text-sm pt-1 ml-2 text-purple-700 cursor-pointer hover:text-red-700"
        >
              {t('labels.toMain')}
        </span>
      </h2>

      <h6 className="py-2 text-center font-bold">
        <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
          {t('labels.name')}
        </p>
        <span className="text-purple-950 lg:text-2xl">{data.name}</span>
      </h6>

      <h6 className="py-2 text-center font-bold">
        <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
          {t('labels.detailInfo')}
        </p>
      </h6>

      <div className="mt-2 flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.status.label')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span className={`${statusColor} font-bold`}>
            {t(`labels.status.${data.status}`)}
          </span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.id')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.id}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.createdAt')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>
            <DateTime date={data.createdAt} />
          </span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.registerCode')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.registerCode}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.requesterFullName')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.requesterFullName}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.requesterEmail')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.requesterEmail}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.requesterPhone')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.requesterPhone}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.shortName')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.shortName || '-'}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.gradingSystem')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.gradingSystem}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.type')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{t(`types.${data.type}`)}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.ownershipType')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{t(`ownershipTypes.${data.ownershipType}`)}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.postalCode')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.postalCode}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.studentsQuantity')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.studentsQuantity}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.areOccupied')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span className={occupiedColor}>
            {t(data.areOccupied ? 'labels.yes' : 'labels.no')}
          </span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.region')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>
            {data.region === 'none' ? '-' : t(`regions.${data.region}`)}
          </span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">
            {t('labels.territorialCommunity')}
          </span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.territorialCommunity || ''}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.address')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.address || ''}</span>
        </div>
      </div>

      <div className="flex pt-4 text-xs lg:text-lg justify-center">
        <div className="flex w-1/2 pr-2 w-[40%]">
          {data.status === 'created' &&
            <Button gradientMonochrome="failure" fullSized>
              <span className="text-white">{t('labels.rejectBtn')}</span>
            </Button>
          }
        </div>
        <div className="flex w-1/2 pl-2 w-[40%]">
          <Button gradientMonochrome="success" fullSized>
            <span className="text-white">{t('labels.approveBtn')}</span>
          </Button>
        </div>
      </div>
    </div>
  );
};

export { JoiningRequest };
