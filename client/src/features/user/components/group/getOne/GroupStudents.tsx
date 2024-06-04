'use client';

import { FC } from 'react';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { useTranslations } from 'next-intl';
import { GroupStudent } from '@/features/user/components/group/getOne/GroupStudent';

type GroupStudentsProps = {
  students: SchoolProfileResponse[];
}

const GroupStudents: FC<GroupStudentsProps> = ({ students }) => {
  const g = useTranslations('Group');

  if (!students || students.length === 0) {
    return (
      <div className="relative mt-2">
        <h2 className="md:text mb-2 mt-2 text-center text-sm font-semibold lg:text-xl">
          {g('students')}
        </h2>
        <div className="flex flex-col flex-wrap items-center">
          <p>{g('studentsNotFound')}</p>
        </div>
      </div>
    )
  }

  return (
    <div className="relative mt-2 pt-2">
      <h2 className="md:text mb-2 mt-2 text-center text-sm font-bold lg:text-xl">
        {g('students')} {students.length > 0 ? `(${students.length})` : ''}
      </h2>

      <div className="flex flex-wrap justify-center">
        {students.map((student, idx) =>
          <GroupStudent key={idx} student={student} />
        )}
      </div>
    </div>
  );
};

export { GroupStudents };
