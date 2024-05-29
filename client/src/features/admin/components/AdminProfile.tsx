'use client';

import { FC, useEffect } from 'react';
import { useAdminStore } from '../store/adminStore';
import { CurrentTab } from '../constants';
import { useLocale, useTranslations } from 'next-intl';
import { Loader } from '@/components/loader';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { Button } from 'flowbite-react';
import { redirect } from 'next/navigation';
import { updateSchoolClientPath } from '@/features/school/constants';

const AdminProfile: FC = () => {
  const t = useTranslations('AdminProfile');
  const activeLocale = useLocale();

  const setCurrentTab = useAdminStore((store) => store.setCurrentTab);
  const { isUserLoading, user } = useAuthRedirectByRole(activeLocale, 'admin');

  useEffect(() => {
    setCurrentTab(CurrentTab.Profile);
  }, [setCurrentTab]);

  if (isUserLoading || !user) {
    return (
      <>
        <Loader />
      </>
    );
  }

  return (
    <div className="mt-5 p-3 bg-white rounded shadow-md max-w-md mx-auto">
      <h2 className="mb-4 text-center text-2xl font-bold">{t('label')}</h2>
      <div className="flex flex-col">
        <div className="mb-4 pl-6">
          <label className="block text-lg font-medium">
            {t('nameLabel')}
          </label>
          <div className="text-lg font-semibold">
            {user?.name ? user.name : '-'}
          </div>
        </div>
        <div className="mb-4 pl-6">
          <label className="block text font-medium">
            {t('emailLabel')}
          </label>
          <div className="text-sm font-semibold">
            {user?.email ? user.email : '-'}
          </div>
        </div>
        <div className="pl-6">
          <Button
            onClick={() => window.location.href = `${process.env.NEXT_PUBLIC_AUTH}/Account/ChangeData`}
            gradientMonochrome="lime" fullSized
          >
            <span>
              {t('changeAccountInfo')}
            </span>
          </Button>
        </div>`
        <div className="mb-4 pl-6">
          <Button
            onClick={() => window.location.href = `${process.env.NEXT_PUBLIC_AUTH}/Account/ForgotPassword`}
            gradientMonochrome="teal" fullSized
          >
            <span className="text-white">
              {t('forgotPassword')}
            </span>
          </Button>
        </div>
      </div>
    </div>
  );
};

export { AdminProfile };
