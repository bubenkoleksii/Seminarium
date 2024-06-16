'use client';

import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { SearchInput } from '@/components/search-input';
import { getAllLessons, removeLesson } from '@/features/user/api/lessonsApi';
import { userMutations, userQueries } from '@/features/user/constants';
import type {
  LessonResponse,
  PagesLessonsResponse,
} from '@/features/user/types/lessonTypes';
import { buildQueryString } from '@/shared/helpers';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { ApiResponse } from '@/shared/types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { Button, Table } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC, useEffect, useState } from 'react';
import toast from 'react-hot-toast';

type CourseLessonsProps = {
  courseId: string;
  canModify: boolean;
};

const CourseLessons: FC<CourseLessonsProps> = ({ courseId, canModify }) => {
  const activeLocale = useLocale();
  const t = useTranslations('Lesson');
  const v = useTranslations('Validation');

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const queryClient = useQueryClient();

  const [search, setSearch] = useState<string>('');

  const { replace } = useRouter();

  const buildQuery = () => {
    const params = new URLSearchParams();

    params.set('courseId', courseId);

    if (search) params.set('topic', search);

    return params.toString();
  };

  const { data, isLoading, refetch } = useQuery<
    ApiResponse<PagesLessonsResponse>
  >({
    queryFn: () => getAllLessons({ query: buildQuery() }),
    queryKey: ['courseLessons', courseId, search],
    retry: userQueries.options.retry,
  });

  const { mutate: deleteLesson } = useMutation({
    mutationFn: removeLesson,
    mutationKey: [userMutations.deleteLesson],
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          404: t('labels.oneNotFound'),
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('labels.deleteSuccess'), { duration: 2500 });
      }
    },
    onSettled: async () => {
      queryClient.invalidateQueries({
        queryKey: ['courseLessons', courseId, search],
        refetchType: 'all',
      });
    },
  });

  const buildUpdateQuery = (lesson: LessonResponse) => {
    return buildQueryString({
      id: lesson.id,
      courseId: courseId,
      number: lesson.number,
      topic: lesson.topic,
      homework: lesson.homework,
    });
  };

  const handleSearch = (search) => setSearch(search);

  const handleLessonDelete = (id: string) => {
    deleteLesson(id);
  };

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
    <div className="overflow-x-auto">
      <div className="mb-4 mt-4 flex w-full justify-center">
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
        <Table className="w-full table-fixed">
          <Table.Head>
            <Table.HeadCell>{t('labels.number')}</Table.HeadCell>
            <Table.HeadCell>{t('labels.topic')}</Table.HeadCell>
            <Table.HeadCell>{t('labels.homework')}</Table.HeadCell>
            <Table.HeadCell></Table.HeadCell>
          </Table.Head>
          <Table.Body className="divide-y">
            {data?.entries?.map((lesson, idx) => (
              <Table.Row
                key={idx}
                className="bg-white dark:border-gray-700 dark:bg-gray-800"
              >
                <Table.Cell>{lesson.number}</Table.Cell>
                <Table.Cell>
                  <div className="flex flex-col gap-2">
                    {lesson.topic}

                    {canModify &&
                      <>
                        <div className="w-[150px]">
                          <Button
                            onClick={() =>
                              replace(
                                `/${activeLocale}/u/practical-item/create/?courseId=${courseId}&lessonId=${lesson.id}`,
                              )
                            }
                            gradientMonochrome="purple"
                            size="xs"
                          >
                            <span className="text-white">{t('addPractical')}</span>
                          </Button>
                        </div>

                        <div className="w-[150px]">
                          <Button
                            onClick={() =>
                              replace(
                                `/${activeLocale}/u/theory-item/create/?courseId=${courseId}&lessonId=${lesson.id}`,
                              )
                            }
                            gradientMonochrome="pink"
                            size="xs"
                          >
                            <span className="text-white">{t('addTheory')}</span>
                          </Button>
                        </div>
                      </>
                    }
                  </div>
                </Table.Cell>
                <Table.Cell>{lesson.homework}</Table.Cell>
                {canModify && (
                  <>
                    <Table.Cell>
                      <div className="mt-2 flex w-full flex-wrap justify-center gap-4 md:flex-nowrap">
                        <Button
                          onClick={() =>
                            replace(
                              `/${activeLocale}/u/lessons/update/${courseId}/?${buildUpdateQuery(lesson)}`,
                            )
                          }
                          gradientMonochrome="lime"
                          size="xs"
                        >
                          <span className="text-gray-700">
                            {t('updateBtn')}
                          </span>
                        </Button>

                        <Button
                          onClick={() => handleLessonDelete(lesson.id)}
                          gradientMonochrome="failure"
                          size="xs"
                        >
                          <span className="text-white">{t('deleteBtn')}</span>
                        </Button>
                      </div>
                    </Table.Cell>
                  </>
                )}
              </Table.Row>
            ))}
          </Table.Body>
        </Table>
      )}
    </div>
  );
};

export { CourseLessons };

