'use client'

import { FC, useEffect } from 'react';
import { useAdminStore } from '../../store/adminStore';
import { CurrentTab } from '../../types';
import { getAll } from '../../api/joiningRequestApi';
import { useQuery } from '@tanstack/react-query';
import { Loader } from '@/components/loader';

const JoiningRequests: FC = () => {
  const setCurrentTab = useAdminStore(store => store.setCurrentTab);

  const { data, status, isLoading, isError, error } = useQuery({
    queryKey: ['joiningRequests'],
    queryFn: () => getAll()
  });

  useEffect(() => {
    setCurrentTab(CurrentTab.JoiningRequest);
  }, []);

  if (isLoading)
    return <Loader />

  return (
    <div>
      {data && JSON.stringify(data)}
    </div>
  );
};

export { JoiningRequests };
