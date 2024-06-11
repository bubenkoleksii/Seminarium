'use client';

import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { Limit, Pagination } from '@/components/pagination';
import { SearchInput } from '@/components/search-input';
import { useProfiles } from '@/features/user';
import { getAllGroupNotices } from '@/features/user/api/groupNoticesApi';
import { CurrentTab, userQueries } from '@/features/user/constants';
import { PagesGroupNoticeResponse } from '@/features/user/types/groupNoticesTypes';
import { mediaQueries } from '@/shared/constants';
import { buildQueryString } from '@/shared/helpers';
import { useAuthRedirectByRole, useSetCurrentTab } from '@/shared/hooks';
import { ApiResponse } from '@/shared/types';
import { useQuery } from '@tanstack/react-query';
import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import { usePathname, useRouter } from 'next/navigation';
import { FC, useEffect, useState } from 'react';
import { useMediaQuery } from 'react-responsive';
import { GroupNoticeItem } from './GroupNoticeItem';

type GetAllGroupNoticesProps = {
  groupId: string;
  myOnlyParameter: boolean | string;
  searchParameter?: string;
  limitParameter?: number | null;
  pageParameter?: number | null;
};

const GetAllGroupNotices: FC<GetAllGroupNoticesProps> = ({
  groupId,
  myOnlyParameter,
  searchParameter,
  limitParameter,
  pageParameter,
}) => {
  const activeLocale = useLocale();
  const t = useTranslations('GroupNotice');

  const pathname = usePathname();
  const { replace } = useRouter();

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');
  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const isDesktopOrLaptop = useMediaQuery({
    query: mediaQueries.desktopOrLaptop,
  });

  const [search, setSearch] = useState<string>(searchParameter || '');
  const handleSearch = (value: string) => {
    setSearch(value);
  };

  const [myOnly, setMyOnly] = useState<boolean>(
    myOnlyParameter ? Boolean(myOnlyParameter) : false,
  );
  const handleFilterByMyOnly = (value: boolean) => {
    setMyOnly(value);
  };
  const handleFilterByMyOnlyClear = () => {
    setMyOnly(false);
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
      search: searchParameter,
      myOnly: myOnlyParameter,
      groupId,
      take: limitParameter,
      skip,
    });

  const { data, isLoading, refetch } = useQuery<ApiResponse<PagesGroupNoticeResponse>>({
    queryFn: () =>
      getAllGroupNotices({
        query: buildQuery(),
      }),
    queryKey: [
      userQueries.getGroupNotices,
      searchParameter,
      myOnlyParameter,
      groupId,
      limitParameter,
      pageParameter,
    ],
    retry: userQueries.options.retry,
  });

  useEffect(() => {
    refetch();
  }, [activeProfile, refetch]);

  useEffect(() => {
    const params = new URLSearchParams();

    if (search) params.set('search', search);
    if (myOnly) params.set('myOnly', myOnly.toString());

    if (limit) params.set('take', limit.toString());
    if (page) params.set('page', page.toString());

    replace(`${pathname}?${params.toString()}`);
  }, [search, myOnly, replace, limit, page]);

  useSetCurrentTab(CurrentTab.GroupNotices);

  if (isLoading || isUserLoading || profilesLoading) {
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
        <h2 className="mb-4 text-center text-xl font-bold">{t('listTitle')}
          <p
            onClick={() => replace(`/${activeLocale}/u/groups/${groupId}`)}
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('backTo')}
          </p>
        </h2>

        <Error error={data.error} />
      </>
    );
  }

  if (data && data.total == 0) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('listTitle')}
          <p
            onClick={() => replace(`/${activeLocale}/u/groups/${groupId}`)}
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('backTo')}
          </p>
        </h2>

        <div className={`flex flex-row justify-center pb-3 w-full ${isDesktopOrLaptop ? 'flex-row gap-8' : 'flex-col gap-6'}`}>
          <Button
            gradientMonochrome="success"
            size="md"
            onClick={() =>
              replace(`/${activeLocale}/u/group-notices/create/${groupId}`)
            }
          >
            <span className="text-white">{t('createBtn')}</span>
          </Button>
        </div>

        <p className="text-center text-red-700 w-[100%]">{t('notFound')}</p>
      </>
    )
  }

  let total = data.crucialNotices.length + data.regularNotices.length;
  if (data.lastNotice)
    total++;

  return (
    <div className="p-3">
      <h2 className="mb-2 text-center text-xl font-bold">
        {t('listTitle')} {data?.total ? `(${data.total})` : ''}

        <p
          onClick={() => replace(`/${activeLocale}/u/groups/${groupId}`)}
          className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
        >
          {t('backTo')}
        </p>
      </h2>

      <div className="flex pb-3 justify-center pb-3 w-full">
        <Button
          gradientMonochrome="success"
          size="md"
          onClick={() =>
            replace(`/${activeLocale}/u/group-notices/create/${groupId}`)
          }
        >
          <span className="text-white">{t('createBtn')}</span>
        </Button>
      </div>

      {data.lastNotice &&
        <div className="mb-4">
          <h2 className="text-xl mt-3 text-center text-sm font-bold">
            {t('lastNotice')}
          </h2>
          <GroupNoticeItem
            notice={data.lastNotice}
            activeProfile={activeProfile}
            groupId={groupId}
          />
        </div>
      }

      <SearchInput
        maxLength={200}
        value={search}
        placeholder={t('placeholders.search')}
        onSubmit={handleSearch}
      />

      <div className="w-full flex mt-2 mb-2 justify-center items-center">
        <div className="w-md relative flex gap-1 items-center">
          <input
            id="myOnly"
            className="block cursor-pointer w-8 h-8 rounded-lg pb-3 appearance-none border border-gray-300
                        py-2 focus:border-purple-950 focus:outline-none focus:ring-1 focus:ring-purple-950"
            name="myOnly"
            type="checkbox"
            checked={myOnly}
            onChange={(e) => handleFilterByMyOnly(e.target.checked)}
          />

          <label className="relative block text-center font-medium text-gray-700">
            {t('myOnly')}
            <span
              onClick={handleFilterByMyOnlyClear}
              className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
            >
              {t('clear')}
            </span>
          </label>
        </div>

        <div className="w-md relative">
          <Limit
            limitOptions={limitOptions}
            currentLimit={limit}
            onChangeLimit={(limit) => handleLimit(limit)}
          />
        </div>
      </div>

      {data && total < data.total && (
        <div className="mt-3">
          <Pagination
            currentPage={page}
            totalCount={data.total}
            limit={data.take}
            onChangePage={(page) => handlePage(page)}
          />
        </div>
      )}

      {
        data.crucialNotices && data.crucialNotices.length > 0 &&
        <>
          <h2 className="text-xl mt-4 text-center text-sm font-bold">
            {t('crucialNotices')}
          </h2>

          {data.crucialNotices.map((notice, idx) =>
            <GroupNoticeItem key={idx} activeProfile={activeProfile} notice={notice} groupId={groupId} />)
          }
        </>
      }

      {
        data.regularNotices && data.regularNotices.length > 0 &&
        <>
          <h2 className="text-xl mt-[30px] text-center text-sm font-bold">
            {t('regularNotices')}
          </h2>

          {data.regularNotices.map((notice, idx) =>
            <GroupNoticeItem key={idx} activeProfile={activeProfile} notice={notice} groupId={groupId} />)
          }
        </>
      }

      {data && total < data.total && (
        <div className="mt-3">
          <Pagination
            currentPage={page}
            totalCount={data.total}
            limit={data.take}
            onChangePage={(page) => handlePage(page)}
          />
        </div>
      )}
    </div >
  );
};

export { GetAllGroupNotices };

