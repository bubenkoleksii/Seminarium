'use client';

import { FC, useEffect, useState } from 'react';
import { useAuthRedirectByRole, useSetCurrentTab } from '@/shared/hooks';
import { useLocale, useTranslations } from 'next-intl';
import { usePathname, useRouter } from 'next/navigation';
import { Error } from '@/components/error';
import { buildQueryString } from '@/shared/helpers';
import {
  CurrentTab,
  schoolOnlyProfileTypes,
  userQueries,
} from '../../constants';
import { mediaQueries } from '@/shared/constants';
import { useMediaQuery } from 'react-responsive';
import { useQuery } from '@tanstack/react-query';
import { getAllBySchool } from '../../api/schoolProfilesApi';
import { ApiResponse } from '@/shared/types';
import { PagesSchoolProfilesResponse } from '../../types/schoolProfileTypes';
import { SearchInput } from '@/components/search-input';
import { Limit, Pagination } from '@/components/pagination';
import { GetAllSchoolProfilesItem } from './GetAllSchoolProfilesItem';
import { Loader } from '@/components/loader';

type GetAllSchoolProfilesProps = {
  typeParameter?: string;
  groupParameter?: string;
  searchParameter?: string;
  limitParameter?: number | null;
  pageParameter?: number | null;
};

const GetAllSchoolProfiles: FC<GetAllSchoolProfilesProps> = ({
  typeParameter,
  groupParameter,
  searchParameter,
  limitParameter,
  pageParameter,
}) => {
  const a = useTranslations('AllSchoolProfiles');

  const activeLocale = useLocale();
  const pathname = usePathname();
  const { replace } = useRouter();

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const [search, setSearch] = useState<string>(searchParameter || '');
  const handleSearch = (value: string) => {
    setSearch(value);
  };

  const [filterByType, setFilterByType] = useState<string>(typeParameter || '');
  const handleFilterByType = (value: string) => {
    setFilterByType(value);
  };
  const handleFilterByTypeClear = () => {
    setFilterByType('');
  };

  const [filterByGroup, setFilterByGroup] = useState<string>(
    groupParameter || '',
  );
  const handleFilterByGroup = (value: string) => {
    setFilterByGroup(value);
  };

  const defaultPage = 1;
  const [page, setPage] = useState<number>(pageParameter || defaultPage);
  const handlePage = (value: number) => {
    setPage(value);
  };

  const limitOptions = [8, 16, 32];
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
      group: groupParameter,
      type: typeParameter,
      take: limitParameter,
      skip,
    });

  useSetCurrentTab(CurrentTab.SchoolProfiles);

  const { data, isLoading } = useQuery<
    ApiResponse<PagesSchoolProfilesResponse>
  >({
    queryKey: [
      userQueries.getAllSchoolProfilesBySchool,
      typeParameter,
      groupParameter,
      searchParameter,
      limitParameter,
      pageParameter,
    ],
    queryFn: () => getAllBySchool(buildQuery()),
    retry: userQueries.options.retry,
  });

  const isDesktopOrLaptop = useMediaQuery({
    query: mediaQueries.desktopOrLaptop,
  });

  useEffect(() => {
    const params = new URLSearchParams();

    if (search) params.set('name', search);
    if (filterByGroup) params.set('group', filterByGroup);
    if (filterByType) params.set('type', filterByType);
    if (limit) params.set('take', limit.toString());
    if (page) params.set('page', page.toString());

    replace(`${pathname}?${params.toString()}`);
  }, [search, filterByGroup, replace, pathname, filterByType, limit, page]);

  if (isLoading || isUserLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{a('listTitle')}</h2>
        <Loader />
      </>
    );
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  }

  if (data && data.error) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{a('listTitle')}</h2>
        <Error error={data.error} />
      </>
    );
  }

  return (
    <div className="p-3">
      <h2 className="mb-4 text-center text-xl font-bold">
        {a('listTitle')} {data?.total ? `(${data.total})` : ''}
      </h2>

      <SearchInput
        maxLength={200}
        value={search}
        placeholder={a('placeholders.name')}
        onSubmit={handleSearch}
      />

      <div
        className={`mb-4 mt-4 flex items-center justify-center ${isDesktopOrLaptop ? 'flex-row gap-8' : 'flex-col gap-6'}`}
      >
        <div className="w-md relative">
          <label className="relative mb-1 block text-center font-medium text-gray-700">
            {a('groupLabel')}
          </label>

          <SearchInput
            maxLength={200}
            value={search}
            placeholder={a('placeholders.group')}
            onSubmit={handleFilterByGroup}
          />
        </div>

        <div className="w-md relative">
          <label
            onClick={() => handleFilterByTypeClear()}
            className="relative mb-1 block text-center font-medium text-gray-700"
          >
            {a('typeLabel')}
            <span className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700">
              {a('clear')}
            </span>
          </label>

          <select
            id="region"
            className="block w-[300px] appearance-none rounded-lg border border-gray-300 px-4 py-2 focus:border-purple-950
            focus:outline-none focus:ring-1 focus:ring-purple-950"
            name="type"
            value={filterByType}
            onChange={(e) => handleFilterByType(e.target.value)}
          >
            <option value=""></option>
            {Object.values(schoolOnlyProfileTypes).map((value, index) => (
              <option key={index} value={value}>
                {a(`types.${value}`)}
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
            {a('notFound')}
          </div>
        </>
      ) : (
        <div className="flex flex-wrap justify-center">
          {data &&
            data.entries.map((entry, idx) => (
              <GetAllSchoolProfilesItem key={idx} schoolProfile={entry} />
            ))}
        </div>
      )}
    </div>
  );
};

export { GetAllSchoolProfiles };
