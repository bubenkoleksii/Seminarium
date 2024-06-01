'use client';

import { FC, useEffect, useState } from 'react';
import { usePathname, useRouter } from 'next/navigation';
import { useAuthRedirectByRole, useSetCurrentTab } from '@/shared/hooks';
import { CurrentTab, userQueries } from '@/features/user/constants';
import { buildQueryString } from '@/shared/helpers';
import { useQuery } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { getAll } from '@/features/user/api/groupsApi';
import { SearchInput } from '@/components/search-input';
import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { Limit, Pagination } from '@/components/pagination';
import { mediaQueries } from '@/shared/constants';
import { useMediaQuery } from 'react-responsive';
import { Button } from 'flowbite-react';
import { groupClientPaths } from '@/features/user/routes';
import { GroupsItem } from './GroupsItem';
import { useProfiles } from '@/features/user';

interface GroupsProps {
  studyPeriodNumber?: number | null;
  searchParameter?: string;
  limitParameter?: number | null;
  pageParameter?: number | null;
}

const Groups: FC<GroupsProps> = ({
  studyPeriodNumber,
  searchParameter,
  limitParameter,
  pageParameter,
}) => {
  const activeLocale = useLocale();
  const t = useTranslations('Group');

  const pathname = usePathname();
  const { replace } = useRouter();

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');
  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const [search, setSearch] = useState<string>(searchParameter || '');
  const handleSearch = (value: string) => {
    setSearch(value);
  };

  const [studyPeriod, setStudyPeriod] = useState<number | null>(
    studyPeriodNumber || null,
  );
  const handleFilterByStudyPeriod = (value: string) => {
    const numberValue = value ? parseInt(value, 10) : null;
    if (numberValue > 100) setStudyPeriod(100);

    setStudyPeriod(numberValue);
  };
  const handleFilterByStudyPeriodClear = () => {
    setStudyPeriod(null);
  };

  const defaultPage = 1;
  const [page, setPage] = useState<number>(pageParameter || defaultPage);
  const handlePage = (value: number) => {
    setPage(value);
  };

  const limitOptions = [10, 20, 30];
  const [limit, setLimit] = useState<number>(limitParameter || limitOptions[0]);
  const handleLimit = (value: number) => {
    setLimit(value);
    setPage(defaultPage);
  };

  const skip =
    ((pageParameter || defaultPage) - 1) * (limitParameter || limitOptions[0]);

  const buildQuery = () =>
    buildQueryString({
      name: searchParameter,
      studyPeriodNumber: studyPeriodNumber,
      take: limitParameter,
      skip,
    });

  useSetCurrentTab(CurrentTab.Group);

  const { data, isLoading, refetch } = useQuery({
    queryFn: () =>
      getAll({
        query: buildQuery(),
      }),
    queryKey: [
      userQueries.getGroups,
      searchParameter,
      studyPeriodNumber,
      limitParameter,
      pageParameter,
    ],
    retry: userQueries.options.retry,
  });

  const isDesktopOrLaptop = useMediaQuery({
    query: mediaQueries.desktopOrLaptop,
  });

  useEffect(() => {
    refetch();
  }, [activeProfile, refetch]);

  useEffect(() => {
    const params = new URLSearchParams();

    if (search) params.set('name', search);
    if (studyPeriod) params.set('studyPeriodNumber', studyPeriod.toString());

    if (limit) params.set('take', limit.toString());
    if (page) params.set('page', page.toString());

    replace(`${pathname}?${params.toString()}`);
  }, [search, studyPeriod, replace, pathname, limit, page]);

  if (isLoading || isUserLoading || profilesLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('listTitle')}</h2>

        <Loader />
      </>
    );
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
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
    <div className="p-3">
      <h2 className="mb-2 text-center text-xl font-bold">
        {t('listTitle')} {data?.total ? `(${data.total})` : ''}
      </h2>

      <div className="mb-4 flex items-center justify-center gap-2">
        <div>
          <p className="mb-4 text-center text-lg font-semibold text-purple-950">
            {data.schoolName}
          </p>
        </div>

        <div className="pb-3">
          <Button
            gradientMonochrome="success"
            size="md"
            onClick={() =>
              replace(`/${activeLocale}${groupClientPaths.create}`)
            }
          >
            <span className="text-white">{t('labels.createBtn')}</span>
          </Button>
        </div>
      </div>

      <SearchInput
        maxLength={200}
        value={search}
        placeholder={t('placeholders.search')}
        onSubmit={handleSearch}
      />

      <div
        className={`mb-4 mt-4 flex items-center justify-center ${isDesktopOrLaptop ? 'flex-row gap-8' : 'flex-col gap-6'}`}
      >
        <div className="w-md relative flex flex-col items-center">
          <label className="relative mb-1 block text-center font-medium text-gray-700">
            {t('labels.studyPeriodNumber')}
            <span
              onClick={() => handleFilterByStudyPeriodClear()}
              className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
            >
              {t('labels.clear')}
            </span>
          </label>
          <input
            id="studyPeriod"
            className="block w-[100px] appearance-none rounded-lg border border-gray-300 px-4
            py-2 focus:border-purple-950 focus:outline-none focus:ring-1 focus:ring-purple-950"
            name="studyPeriod"
            type="number"
            min="0"
            placeholder="8"
            value={studyPeriod !== null ? studyPeriod : ''}
            onChange={(e) => handleFilterByStudyPeriod(e.target.value)}
          ></input>
        </div>

        <div className="w-md relative">
          <Limit
            limitOptions={limitOptions}
            currentLimit={limit}
            onChangeLimit={(limit) => handleLimit(limit)}
          />
        </div>
      </div>

      {data && data.entries.length !== data.total && (
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
          <div className="flex flex-wrap justify-center">
            {data.entries.map((group, idx) => (
              <GroupsItem key={idx} group={group} activeProfile={activeProfile} />
            ))}
          </div>
        </div>
      )}
    </div>
  );
};

export { Groups };
