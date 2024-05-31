'use client';

import { FC } from 'react';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useLocale, useTranslations } from 'next-intl';
import { Loader } from '@/components/loader';
import { Button } from 'flowbite-react';
import { useProfiles } from '@/features/user/hooks';
import { SchoolProfiles } from '@/features/user/components/profile/SchoolProfiles';
import { useIsMutating } from '@tanstack/react-query';
import { useSchoolProfilesStore } from '@/features/user/store/schoolProfilesStore';
import { routes } from '@/shared/constants';
import Link from 'next/link';

const UserProfile: FC = () => {
  const activeLocale = useLocale();
  const t = useTranslations('UserProfile');
  const w = useTranslations('Welcome');

  const isMutating = useIsMutating();
  const { isUserLoading, user } = useAuthRedirectByRole(
    activeLocale,
    'userOnly',
  );
  const { isLoading: profilesLoading, isError: profilesError } = useProfiles();
  const profiles = useSchoolProfilesStore((store) => store.profiles);

  if (isUserLoading || !user || profilesLoading || isMutating) {
    return <Loader />;
  }

  if (profilesError) {
    return <p className="text-red-600">{t('profilesError')}</p>;
  }

  return (
    <div className="flex flex-col items-center gap-4">
      <div className="mt-2">
        <h2 className="md:text-md text-center text-sm font-semibold sm:text-sm lg:text-xl">
          {user.name}
        </h2>

        <p className="md:text-md lg:text mt-3 text-center text-sm font-medium sm:text-sm">
          {user.email}
        </p>

        <div className="mt-3 flex justify-center">
          <Button
            onClick={() =>
              (window.location.href = `${process.env.NEXT_PUBLIC_AUTH}/Account/ChangeData`)
            }
            gradientMonochrome="lime"
            size="md"
          >
            <span>{t('changeAccountInfo')}</span>
          </Button>
        </div>

        <div className="mt-3 flex justify-center">
          <Button
            onClick={() =>
              (window.location.href = `${process.env.NEXT_PUBLIC_AUTH}/Account/ForgotPassword`)
            }
            gradientMonochrome="teal"
            size="md"
          >
            <span className="text-white">{t('forgotPassword')}</span>
          </Button>
        </div>

        <div className="mt-3 flex justify-center">
          <button className="inline-flex w-[250px] items-center justify-center rounded border-0 bg-gray-200 px-6 py-2 text-lg text-gray-700 hover:bg-gray-300 focus:outline-none">
            <Link href={routes.getCreateJoiningRequest(activeLocale)}>
              {w('joiningRequestBtn')}
            </Link>
          </button>
        </div>
      </div>

      <div>
        <SchoolProfiles profiles={profiles} />
      </div>
    </div>
  );
};

export { UserProfile };
