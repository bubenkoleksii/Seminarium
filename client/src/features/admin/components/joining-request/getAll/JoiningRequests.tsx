'use client';

import { FC, useEffect, useState } from 'react';
import { usePathname, useRouter } from 'next/navigation';
import type { ApiResponse } from '@/shared/types';
import type { PagedJoiningRequests } from '@/features/admin/types/joiningRequestTypes';
import { adminQueries, CurrentTab } from '../../../constants';
import { getAll } from '../../../api/joiningRequestApi';
import { useQuery } from '@tanstack/react-query';
import { Loader } from '@/components/loader';
import { JoiningRequestsItem } from './JoiningRequestsItem';
import { Table } from 'flowbite-react';
import { useMediaQuery } from 'react-responsive';
import { useLocale, useTranslations } from 'next-intl';
import { Error } from '@/components/error';
import { useAuthRedirectByRole, useSetCurrentTab } from '@/shared/hooks';
import { mediaQueries, routes, school } from '@/shared/constants';
import { SearchInput } from '@/components/search-input';
import { buildQueryString } from '@/shared/helpers';
import { Limit, Pagination } from '@/components/pagination';
import Link from 'next/link';

interface JoiningRequestsProps {
  regionParameter?: string;
  sortByDateAscParameter?: string;
  searchParameter?: string;
  statusParameter?: string;
  limitParameter?: number | null;
  pageParameter?: number | null;
}

const JoiningRequests: FC<JoiningRequestsProps> = ({
  regionParameter,
  sortByDateAscParameter,
  searchParameter,
  statusParameter,
  limitParameter,
  pageParameter,
}) => {
  const t = useTranslations('JoiningRequest');
  const w = useTranslations('Welcome');
  const activeLocale = useLocale();

  const pathname = usePathname();
  const { replace } = useRouter();

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'admin');

  const [search, setSearch] = useState<string>(searchParameter || '');
  const handleSearch = (value: string) => {
    setSearch(value);
  };

  const [status, setStatus] = useState<string>(statusParameter || '');
  const handleStatus = (value: string) => {
    setStatus(value);
  };
  const handleStatusClear = () => {
    setStatus('');
  };

  const [sortByDate, setSortByDate] = useState<string>(
    sortByDateAscParameter || '',
  );
  const handleSortByDate = (value: string) => {
    setSortByDate(value);
  };
  const handleSortByDateClear = () => {
    if (sortByDate) setSortByDate('');
  };

  const [filterByRegion, setFilterByRegion] = useState<string>(
    regionParameter || '',
  );
  const handleFilterByRegion = (value: string) => {
    setFilterByRegion(value);
  };
  const handleFilterByRegionClear = () => {
    setFilterByRegion('');
  };

  const defaultPage = 1;
  const [page, setPage] = useState<number>(pageParameter || defaultPage);
  const handlePage = (value: number) => {
    setPage(value);
  };

  const limitOptions = [4, 8, 12];
  const [limit, setLimit] = useState<number>(limitParameter || limitOptions[0]);
  const handleLimit = (value: number) => {
    setLimit(value);
    setPage(defaultPage);
  };

  const skip =
    ((pageParameter || defaultPage) - 1) * (limitParameter || limitOptions[0]);

  const buildQuery = () =>
    buildQueryString({
      region: regionParameter,
      sortByDateAsc: sortByDateAscParameter,
      schoolName: searchParameter,
      status: statusParameter,
      take: limitParameter,
      skip,
    });

  const { data, isLoading } = useQuery<ApiResponse<PagedJoiningRequests>>({
    queryKey: [
      adminQueries.getAllJoiningRequests,
      regionParameter,
      sortByDateAscParameter,
      searchParameter,
      statusParameter,
      limitParameter,
      pageParameter,
    ],
    queryFn: () => getAll(buildQuery()),
    retry: adminQueries.options.retry,
  });

  useSetCurrentTab(CurrentTab.JoiningRequest);

  const isDesktopOrLaptop = useMediaQuery({
    query: mediaQueries.desktopOrLaptop,
  });

  useEffect(() => {
    const params = new URLSearchParams();

    if (search) params.set('schoolName', search);
    if (status) params.set('status', status);
    if (filterByRegion) params.set('region', filterByRegion);
    if (limit) params.set('take', limit.toString());
    if (page) params.set('page', page.toString());

    if (sortByDate) {
      const sortByDateParameter = sortByDate === 'asc' ? 'true' : 'false';
      params.set('sortByDateAsc', sortByDateParameter);
    }

    replace(`${pathname}?${params.toString()}`);
  }, [
    search,
    status,
    sortByDate,
    replace,
    pathname,
    filterByRegion,
    limit,
    page,
  ]);

  if (isLoading || isUserLoading) {
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
      <h2 className="mb-4 text-center text-xl font-bold">{t('listTitle')}</h2>

      <SearchInput
        maxLength={200}
        value={search}
        placeholder={t('placeholders.shortName')}
        onSubmit={handleSearch}
      />

      <div
        className={`mb-4 mt-4 flex items-center justify-center ${isDesktopOrLaptop ? 'flex-row gap-8' : 'flex-col gap-6'}`}
      >
        <div className="w-md relative">
          <label className="relative mb-1 block text-center font-medium text-gray-700">
            {t('labels.dateSorter.label')}
            <span
              onClick={() => handleSortByDateClear()}
              className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
            >
              {t('labels.clear')}
            </span>
          </label>
          <select
            id="date"
            className="block w-[300px] appearance-none rounded-lg border border-gray-300 px-4
            py-2 focus:border-purple-950 focus:outline-none focus:ring-1 focus:ring-purple-950"
            name="type"
            value={sortByDate}
            onChange={(e) => handleSortByDate(e.target.value)}
          >
            <option value=""></option>
            <option value={'desc'}>{t(`labels.dateSorter.desc`)}</option>
            <option value={'asc'}>{t(`labels.dateSorter.asc`)}</option>
          </select>
        </div>

        <div className="w-md relative">
          <label className="relative mb-1 block text-center font-medium text-gray-700">
            {t('labels.status.filterLabel')}
            <span
              onClick={() => handleStatusClear()}
              className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
            >
              {t('labels.clear')}
            </span>
          </label>
          <select
            id="date"
            className="block w-[300px] appearance-none rounded-lg border border-gray-300 px-4
            py-2 focus:border-purple-950 focus:outline-none focus:ring-1 focus:ring-purple-950"
            name="type"
            value={status}
            onChange={(e) => handleStatus(e.target.value)}
          >
            <option value=""></option>
            <option value={'created'}>{t(`labels.status.created`)}</option>
            <option value={'rejected'}>{t(`labels.status.rejected`)}</option>
            <option value={'approved'}>{t(`labels.status.approved`)}</option>
          </select>
        </div>

        <div className="w-md relative">
          <label
            onClick={() => handleFilterByRegionClear()}
            className="relative mb-1 block text-center font-medium text-gray-700"
          >
            {t('labels.regionFilter')}
            <span className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700">
              {t('labels.clear')}
            </span>
          </label>

          <select
            id="region"
            className="block w-[300px] appearance-none rounded-lg border border-gray-300 px-4 py-2 focus:border-purple-950
            focus:outline-none focus:ring-1 focus:ring-purple-950"
            name="type"
            value={filterByRegion}
            onChange={(e) => handleFilterByRegion(e.target.value)}
          >
            <option value=""></option>
            {Object.values(school.region).map((value, index) => (
              <option key={index} value={value}>
                {t(`regions.${value}`)}
              </option>
            ))}
          </select>
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
        <>
          <div className="mt-3 flex items-center justify-center">
            {isDesktopOrLaptop ? (
              <Table hoverable>
                <Table.Head>
                  <Table.HeadCell>{t('labels.name')}</Table.HeadCell>
                  <Table.HeadCell>
                    {t('labels.requesterFullName')}
                  </Table.HeadCell>
                  <Table.HeadCell>{t('labels.requesterEmail')}</Table.HeadCell>
                  <Table.HeadCell>{t('labels.requesterPhone')}</Table.HeadCell>
                  <Table.HeadCell>{t('labels.region')}</Table.HeadCell>
                  <Table.HeadCell>{t('labels.createdAt')}</Table.HeadCell>
                  <Table.HeadCell>{t('labels.status.label')}</Table.HeadCell>
                  <Table.HeadCell></Table.HeadCell>
                </Table.Head>
                <Table.Body className="divide-y">
                  {data &&
                    data.entries.map((entry, idx) => (
                      <JoiningRequestsItem
                        key={entry.id}
                        item={entry}
                        index={idx}
                      />
                    ))}
                </Table.Body>
              </Table>
            ) : (
              <div className="w-[100%] font-medium">
                {data &&
                  data.entries.map((entry, idx) => (
                    <JoiningRequestsItem
                      key={entry.id}
                      item={entry}
                      index={idx}
                    />
                  ))}
              </div>
            )}
          </div>
        </>
      )}

      <div className="flex w-[100%] justify-center mt-3">
        <button className="w-200 inline-flex items-center justify-center rounded border-0 bg-gray-200 px-6 py-2 text-lg text-gray-700 hover:bg-gray-300 focus:outline-none">
          <Link href={routes.getCreateJoiningRequest(activeLocale)}>
            {w('joiningRequestBtn')}
          </Link>
        </button>
      </div>
    </div>
  );
};

export { JoiningRequests };
