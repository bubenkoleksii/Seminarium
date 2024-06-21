'use client';

import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { Limit, Pagination } from '@/components/pagination';
import { SearchInput } from '@/components/search-input';
import { useProfiles } from '@/features/user';
import { getAllCourses } from '@/features/user/api/coursesApi';
import { CurrentTab, userQueries } from '@/features/user/constants';
import type { PagesCoursesResponse } from '@/features/user/types/courseTypes';
import { buildQueryString } from '@/shared/helpers';
import { useAuthRedirectByRole, useSetCurrentTab } from '@/shared/hooks';
import { ApiResponse } from '@/shared/types';
import { useQuery } from '@tanstack/react-query';
import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import { usePathname, useRouter } from 'next/navigation';
import { FC, useEffect, useState } from 'react';
import { StudyPeriodsDropdown } from '../../study-period/getAll/StudyPeriodDropdown';
import { CourseItem } from './CoursesItem';

type CoursesProps = {
  studyPeriodIdParameter: string | null;
  searchParameter?: string;
  limitParameter?: number | null;
  pageParameter?: number | null;
  groupIdParameter?: string | null;
  teacherIdParameter?: string | null;
};

const Courses: FC<CoursesProps> = ({
  studyPeriodIdParameter,
  searchParameter,
  limitParameter,
  pageParameter,
  groupIdParameter,
  teacherIdParameter,
}) => {
  const activeLocale = useLocale();
  const t = useTranslations('Course');

  const pathname = usePathname();
  const { replace } = useRouter();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');
  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const [studyPeriodId, setStudyPeriodId] = useState<string | null>(
    studyPeriodIdParameter,
  );

  const [groupId, setGroupId] = useState<string | null | undefined>(
    groupIdParameter,
  );
  const [teacherId, setTeacherId] = useState<string | null | undefined>(
    teacherIdParameter,
  );

  const [search, setSearch] = useState<string>(searchParameter || '');
  const handleSearch = (value: string) => {
    setSearch(value);
  };

  const defaultPage = 1;
  const [page, setPage] = useState<number>(pageParameter || defaultPage);
  const handlePage = (value: number) => {
    setPage(value);
  };

  const limitOptions = [8, 20, 30];
  const [limit, setLimit] = useState<number>(limitParameter || limitOptions[0]);
  const handleLimit = (value: number) => {
    setLimit(value);
    setPage(defaultPage);
  };

  const skip =
    ((pageParameter || defaultPage) - 1) * (limitParameter || limitOptions[0]);

  const buildQuery = () =>
    buildQueryString({
      studyPeriodId,
      name: searchParameter,
      groupId: groupIdParameter,
      teacherId: teacherIdParameter,
      take: limitParameter,
      skip,
    });

  const { data, isLoading, refetch } = useQuery<
    ApiResponse<PagesCoursesResponse>
  >({
    queryFn: () => getAllCourses(buildQuery()),
    queryKey: [
      userQueries.getCourses,
      searchParameter,
      limitParameter,
      pageParameter,
      studyPeriodIdParameter,
      teacherIdParameter,
      groupIdParameter,
    ],
    retry: userQueries.options.retry,
  });

  useEffect(() => {
    setStudyPeriodId(studyPeriodIdParameter);
  }, [studyPeriodIdParameter]);

  useEffect(() => {
    setGroupId(groupIdParameter);
  }, [groupIdParameter]);

  useEffect(() => {
    setTeacherId(teacherIdParameter);
  }, [teacherIdParameter]);

  useEffect(() => {
    refetch();
  }, [activeProfile, refetch]);

  useEffect(() => {
    const params = new URLSearchParams();

    if (studyPeriodId) params.set('studyPeriodId', studyPeriodId);

    if (search) params.set('search', search);
    if (groupId) params.set('groupId', groupId);
    if (teacherId) params.set('teacherId', teacherId);

    if (limit) params.set('take', limit.toString());
    if (page) params.set('page', page.toString());

    replace(`${pathname}?${params.toString()}`);
  }, [
    search,
    studyPeriodId,
    pathname,
    replace,
    limit,
    page,
    groupId,
    teacherId,
  ]);

  useSetCurrentTab(CurrentTab.Course);

  if (isLoading || isUserLoading || profilesLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('listTitle')}</h2>
        <Loader />
      </>
    );
  }

  if (data && data.error) {
    if (data.status = 404) {
      <div>
        <h2 className="mb-2 mt-6 text-center text-xl font-bold">
          {t('listTitle')} {data?.length ? `(${data.length})` : ''}
        </h2>

        {activeProfile.type !== 'student' && activeProfile.type !== 'parent' && (
          <div className="flex w-full justify-center pb-3 pb-3">
            <Button
              gradientMonochrome="success"
              size="md"
              onClick={() => replace(`/${activeLocale}/u/courses/create/`)}
            >
              <span className="text-white">{t('createBtn')}</span>
            </Button>
          </div>
        )}
      </div>
    }

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

      {activeProfile.type !== 'student' && activeProfile.type !== 'parent' && (
        <div className="flex w-full justify-center pb-3 pb-3">
          <Button
            gradientMonochrome="success"
            size="md"
            onClick={() => replace(`/${activeLocale}/u/courses/create/`)}
          >
            <span className="text-white">{t('createBtn')}</span>
          </Button>
        </div>
      )}

      <SearchInput
        maxLength={200}
        value={search}
        placeholder={t('placeholders.search')}
        onSubmit={handleSearch}
      />

      <div className="mb-2 mt-2 flex w-full items-center justify-center">
        <div className="w-md relative">
          <StudyPeriodsDropdown onSelect={(id) => setStudyPeriodId(id)} />
        </div>

        <div className="w-md relative">
          <Limit
            limitOptions={limitOptions}
            currentLimit={limit}
            onChangeLimit={(limit) => handleLimit(limit)}
          />
        </div>
      </div>

      {data && data.total > data.take && (
        <div className="mt-6">
          <Pagination
            currentPage={page}
            totalCount={data.total}
            limit={data.take}
            onChangePage={(page) => handlePage(page)}
          />
        </div>
      )}

      {data && data.entries.length === 0 ? (
        <>
          <div className="mt-16 flex items-center justify-center font-semibold">
            {t('labels.notFound')}
          </div>
        </>
      ) : (
        <div className="relative">
          <div className="flex flex-wrap justify-center gap-4">
            {data.entries.map((course, idx) => (
              <CourseItem
                key={idx}
                course={course}
                activeProfile={activeProfile}
              />
            ))}
          </div>
        </div>
      )}
    </div>
  );
};

export { Courses };

