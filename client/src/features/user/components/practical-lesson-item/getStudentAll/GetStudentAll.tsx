'use client';

import { DateTime } from '@/components/date-time';
import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { Limit, Pagination } from '@/components/pagination';
import { getAllStudentPracticalLessonItems } from '@/features/user/api/practicalLessonItemsApi';
import { CurrentTab, userQueries } from '@/features/user/constants';
import { GetAllStudentPracticalLessonItemsResponse } from '@/features/user/types/practicalLessonItemTypes';
import { buildQueryString } from '@/shared/helpers';
import { useSetCurrentTab } from '@/shared/hooks';
import { ApiResponse } from '@/shared/types';
import { useQuery } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { usePathname, useRouter } from 'next/navigation';
import { FC, useEffect, useState } from 'react';

type GetStudentAllProps = {
  studentId: string;
  limitParameter: any;
  pageParameter: any;
};

const GetStudentAll: FC<GetStudentAllProps> = ({
  studentId,
  limitParameter,
  pageParameter,
}) => {
  const activeLocale = useLocale();
  const t = useTranslations('PracticalItem');
  const { replace } = useRouter();

  const pathname = usePathname();

  const defaultPage = 1;
  const limitOptions = [8, 20, 30];

  const [limit, setLimit] = useState<number>(
    parseInt(limitParameter) || limitOptions[0],
  );
  const [page, setPage] = useState<number>(
    parseInt(pageParameter) || defaultPage,
  );

  const skip = (page - 1) * limit;

  const buildQuery = () =>
    buildQueryString({
      studentId,
      take: limit,
      skip,
    });

  const textColorClasses = {
    submitted: 'font-semibold text-yellow-800',
    rejected: 'font-semibold text-red-800',
    accepted: 'font-semibold text-green-800',
  };

  const statusClasses = {
    submitted:
      'bg-gradient-to-r from-yellow-50 to-yellow-100 p-4 rounded-lg shadow-md mb-4',
    rejected:
      'bg-gradient-to-r from-red-50 to-red-100 p-4 rounded-lg shadow-md mb-4',
    accepted:
      'bg-gradient-to-r from-green-50 to-green-100 p-4 rounded-lg shadow-md mb-4',
  };

  const { data, isLoading } = useQuery<
    ApiResponse<GetAllStudentPracticalLessonItemsResponse>
  >({
    queryFn: () => getAllStudentPracticalLessonItems(buildQuery()),
    queryKey: ['getAllPracticalLessonItems', studentId, limit, skip],
    retry: userQueries.options.retry,
  });

  useEffect(() => {
    const params = new URLSearchParams();

    params.set('studentId', studentId);

    if (limit) params.set('take', limit.toString());
    if (page) params.set('page', page.toString());

    replace(`${pathname}?${params.toString()}`);
  }, [replace, limit, page, pathname, studentId]);

  useSetCurrentTab(CurrentTab.PracticalItems);

  if (isLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('studentListTitle')}
        </h2>
        <Loader />
      </>
    );
  }

  if (data && data.error) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('studentListTitle')}
        </h2>
        <Error error={data.error} />
      </>
    );
  }

  if (data && data.total === 0) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('studentListTitle')}
        </h2>
        <p className="w-full text-center text-red-700">{t('notFound')}</p>
      </>
    );
  }

  return (
    <div className="p-3">
      <h2 className="mb-2 text-center text-xl font-bold">
        {t('studentListTitle')} {data?.total ? `(${data.total})` : ''}
      </h2>

      <div className="mb-2 mt-2 flex w-full items-center justify-center">
        <Limit
          limitOptions={limitOptions}
          currentLimit={limit}
          onChangeLimit={(value) => {
            setLimit(value);
            setPage(defaultPage);
          }}
        />
      </div>

      <div className="mx-2 mt-3 flex flex-wrap justify-center">
        {data?.entries.map((item) => {
          const deadline = item.deadline ? new Date(item.deadline) : null;
          const now = new Date();

          let deadlineClassName = 'text-sm font-semibold text-gray-600';

          if (deadline && deadline.getTime() < now.getTime()) {
            deadlineClassName = 'text-sm font-semibold text-red-600';
          }

          return (
            <div
              key={item.id}
              className="mx-2 mb-4 w-full justify-center rounded border p-4 shadow-lg sm:w-1/2 md:w-1/3 lg:w-1/4 xl:w-1/5"
            >
              <h3 className="text-lg font-semibold">{item.title}</h3>
              <p className={`mt-2 ${deadlineClassName}`}>
                {t('item.deadline')}:{' '}
                {deadline ? <DateTime date={deadline} /> : '-'}
              </p>
              <p className="mt-2 text-sm font-medium text-gray-600">
                {t('item.createdAt')}: <DateTime date={item.createdAt} />
              </p>
              <p className="mt-4 text-sm font-medium text-gray-600">
                {t('item.courseName')}: {item.courseName}
              </p>
              <p className="mt-2 text-sm font-medium text-gray-600">
                {t('item.lessonTopic')}: {item.lessonTopic}
              </p>
              <p
                className={`mt-4 text-sm font-medium ${textColorClasses[item.status]}`}
              >
                {t('item.status')}:{' '}
                {item.status ? t(`item.statuses.${item.status}`) : '-'}
              </p>
              <button
                onClick={() =>
                  replace(
                    `/${activeLocale}/u/practical-item-submit/getOne/?studentId=${data?.studentId}&itemId=${item.id}`,
                  )
                }
                className="mt-2 rounded bg-blue-500 px-4 py-2 text-white hover:bg-blue-700"
              >
                {t('item.details')}
              </button>
            </div>
          );
        })}
      </div>

      {data && data.total > limit && (
        <Pagination
          currentPage={page}
          totalCount={data.total}
          limit={limit}
          onChangePage={(value) => setPage(value)}
        />
      )}
    </div>
  );
};

export { GetStudentAll };
