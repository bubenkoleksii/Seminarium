'use client'

import { FC, useEffect } from 'react';
import { useAdminStore } from '../store/adminStore';
import { CurrentTab } from '@/features/admin/types';

const AdminProfile: FC = () => {
  const setCurrentTab = useAdminStore(store => store.setCurrentTab);

  useEffect(() => {
    setCurrentTab(CurrentTab.Profile)
  }, []);

  return (
    <div>
      Admin profile page
    </div>
  );
};

export { AdminProfile };
