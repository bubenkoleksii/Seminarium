"use client";

import { FC } from 'react';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { Button } from 'flowbite-react';
import { getDefaultProfileImgByType } from '@/shared/helpers';
import { CustomImage } from '@/components/custom-image';
import { useTranslations } from 'next-intl';
import { DateTime } from '@/components/date-time';
import { activate } from '@/features/user/api/schoolProfilesApi';
import { useMutation } from '@tanstack/react-query';
import { userMutations } from '@/features/user/constants';
import { useSchoolProfilesStore } from '@/features/user/store/schoolProfilesStore';
import { toast } from 'react-hot-toast';

interface SchoolProfileProps {
  profile: SchoolProfileResponse;
}

const SchoolProfile: FC<SchoolProfileProps> = ({ profile }) => {
  const t = useTranslations('SchoolProfile');
  const changeActiveProfile = useSchoolProfilesStore(store => store.changeActiveProfile);

  const image = profile.img || getDefaultProfileImgByType(profile.type);

  const { mutate: activateProfile } = useMutation({
    mutationFn: activate,
    mutationKey: [userMutations.activateProfile, profile.id],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          400: t('activateFail'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
        );
      } else {
        changeActiveProfile(profile.id);
        toast.success(t('activateSuccess'), { duration: 1500 });
      }
    }
  });

  const handleActivate = () => {
    activateProfile(profile.id);
  }

  return (
    <div className={`rounded-lg m-4 p-4 min-w-min max-w-xs md:max-w-sm lg:max-w-md xl:max-w-lg relative shadow-xl ${profile.isActive ? 'bg-green-100' : ''}`}>
      <div className="flex justify-center">
        <CustomImage
          src={image}
          width={50}
          height={50}
          alt={profile.type}
        />
      </div>

      <h2 className="text-xl font-semibold text-center">
        {t(`type.${profile.type}`)}
      </h2>

      <p className="mt-2 font-medium text-gray-600 text-xs">
        {t(`isActive`)}:
        {profile.isActive
          ? <span className="ml-1 text-green-600">{t('yes')}</span>
          : <span className="ml-1 text-red-600">{t('no')}</span>}
      </p>
      {profile.schoolName &&
        <p className="mt-2 font-medium text-gray-600 text-xs">
          {t(`item.school`)}: {profile.schoolName}
        </p>
      }
      <p className="mt-2 font-medium text-gray-600 text-xs">
        {t(`item.phone`)}: {profile.phone || '-'}
      </p>
      <p className="mt-2 font-medium text-gray-600 text-xs">
        {t(`item.email`)}: {profile.email || '-'}
      </p>
      <p className="mt-2 font-medium text-gray-600 text-xs">
        {t(`item.createdAt`)}: <DateTime date={profile.createdAt}/>
      </p>
      <p className="mt-2 font-medium text-gray-400 text-xs">
        {t(`item.id`)}: {profile.id}
      </p>

      <div className="w-full mt-2 flex flex-wrap gap-4 justify-center md:flex-nowrap">
        <Button gradientMonochrome="failure" size="xs">
          <span className="text-white">{t('deleteBtn')}</span>
        </Button>

        <Button gradientMonochrome="success" size="xs">
          <span className="text-white">{t('detailsBtn')}</span>
        </Button>

        <Button gradientMonochrome="lime" size="xs">
          <span>{t('updateBtn')}</span>
        </Button>
      </div>

      {!profile.isActive &&
        <div className="absolute top-1 right-1 text-white px-1 py-1">
          <Button onClick={handleActivate} gradientMonochrome="purple" size="xs">
            <span className="text-white">{t('activateBtn')}</span>
          </Button>
        </div>
      }
    </div>
  );
};

export { SchoolProfile };
