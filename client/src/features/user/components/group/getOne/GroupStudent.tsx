import { FC } from 'react';
import { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { useLocale, useTranslations } from 'next-intl';
import { getColorByStatus, getDefaultProfileImgByType } from '@/shared/helpers';
import { CustomImage } from '@/components/custom-image';
import { Button } from 'flowbite-react';
import { useSchoolProfilesStore } from '@/features/user';
import { DateOnly } from '@/components/date-time';
import { useRouter } from 'next/navigation';

type GroupStudentProps = {
  student: SchoolProfileResponse;
};

const GroupStudent: FC<GroupStudentProps> = ({ student }) => {
  const t = useTranslations('SchoolProfile');
  const activeProfile = useSchoolProfilesStore((store) => store.activeProfile);

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const image = student.img || getDefaultProfileImgByType('student');
  const amI = student.id == activeProfile?.id;

  const healthGroupColor = getColorByStatus('free');

  return (
    <div
      className={`relative m-4 min-w-min max-w-xs rounded-lg p-4 shadow-xl md:max-w-sm lg:max-w-md xl:max-w-lg 
      ${amI ? 'bg-green-100' : student.studentIsClassLeader ? 'bg-purple-200' : ''}`}
    >
      <div className="flex justify-center">
        <CustomImage src={image} width={50} height={50} alt={student.name} />
      </div>

      <p className="mt-1 text-center text-lg font-bold">{student.name}</p>
      {student.studentIsClassLeader ? (
        <p className="mt-1 text-center text-lg font-semibold">
          {t('item.studentIsClassLeader')}
        </p>
      ) : (
        <span>&nbsp;</span>
      )}
      <p className="mt-2 text-sm font-semibold text-gray-600">
        {t(`item.studentHealthGroup`)}:
        {student.studentHealthGroup ? (
          <span className={healthGroupColor}>
            {t(`item.${student.studentHealthGroup}`)}
          </span>
        ) : (
          ' -'
        )}
      </p>
      <p className="mt-2 text-xs font-medium text-gray-600">
        {t(`item.studentDateOfBirth`)}:
        {student.studentDateOfBirth ? (
          <DateOnly date={student.studentDateOfBirth} showDayOfWeek={false} />
        ) : (
          ' -'
        )}
      </p>
      <p className="mt-2 text-xs font-medium text-gray-600">
        {t(`item.phone`)}: {student.phone || '-'}
      </p>
      <p className="mt-2 text-xs font-medium text-gray-600">
        {t(`item.email`)}: {student.email || '-'}
      </p>

      <div className="mt-2 flex w-full flex-wrap justify-center gap-4 md:flex-nowrap">
        <Button onClick={() => replace(`/${activeLocale}/u/school-profile/${student.id}`)}
          gradientMonochrome="success" size="xs">
          <span className="text-white">{t('detailsBtn')}</span>
        </Button>
      </div>
    </div>
  );
};

export { GroupStudent };
