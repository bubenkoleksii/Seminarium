import { FC } from 'react';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { Button } from 'flowbite-react';
import { getDefaultProfileImgByType } from '@/shared/helpers';
import { CustomImage } from '@/components/custom-image';
import { useTranslations } from 'next-intl';

interface SchoolProfileProps {
  profile: SchoolProfileResponse;
}

const SchoolProfile: FC<SchoolProfileProps> = ({ profile }) => {
  const t = useTranslations('SchoolProfile');

  const image = profile.img || getDefaultProfileImgByType(profile.type);

  return (
    <div className={`rounded-lg m-4 p-4 min-w-min max-w-xs relative shadow-xl ${profile.isActive ? 'bg-green-100' : ''}`}>
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
      <p className="mt-2 font-medium text-gray-600 text-xs">
        {t(`item.phone`)}: {profile.phone || '-'}
      </p>
      <p className="mt-2 font-medium text-gray-600 text-xs">
        {t(`item.email`)}: {profile.email || '-'}
      </p>
      <p className="mt-2 font-medium text-gray-400 text-xs">
        {t(`item.id`)}: {profile.id}
      </p>

      <div className="w-[100%] mt-2 left-4 flex gap-4 justify-center">
        <Button gradientMonochrome="failure" size="sm">
          <span className="text-white">{t('deleteBtn')}</span>
        </Button>

        <Button gradientMonochrome="success" size="sm">
          <span className="text-white">{t('detailsBtn')}</span>
        </Button>

        <Button gradientMonochrome="lime" size="sm">
          <span>{t('updateBtn')}</span>
        </Button>
      </div>

      {!profile.isActive &&
        <div className="absolute top-1 right-1 text-white px-1 py-1">
          <Button gradientMonochrome="purple" size="xs">
            <span className="text-white">{t('activateBtn')}</span>
          </Button>
        </div>
      }
    </div>
  );
};

export { SchoolProfile };
