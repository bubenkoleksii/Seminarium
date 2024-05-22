'use client';

import { FC } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useRouter } from 'next/navigation';
import { useQuery } from '@tanstack/react-query';
import { ApiResponse } from '@/shared/types';
import { getOne } from './api';
import { getOneSchoolRoute, schoolsClientPath } from './constants';
import { SchoolResponse } from './types';
import { Loader } from '@/components/loader';
import { getColorByStatus } from '@/shared/helpers';
import { Error } from '@/components/error';
import { CustomImage } from '@/components/custom-image';
import { mediaQueries } from '@/shared/constants';
import { useMediaQuery } from 'react-responsive';

interface SchoolProps {
  id: string;
}

const School: FC<SchoolProps> = ({ id }) => {
  const t = useTranslations('School');

  const { replace } = useRouter();
  const activeLocale = useLocale();

  const { isUserLoading, user } = useAuthRedirectByRole(activeLocale, 'user');

  const { data, isLoading } = useQuery<ApiResponse<SchoolResponse>>({
    queryKey: [getOneSchoolRoute, id],
    queryFn: () => getOne(id),
    enabled: !!id,
  });

  const isPhone = useMediaQuery({ query: mediaQueries.phone });

  if (isLoading || isUserLoading) {
    return (
      <>
        <h2 className="mb-4 mt-2 text-center text-xl font-bold">
          {t('oneTitle')}

          {user?.role === 'admin' &&
            <span
              onClick={() =>
                replace(`/${activeLocale}/${schoolsClientPath}`)
              }
              className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
            >
            {t('labels.toMain')}
          </span>
          }
        </h2>
        <Loader />
      </>
    )
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  }

  if (data && data.error) {
    return (
      <div className="flex flex-col justify-center">
        <h2 className="mb-4 mt-2 text-center text-xl font-bold">
          {t('oneTitle')}

          {user?.role === 'admin' &&
            <span
              onClick={() =>
                replace(`/${activeLocale}/${schoolsClientPath}`)
              }
              className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
            >
            {t('labels.toMain')}
          </span>
          }
        </h2>

        <Error error={data.error} />
      </div>
    );
  }

  const occupiedColor = getColorByStatus(data.areOccupied ? 'danger' : 'ok');

  return (
    <div className="p-3">
      <h2 className="mb-4 mt-2 text-center text-xl font-bold">
        {t('oneTitle')}

        {user?.role === 'admin' &&
          <span
            onClick={() =>
              replace(`/${activeLocale}/${schoolsClientPath}`)
            }
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('labels.toMain')}
          </span>
        }

        <h6 className="py-2 text-center font-bold">
          <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
            {t('labels.name')}
          </p>
          <span className="text-purple-950 lg:text-2xl">{data.name}</span>
        </h6>

        <div className="flex justify-center items-center">
          <CustomImage
            src={data.img}
            alt='School image'
            width={isPhone ? 300 : 600}
            height={isPhone ? 200 : 400}
          />
        </div>

        <h6 className="pt-4 pb-2 text-center font-bold">
          <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
            {t('labels.detailInfo')}
          </p>
        </h6>

        <div className="flex text-xs lg:text-lg">
          <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
            <span className="text-center">{t('labels.id')}</span>
          </div>
          <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
            <span>{data.id}</span>
          </div>
        </div>
      </h2>
    </div>
  );
};

export { School };
