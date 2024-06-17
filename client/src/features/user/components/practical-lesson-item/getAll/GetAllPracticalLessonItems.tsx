'use client';

import { NotFound } from '@/components/error';
import { Loader } from '@/components/loader';
import { getAllPracticalLessonItems } from '@/features/user/api/practicalLessonItemsApi';
import { userQueries } from '@/features/user/constants';
import { useProfiles } from '@/features/user/hooks';
import { PracticalLessonItemResponse } from '@/features/user/types/practicalLessonItemTypes';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { ApiResponse } from '@/shared/types';
import { useQuery } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import { PracticalLessonItem } from './PracticalLessonItem';

type GetAllPracticalLessonItemsProps = {
  lessonId: string;
  courseId: string;
};

const GetAllPracticalLessonItems: FC<GetAllPracticalLessonItemsProps> = ({
  lessonId,
  courseId,
}) => {
  const activeLocale = useLocale();
  const t = useTranslations('PracticalItem');
  const { replace } = useRouter();

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');
  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const { data, isLoading } = useQuery<
    ApiResponse<PracticalLessonItemResponse[]>
  >({
    queryFn: () => getAllPracticalLessonItems(lessonId),
    queryKey: ['practicalItems', lessonId],
    retry: userQueries.options.retry,
  });

  if (isLoading || isUserLoading || profilesLoading) {
    return (
      <>
        <Loader />
      </>
    );
  }

  if (!data || data.error) {
    return <NotFound />;
  }

  const canModify = activeProfile.type === 'teacher';

  console.log('data', data);

  return (
    <div>
      <h2 className="mb-2 mt-6 text-center text-xl font-bold">
        {t('listTitle')}
        <p
          onClick={() => replace(`/${activeLocale}/u/courses/${courseId}`)}
          className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
        >
          {t('backTo')}
        </p>
      </h2>

      {data &&
        data?.map((lesson) => (
          <PracticalLessonItem
            key={lesson.lessonId}
            courseId={courseId}
            canModify={canModify}
            lesson={lesson}
            activeProfile={activeProfile}
          />
        ))}
    </div>
  );
};

export { GetAllPracticalLessonItems };
