import { CourseTeacherResponse } from '@/features/user/types/courseTypes';
import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import Link from 'next/link';
import { FC } from 'react';

interface CourseTeacherProps {
  teacher: CourseTeacherResponse;
  canModify: boolean;
  onDelete: (teacherId: string) => void;
}

const CourseTeacher: FC<CourseTeacherProps> = ({
  teacher,
  onDelete,
  canModify,
}) => {
  const activeLocale = useLocale();
  const t = useTranslations('Course');

  return (
    <div className="mb-2 flex flex-col items-center p-4 shadow-md">
      <Link
        className="mb-3 text-center"
        href={`/${activeLocale}/u/school-profile/${teacher.id}`}
      >
        {teacher.name}
      </Link>

      {teacher.isCreator ? (
        <div className="mb-2">
          <span>{t('creator')}</span>
        </div>
      ) : null}

      {canModify && (
        <Button
          onClick={() => onDelete(teacher.id)}
          gradientMonochrome="failure"
          size="xs"
        >
          <span className="text-white">{t('deleteBtn')}</span>
        </Button>
      )}
    </div>
  );
};

export { CourseTeacher };
