'use client';

import { useEffect, useState } from 'react';
import { useAdminStore } from '@/features/admin/store/adminStore';

export const useSetCurrentTab = (currentTab) => {
  const setCurrentTab = useAdminStore((store) => store.setCurrentTab);

  useEffect(() => {
    setCurrentTab(currentTab);
  }, [setCurrentTab, currentTab]);
};

export const useScrollOffset = () => {
  const [topOffset, setTopOffset] = useState(0);

  const handleScroll = () => {
    const offset =
      window.pageYOffset ||
      document.documentElement.scrollTop ||
      document.body.scrollTop;

    setTopOffset(offset);
  };

  useEffect(() => {
    window.addEventListener('scroll', handleScroll);

    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  }, []);

  return topOffset;
};
