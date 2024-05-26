'use client';

import { FC, useEffect } from 'react';
import { useAdminStore } from '../store/adminStore';
import { CurrentTab } from '../constants';
import { useLocale, useTranslations } from 'next-intl';
import { Loader } from '@/components/loader';
import { useAuthRedirectByRole } from '@/shared/hooks';

const AdminProfile: FC = () => {
  const t = useTranslations('AdminProfile');
  const activeLocale = useLocale();

  const setCurrentTab = useAdminStore((store) => store.setCurrentTab);
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'admin');

  useEffect(() => {
    setCurrentTab(CurrentTab.Profile);
  }, [setCurrentTab]);

  if (isUserLoading) {
    return (
      <>
        <Loader />
      </>
    );
  }

  return (
    <div className="p-3">
      <h2 className="mb-4 text-center text-xl font-bold">{t('label')}</h2>


    </div>
  );
};

export { AdminProfile };
