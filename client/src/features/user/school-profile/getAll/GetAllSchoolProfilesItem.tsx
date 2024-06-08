import { FC } from 'react';
import { SchoolProfileResponse } from '../../types/schoolProfileTypes';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { Button } from 'flowbite-react';
import { mediaQueries } from '@/shared/constants';
import { CustomImage } from '@/components/custom-image';
import { useMediaQuery } from 'react-responsive';
import { getDefaultProfileImgByType } from '@/shared/helpers';

type GetAllSchoolProfilesItemProps = {
  schoolProfile: SchoolProfileResponse;
};

const GetAllSchoolProfilesItem: FC<GetAllSchoolProfilesItemProps> = ({
  schoolProfile,
}) => {
  const t = useTranslations('SchoolProfile');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const isPhone = useMediaQuery({ query: mediaQueries.phone });

  return (
    <div className="relative m-4 min-h-[300px] w-full min-w-[200px]  max-w-[200px] rounded-lg p-4 shadow-xl">
      <div className="h-20 overflow-hidden">
        <p className="mt-1 text-center text-lg font-bold">
          {schoolProfile.name}
        </p>
      </div>

      <div className="flex w-[100%] items-center justify-center">
        <CustomImage
          src={
            schoolProfile.img || getDefaultProfileImgByType(schoolProfile.type)
          }
          alt={schoolProfile.name}
          width={isPhone ? 60 : 100}
          height={isPhone ? 60 : 100}
        />
      </div>

      <h2 className="text-center text-sm font-semibold">
        {t(`type.${schoolProfile.type}`)}
      </h2>

      <p className="mt-2 text-xs font-medium text-gray-600">
        {t(`item.phone`)}: {schoolProfile.phone || '-'}
      </p>
      <p className="mt-2 text-xs font-medium text-gray-600">
        {t(`item.email`)}: {schoolProfile.email || '-'}
      </p>

      {schoolProfile.type === 'student' && (
        <p className="mt-2 text-xs font-medium text-gray-600">
          {t(`item.group`)}: {schoolProfile.group?.name || '-'}
        </p>
      )}

      {schoolProfile.type === 'class_teacher' && (
        <p className="mt-2 text-xs font-medium text-gray-600">
          {t(`item.group`)}: {schoolProfile.classTeacherGroup?.name || '-'}
        </p>
      )}

      {schoolProfile.type === 'teacher' && (
        <p className="mt-2 text-xs font-medium text-gray-600">
          {t(`item.teacherSubjects`)}: {schoolProfile.teacherSubjects || '-'}
        </p>
      )}

      <div className="mt-12">
        <Button
          onClick={() =>
            replace(`/${activeLocale}/u/school-profile/${schoolProfile.id}`)
          }
          gradientMonochrome="success"
          size="xs"
          className="absolute bottom-4 left-1/2 -translate-x-1/2 transform"
        >
          <span className="text-white">{t('detailsBtn')}</span>
        </Button>
      </div>
    </div>
  );
};

export { GetAllSchoolProfilesItem };
