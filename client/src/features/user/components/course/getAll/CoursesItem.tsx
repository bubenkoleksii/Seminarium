'use client';

import { CourseResponse } from '@/features/user/types/courseTypes';
import { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';

type CourseItemProps = {
  course: CourseResponse;
  activeProfile: SchoolProfileResponse;
};

const CourseItem: FC<CourseItemProps> = ({ course }) => {
  const t = useTranslations('Course');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  return (
    <div className="mb-4 mt-3 rounded-lg bg-white p-4 text-center shadow-md">
      <h2 className="mb-4 text-xl font-bold">{course.name}</h2>

      <div className="mt-2 flex w-full flex-wrap justify-center gap-4 md:flex-nowrap">
        <Button
          onClick={() => replace(`/${activeLocale}/u/courses/${course.id}`)}
          gradientMonochrome="success"
          size="xs"
        >
          <span className="text-white">{t('detailsBtn')}</span>
        </Button>
      </div>
    </div>
  );
};

export { CourseItem };
