'use client';

import { FC, useEffect, useState } from 'react';
import { usePathname, useRouter, useSearchParams } from 'next/navigation';
import type { ApiResponse } from '@/shared/types';
import type { PagedJoiningRequests } from '@/features/admin/types/joiningRequestTypes';
import { CurrentTab } from '../../constants';
import { getAll } from '../../api/joiningRequestApi';
import { useQuery } from '@tanstack/react-query';
import { Loader } from '@/components/loader';
import { JoiningRequestsItem } from '@/features/admin/components/joining-request/JoiningRequestsItem';
import { Table } from 'flowbite-react';
import { useMediaQuery } from 'react-responsive';
import { useTranslations } from 'next-intl';
import { Error } from '@/components/error';
import { useSetCurrentTab } from '@/shared/hooks';
import { mediaQueries, school } from '@/shared/constants';
import { SearchInput } from '@/components/search-input';
import { buildQueryString } from '@/shared/helpers';

interface JoiningRequestsProps {
  regionParameter?: string,
  sortByDateAscParameter?: string,
  searchParameter?: string
}

const JoiningRequests: FC<JoiningRequestsProps> = ({
    regionParameter,
    sortByDateAscParameter,
    searchParameter
}) => {
  const t = useTranslations('JoiningRequest');
  const pathname = usePathname();
  const { replace } = useRouter();

  const [search, setSearch] = useState<string>(searchParameter || '');
  const handleSearch = (value: string) => {
    console.log('searc', value);
    setSearch(value);
  }

  const [sortByDate, setSortByDate] = useState<string>(sortByDateAscParameter || '');
  const handleSortByDate = (value: string) => {
    setSortByDate(value);
  }
  const handleSortByDateClear = () => {
    if (sortByDate)
      setSortByDate('');
  }

  const [filterByRegion, setFilterByRegion] = useState<string>(regionParameter || '');
  const handleFilterByRegion = (value: string) => {
    setFilterByRegion(value);
  }
  const handleFilterByRegionClear = () => {
    setFilterByRegion('');
  }

  const buildQuery = () => buildQueryString({
    region: regionParameter,
    sortByDateAsc: sortByDateAscParameter,
    schoolName: searchParameter
  });

  const { data, isLoading } = useQuery<ApiResponse<PagedJoiningRequests>>({
    queryKey: ['joiningRequests', regionParameter, sortByDateAscParameter, searchParameter],
    queryFn: () => getAll(buildQuery()),
  });

  useSetCurrentTab(CurrentTab.JoiningRequest);

  const isDesktopOrLaptop = useMediaQuery({
    query: mediaQueries.desktopOrLaptop,
  });

  useEffect(() => {
    const params = new URLSearchParams();

    if (search) params.set('schoolName', search);
    if (filterByRegion) params.set('region', filterByRegion);

    if (sortByDate) {
      const sortByDateParameter = sortByDate === 'asc' ? 'true' : 'false';
      params.set('sortByDateAsc', sortByDateParameter)
    }

    replace(`${pathname}?${params.toString()}`)
  }, [search, sortByDate, filterByRegion]);

  if (data && data.error) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('listTitle')}</h2>
        <Error error={data.error} />
      </>
    );
  }

  if (isLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('listTitle')}</h2>
        <Loader />
      </>
    );
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
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

      <div className={`mt-4 mb-4 flex justify-center items-center ${isDesktopOrLaptop ? 'flex-row gap-8' : 'flex-col gap-6' }`}>
        <div className="relative w-md">
          <label className="block mb-1 font-medium text-gray-700 text-center relative">
            {t('labels.dateSorter.label')}
            <span onClick={() => handleSortByDateClear()}
              className="text-sm pt-1 ml-2 text-purple-700 cursor-pointer hover:text-red-700"
            >
              {t('labels.clear')}
            </span>
          </label>
          <select
            id="date"
            className="block w-[300px] px-4 py-2 border border-gray-300 rounded-lg
            appearance-none focus:outline-none focus:border-purple-950 focus:ring-1 focus:ring-purple-950"
            name="type"
            value={sortByDate}
            onChange={(e) => handleSortByDate(e.target.value)}
          >
            <option value=""></option>
            <option value={'desc'}>
              {t(`labels.dateSorter.desc`)}
            </option>
            <option value={'asc'}>
              {t(`labels.dateSorter.asc`)}
            </option>
          </select>
        </div>

        <div className="relative w-md">
          <label onClick={() => handleFilterByRegionClear()}
            className="block mb-1 font-medium text-gray-700 text-center relative"
          >
            {t('labels.regionFilter')}
            <span className="text-sm pt-1 ml-2 text-purple-700 cursor-pointer hover:text-red-700">
              {t('labels.clear')}
            </span>
          </label>

          <select
            id="region"
            className="block w-[300px] px-4 py-2 border border-gray-300 rounded-lg appearance-none focus:outline-none
            focus:border-purple-950 focus:ring-1 focus:ring-purple-950"
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
      </div>

      <div className="flex items-center justify-center mt-3">
        {isDesktopOrLaptop ? (
          <Table hoverable>
            <Table.Head>
              <Table.HeadCell>{t('labels.name')}</Table.HeadCell>
              <Table.HeadCell>{t('labels.requesterFullName')}</Table.HeadCell>
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
                <JoiningRequestsItem key={entry.id} item={entry} index={idx} />
              ))}
          </div>
        )}
      </div>
    </div>
  );
};

export { JoiningRequests };
