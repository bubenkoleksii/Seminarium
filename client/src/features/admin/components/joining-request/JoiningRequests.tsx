'use client'

import { FC, useEffect } from 'react';
import { useAdminStore } from '@/features/admin/store/adminStore';
import { CurrentTab } from '@/features/admin/types';

const JoiningRequests: FC = () => {
  const setCurrentTab = useAdminStore(store => store.setCurrentTab);

  useEffect(() => {
    setCurrentTab(CurrentTab.JoiningRequest)
  }, []);

  return (
    <div>
      Joining Requests
    </div>
  );
};

export { JoiningRequests };
