import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import Link from 'next/link';
import { FC } from 'react';

interface CourseTeacherProps {
  teacher: { id: string; name: string };
  onDelete: (teacherId: string) => void;
}

const CourseTeacher: FC<CourseTeacherProps> = ({ teacher, onDelete }) => {
  const activeLocale = useLocale();
  const t = useTranslations('Course');

  return (
    <div className="mb-2 flex flex-col items-start">
      <Link href={`/${activeLocale}/u/school-profile/${teacher.id}`}>
        <a className="text-blue-500 hover:underline">{teacher.name}</a>
      </Link>
      <Button
        onClick={() => onDelete(teacher.id)}
        gradientMonochrome="failure"
        size="xs"
      >
        <span className="text-white">{t('deleteBtn')}</span>
      </Button>
    </div>
  );
};

export { CourseTeacher };

