import { FC } from 'react';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { SchoolProfile } from '@/features/user/components/profile/SchoolProfile';
import { useTranslations } from 'next-intl';

type SchoolProfilesProps = {
  profiles: SchoolProfileResponse[]
}

const SchoolProfiles: FC<SchoolProfilesProps> = ({ profiles }) => {
  const t = useTranslations('SchoolProfile');

  if (!profiles || profiles.length === 0) {
    return (
      <div className="relative">
        <h2 className="text-sm md:text lg:text-xl text-center font-semibold mb-2 mt-2">
          {t('listLabel')}
        </h2>
        <div className="flex flex-wrap justify-center">
          <p>
            {t('notFound')}
          </p>
        </div>
      </div>
    )
  }

  return (
    <div className="relative">
      <h2 className="text-sm md:text lg:text-xl text-center font-bold mb-2 mt-2">
        {t('listLabel')} {profiles.length > 0 ? `(${profiles.length})` : ''}
      </h2>
      <div className="flex flex-wrap justify-center">
        {profiles.map(profile => (
          <SchoolProfile key={profile.id} profile={profile} />
        ))}
      </div>
    </div>
  );
};


export { SchoolProfiles };
