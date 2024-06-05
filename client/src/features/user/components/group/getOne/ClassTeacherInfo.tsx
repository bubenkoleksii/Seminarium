import { FC } from 'react';
import { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { getDefaultProfileImgByType } from '@/shared/helpers';
import { CustomImage } from '@/components/custom-image';

type ClassTeacherInfoProps = {
  classTeacher: SchoolProfileResponse;
};

const ClassTeacherInfo: FC<ClassTeacherInfoProps> = ({ classTeacher }) => {
  const t = useTranslations('SchoolProfile');
  const activeLocale = useLocale();
  const { replace } = useRouter();

  const image = classTeacher.img || getDefaultProfileImgByType('class_teacher');

  return (
    <div
      className="relative m-4 w-[300px] min-w-min max-w-md rounded-lg
                     p-4 shadow-xl md:max-w-lg lg:max-w-lg xl:max-w-xl"
    >
      <div className="mt-3 flex justify-center">
        <CustomImage
          src={image}
          width={50}
          height={50}
          alt={classTeacher.name}
        />

        <h2 className="text-center text-xl font-semibold">
          {t(`item.phone`)}: {classTeacher.name}
        </h2>

        <p className="mt-2 text-xs font-medium text-gray-600">
          {t(`item.phone`)}: {classTeacher.phone || '-'}
        </p>
        <p className="mt-2 text-xs font-medium text-gray-600">
          {t(`item.email`)}: {classTeacher.email || '-'}
        </p>
      </div>
    </div>
  );
};

export default ClassTeacherInfo;
