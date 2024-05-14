'use client';

import { FC } from 'react';
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
import { mediaQueries } from '@/shared/constants';
import { SearchInput } from '@/components/search-input';

const JoiningRequests: FC = () => {
  const t = useTranslations('JoiningRequest');

  const { data, isLoading } = useQuery<ApiResponse<PagedJoiningRequests>>({
    queryKey: ['joiningRequests'],
    queryFn: () => getAll(),
  });

  useSetCurrentTab(CurrentTab.JoiningRequest);

  const isDesktopOrLaptop = useMediaQuery({
    query: mediaQueries.desktopOrLaptop,
  });

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

  const handleSearch = (value: string) => {
    console.log(value);
  }

  return (
    <div className="p-3">
      <h2 className="mb-4 text-center text-xl font-bold">{t('listTitle')}</h2>

      <SearchInput
        maxLength={200}
        placeholder={t('placeholders.shortName')}
        onSubmit={handleSearch}
      />

      <div className="flex items-center justify-center">
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
