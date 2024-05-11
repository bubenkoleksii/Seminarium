'use client';

import { FC, useEffect } from 'react';
import { useAdminStore } from '../store/adminStore';
import { CurrentTab } from '../constants';

const AdminProfile: FC = () => {
  const setCurrentTab = useAdminStore((store) => store.setCurrentTab);

  useEffect(() => {
    setCurrentTab(CurrentTab.Profile);
  }, [setCurrentTab]);

  return <div>Admin profile page</div>;
};

export { AdminProfile };
