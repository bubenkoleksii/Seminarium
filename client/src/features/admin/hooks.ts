'use client';

import { useEffect } from 'react';
import { CurrentTab } from '@/features/admin/constants';
import { useAdminStore } from '@/features/admin/store/adminStore';

export const useSetCurrentTab = (currentTab: CurrentTab) => {
  const setCurrentTab = useAdminStore((store) => store.setCurrentTab);

  useEffect(() => {
    setCurrentTab(currentTab);
  }, [setCurrentTab, currentTab]);
};
