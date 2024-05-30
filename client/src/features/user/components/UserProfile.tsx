'use client';

import { FC } from 'react';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useLocale, useTranslations } from 'next-intl';
import { Loader } from '@/components/loader';
import { Button } from 'flowbite-react';
import { useProfiles } from '@/features/user/hooks';
import { SchoolProfiles } from '@/features/user/components/profile/SchoolProfiles';

const UserProfile: FC = () => {
  const activeLocale = useLocale();
  const t = useTranslations('UserProfile');

  const { isUserLoading, user } = useAuthRedirectByRole(activeLocale, 'userOnly');
  const { profiles, isLoading: profilesLoading, isError: profilesError } = useProfiles();

  if (isUserLoading || !user || profilesLoading) {
    return <Loader />
  }

  if (profilesError) {
    return (
      <p className="text-red-600">
        {t('profilesError')}
      </p>
    )
  }

  return (
    <div className="flex flex-col items-center gap-4">
      <div className="mt-2">
        <h2 className="text-center text-sm md:text-md sm:text-sm lg:text-xl font-semibold">
          {user.name}
        </h2>

        <p className="mt-3 text-sm md:text-md sm:text-sm lg:text text-center font-medium">
          {user.email}
        </p>

        <div className="mt-3">
          <Button
            onClick={() =>
              (window.location.href = `${process.env.NEXT_PUBLIC_AUTH}/Account/ChangeData`)
            }
            gradientMonochrome="lime"
            fullSized
          >
            <span>{t('changeAccountInfo')}</span>
          </Button>
        </div>

        <div className="mt-3">
          <Button
            onClick={() =>
              (window.location.href = `${process.env.NEXT_PUBLIC_AUTH}/Account/ForgotPassword`)
            }
            gradientMonochrome="teal"
            fullSized
          >
            <span className="text-white">{t('forgotPassword')}</span>
          </Button>
        </div>
      </div>

      <div>
        <SchoolProfiles profiles={profiles} />
      </div>
    </div>
  );
};

export { UserProfile };
