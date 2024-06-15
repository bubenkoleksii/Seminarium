'use client';

import { CourseResponse } from '@/features/user/types/courseTypes';
import { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { useTranslations } from 'next-intl';
import { FC } from 'react';

type CourseItemProps = {
  course: CourseResponse;
  activeProfile: SchoolProfileResponse;
};

const CourseItem: FC<CourseItemProps> = ({ course, activeProfile }) => {
  const t = useTranslations('Course');

  console.log('course', course);

  return (
    <div className="mb-4 mt-3 rounded-lg bg-white p-4 text-center shadow-md">
      <h2 className="mb-4 text-xl font-bold">{course.name}</h2>
    </div>
  );
};

export { CourseItem };

