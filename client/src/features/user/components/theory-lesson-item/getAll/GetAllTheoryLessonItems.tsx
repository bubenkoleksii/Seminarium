'use client';

import { NotFound } from '@/components/error';
import { Loader } from '@/components/loader';
import { getAllTheoryLessonItems } from '@/features/user/api/theoryLessonItemsApi';
import { userQueries } from '@/features/user/constants';
import { useProfiles } from '@/features/user/hooks';
import { TheoryLessonItemResponse } from '@/features/user/types/theoryLessonItemTypes';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { ApiResponse } from '@/shared/types';
import { useQuery } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import TheoryLessonItem from './TheoryLessonItem';

type GetAllTheoryLessonItemsProps = {
  lessonId: string;
  courseId: string;
};

const GetAllTheoryLessonItems: FC<GetAllTheoryLessonItemsProps> = ({
  lessonId,
  courseId,
}) => {
  const activeLocale = useLocale();
  const t = useTranslations('TheoryItem');
  const v = useTranslations('Validation');
  const { replace } = useRouter();

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');
  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const { data, isLoading } = useQuery<ApiResponse<TheoryLessonItemResponse[]>>(
    {
      queryFn: () => getAllTheoryLessonItems(lessonId),
      queryKey: ['theoryItems', lessonId],
      retry: userQueries.options.retry,
    },
  );

  if (isLoading || isUserLoading || profilesLoading) {
    return (
      <>
        <Loader />
      </>
    );
  }

  if (!data) {
    return <NotFound />;
  }

  const canModify = activeProfile.type === 'teacher';

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
          <TheoryLessonItem
            key={lesson.lessonId}
            courseId={courseId}
            canModify={canModify}
            lesson={lesson}
          />
        ))}
    </div>
  );
};

export { GetAllTheoryLessonItems };
