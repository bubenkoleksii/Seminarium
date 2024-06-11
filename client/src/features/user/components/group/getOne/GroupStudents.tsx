'use client';

import { GroupStudent } from '@/features/user/components/group/getOne/GroupStudent';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { useTranslations } from 'next-intl';
import { FC } from 'react';

type GroupStudentsProps = {
  students: SchoolProfileResponse[];
};

const GroupStudents: FC<GroupStudentsProps> = ({ students }) => {
  const g = useTranslations('Group');

  if (!students || students.length === 0) {
    return (
      <div className="relative">
        <div className="flex flex-col flex-wrap items-center">
          <p>{g('studentsNotFound')}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="relative mt-2 pt-2">
      <div className="flex flex-wrap justify-center">
        {students.map((student, idx) => (
          <GroupStudent key={idx} student={student} />
        ))}
      </div>
    </div>
  );
};

export { GroupStudents };

