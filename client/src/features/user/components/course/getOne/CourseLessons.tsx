'use client';

import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { SearchInput } from '@/components/search-input';
import { getAllLessons } from '@/features/user/api/lessonsApi';
import { userQueries } from '@/features/user/constants';
import type { PagesLessonsResponse } from '@/features/user/types/lessonTypes';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { ApiResponse } from '@/shared/types';
import { useQuery } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { FC, useEffect, useState } from 'react';

type CourseLessonsProps = {
  courseId: string;
};

const CourseLessons: FC<CourseLessonsProps> = ({ courseId }) => {
  const activeLocale = useLocale();
  const t = useTranslations('Lesson');

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const [search, setSearch] = useState<string>('');

  const buildQuery = () => {
    const params = new URLSearchParams();

    params.set('courseId', courseId);

    if (search) params.set('search', search);

    return params.toString();
  };

  const { data, isLoading, refetch } = useQuery<
    ApiResponse<PagesLessonsResponse>
  >({
    queryFn: () => getAllLessons({ query: buildQuery() }),
    queryKey: ['courseLessons', courseId, search],
    retry: userQueries.options.retry,
  });

  const handleSearch = (search) => setSearch(search);

  useEffect(() => {
    refetch();
  }, [courseId, refetch, search]);

  if (isLoading || isUserLoading) {
    return (
      <>
        <Loader />
      </>
    );
  }

  if (data && data.error) {
    return (
      <>
        <Error error={data.error} />
      </>
    );
  }

  return (
    <div>
      <div className="mb-4 flex w-full justify-center">
        <SearchInput
          maxLength={200}
          value={search}
          placeholder={t('placeholders.topic')}
          onSubmit={handleSearch}
        />
      </div>

      {data.entries && data.entries.length === 0 ? (
        <div className="mt-16 flex items-center justify-center font-semibold">
          {t('labels.notFound')}
        </div>
      ) : (
        <div className="relative">
          <div className="flex flex-col items-start gap-4">
            {data?.entries?.map((lesson, idx) => (
              <div
                key={idx}
                className="w-full rounded-md border border-gray-200 p-4 shadow-sm"
              >
                <h3 className="text-lg font-semibold">
                  {t('lesson')} {lesson.number}
                </h3>
                <p>
                  <strong>{t('topic')}: </strong>
                  {lesson.topic}
                </p>
                {lesson.homework && (
                  <p>
                    <strong>{t('homework')}: </strong>
                    {lesson.homework}
                  </p>
                )}
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};

export { CourseLessons };

