'use client';

import { DateTime } from '@/components/date-time';
import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { Limit, Pagination } from '@/components/pagination';
import { getTeacherAllPracticalLessonItemSubmit } from '@/features/user/api/practicalLessonItemSubmitApi';
import { PagesPracticalLessonItemSubmitResponse } from '@/features/user/types/practicalLessonItemSubmitTypes';
import { buildQueryString } from '@/shared/helpers';
import { ApiResponse } from '@/shared/types';
import { useQuery } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { usePathname, useRouter } from 'next/navigation';
import { FC, useEffect, useState } from 'react';

type GetTeacherAllProps = {
  itemId: string;
  limitParameter: any;
  pageParameter: any;
};

const GetTeacherAll: FC<GetTeacherAllProps> = ({ itemId, limitParameter, pageParameter }) => {
  const activeLocale = useLocale();
  const t = useTranslations('PracticalItemSubmit');
  const { replace } = useRouter();

  const pathname = usePathname();

  const defaultPage = 1;
  const limitOptions = [8, 20, 30];

  const [limit, setLimit] = useState<number>(parseInt(limitParameter) || limitOptions[0]);
  const [page, setPage] = useState<number>(parseInt(pageParameter) || defaultPage);

  const skip =
    ((pageParameter || defaultPage) - 1) * (limitParameter || limitOptions[0]);

  const buildQuery = () =>
    buildQueryString({
      itemId,
      take: limit,
      skip,
    });

  const { data, isLoading } = useQuery<ApiResponse<PagesPracticalLessonItemSubmitResponse>>({
    queryFn: () => getTeacherAllPracticalLessonItemSubmit(buildQuery()),
    queryKey: ['getTeacherAllPracticalLessonItemSubmit', 99],
    retry: 1,
    enabled: !!itemId,
  });

  useEffect(() => {
    const params = new URLSearchParams();

    if (limit) params.set('take', limit.toString());
    if (page) params.set('page', page.toString());

    params.set('itemId', itemId);

    replace(`${pathname}?${params.toString()}`);

  }, [replace, limit, page]);

  if (isLoading) {
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
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('listTitle')}

        </h2>
        <Error error={data.error} />
      </>
    );
  }

  if (data && data.total === 0) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('listTitle')}

        </h2>
        <p className="w-full text-center text-red-700">{t('notFound')}</p>
      </>
    );
  }

  return (
    <div className="p-3">
      <h2 className="mb-2 text-center text-xl font-bold">
        {t('listTitle')} {data?.total ? `(${data.total})` : ''}
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

      <div className="flex flex-wrap mx-2">
        {data?.entries.map((item) => (
          <div key={item.id} className="mb-4 p-4 border justify-center rounded shadow-lg w-full sm:w-1/2 md:w-1/3 lg:w-1/4 xl:w-1/5 mx-2">
            <a href={`/${activeLocale}/u/school-profile/${item.studentId}`} className="text-lg font-semibold text-blue-500 hover:underline">
              {item.studentName || item.studentId}
            </a>

            <p><DateTime date={item.createdAt} /></p>
            <button
              onClick={() => replace(`/${activeLocale}/u/practical-item-submit/getOne/?studentId=${item.studentId}&itemId=${itemId}`)}
              className="mt-2 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-700">
              {t('details')}
            </button>
          </div>
        ))}
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

export { GetTeacherAll };
