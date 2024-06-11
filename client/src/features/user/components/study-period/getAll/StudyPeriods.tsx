'use client';

import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { useProfiles } from '@/features/user';
import { getAllStudyPeriods } from '@/features/user/api/studyPeriodApi';
import { CurrentTab, userQueries } from '@/features/user/constants';
import { StudyPeriodResponse } from '@/features/user/types/studyPeriodTypes';
import { useAuthRedirectByRole, useSetCurrentTab } from '@/shared/hooks';
import { ApiResponse } from '@/shared/types';
import { useQuery } from '@tanstack/react-query';
import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC, useEffect } from 'react';
import { StudyPeriodsItem } from './StudyPeriodItem';

const StudyPeriods: FC = () => {
  const t = useTranslations('StudyPeriod');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');
  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const { data, isLoading, refetch } = useQuery<
    ApiResponse<StudyPeriodResponse[]>
  >({
    queryFn: () => getAllStudyPeriods(),
    queryKey: [userQueries.getStudyPeriods],
    retry: userQueries.options.retry,
  });

  useEffect(() => {
    refetch();
  }, [activeProfile, refetch]);

  useSetCurrentTab(CurrentTab.StudyPeriods);

  if (isLoading || isUserLoading || profilesLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('listTitle')}</h2>

        <Loader />
      </>
    );
  }

  if (data && data.error) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('listTitle')}</h2>

        <Error error={data.error} />
      </>
    );
  }

  return (
    <div>
      <h2 className="mb-2 mt-6 text-center text-xl font-bold">
        {t('listTitle')} {data?.length ? `(${data.length})` : ''}
      </h2>

      <div className="flex w-full justify-center pb-3 pb-3">
        <Button
          gradientMonochrome="success"
          size="md"
          onClick={() => replace(`/${activeLocale}/u/study-periods/create/`)}
        >
          <span className="text-white">{t('createBtn')}</span>
        </Button>
      </div>

      <div className="mt-4 grid grid-cols-1 justify-items-center gap-4 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4">
        {data &&
          data.length > 0 &&
          data?.map((period, idx) => (
            <StudyPeriodsItem
              activeProfile={activeProfile}
              key={period.id}
              studyPeriod={period}
              index={data.length - idx}
            />
          ))}
      </div>
    </div>
  );
};

export { StudyPeriods };
