'use client';

import { FC, useEffect } from 'react';
import { useAdminStore } from '../../store/adminStore';
import { CurrentTab } from '../../constants';
import { getAll } from '../../api/joiningRequestApi';
import { useQuery } from '@tanstack/react-query';
import { Loader } from '@/components/loader';
import { JoiningRequestsItem } from '@/features/admin/components/joining-request/JoiningRequestsItem';
import { Table } from "flowbite-react";
import { useMediaQuery } from 'react-responsive';

const JoiningRequests: FC = () => {
  const setCurrentTab = useAdminStore((store) => store.setCurrentTab);
  const isDesktopOrLaptop = useMediaQuery({ query: '(min-width: 1280px)' });

  const { data, status, isLoading, isError, error } = useQuery({
    queryKey: ['joiningRequests'],
    queryFn: () => getAll(),
  });

  useEffect(() => {
    setCurrentTab(CurrentTab.JoiningRequest);
  }, [setCurrentTab]);

  if (isLoading) return (
    <>
      <h2>Joining requests</h2>
      <Loader />
    </>
  );

  return (
    <div className="p-4">
      <h2 className="text-xl font-bold mb-4">Joining requests</h2>

      <Table hoverable>
        {isDesktopOrLaptop
          ? <>
            <Table.Head>
              <Table.HeadCell>Name</Table.HeadCell>
              <Table.HeadCell>Requester</Table.HeadCell>
              <Table.HeadCell>Email</Table.HeadCell>
              <Table.HeadCell>Phone</Table.HeadCell>
              <Table.HeadCell>Region</Table.HeadCell>
              <Table.HeadCell>Status</Table.HeadCell>
              <Table.HeadCell>Details</Table.HeadCell>
            </Table.Head>
            <Table.Body className="divide-y">
              {data && data.entries.map((entry) => (
                <JoiningRequestsItem key={entry.id} item={entry} />
              ))}
            </Table.Body>
          </>
          : <Table.Body>
            {data && data.entries.map((entry) => (
              <JoiningRequestsItem key={entry.id} item={entry} />
            ))}
          </Table.Body>
        }
      </Table>
    </div>
  );
};

export { JoiningRequests };
