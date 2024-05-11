'use client';

import { FC, useEffect } from 'react';
import { useAdminStore } from '../../store/adminStore';
import { CurrentTab } from '../../constants';
import { getAll } from '../../api/joiningRequestApi';
import { useQuery } from '@tanstack/react-query';
import { Loader } from '@/components/loader';
import { JoiningRequestsItem } from '@/features/admin/components/joining-request/JoiningRequestsItem';
import { Table } from 'flowbite-react';
import { useMediaQuery } from 'react-responsive';
import { useTranslations } from 'next-intl';

const JoiningRequests: FC = () => {
  const t = useTranslations('JoiningRequest');

  const setCurrentTab = useAdminStore((store) => store.setCurrentTab);
  const isDesktopOrLaptop = useMediaQuery({ query: '(min-width: 1280px)' });

  const { data, status, isLoading, isError, error } = useQuery({
    queryKey: ['joiningRequests'],
    queryFn: () => getAll(),
  });

  useEffect(() => {
    setCurrentTab(CurrentTab.JoiningRequest);
  }, [setCurrentTab]);

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

      <div className="flex items-center justify-center">
          {isDesktopOrLaptop ? (
            <Table hoverable>
              <Table.Head>
                <Table.HeadCell>{t('labels.name')}</Table.HeadCell>
                <Table.HeadCell>{t('labels.requesterFullName')}</Table.HeadCell>
                <Table.HeadCell>{t('labels.requesterEmail')}</Table.HeadCell>
                <Table.HeadCell>{t('labels.requesterPhone')}</Table.HeadCell>
                <Table.HeadCell>{t('labels.region')}</Table.HeadCell>
                <Table.HeadCell>{t('labels.status.label')}</Table.HeadCell>
                <Table.HeadCell></Table.HeadCell>
              </Table.Head>
              <Table.Body className="divide-y">
                {data &&
                  data.entries.map((entry, idx) => (
                    <JoiningRequestsItem key={entry.id} item={entry} index={idx}/>
                  ))}
              </Table.Body>
            </Table>
          ) : (
            <div className="font-medium w-[100%]">
              {data && data.entries.map((entry, idx) => (
                <JoiningRequestsItem key={entry.id} item={entry} index={idx} />
              ))}
          </div>
          )}
      </div>
    </div>
  );
};

export { JoiningRequests };
