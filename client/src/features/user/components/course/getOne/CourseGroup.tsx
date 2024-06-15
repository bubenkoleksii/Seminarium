import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import Link from 'next/link';
import { FC } from 'react';

interface CourseGroupProps {
  group: { id: string; name: string };
  onDelete: (groupId: string) => void;
}

const CourseGroup: FC<CourseGroupProps> = ({ group, onDelete }) => {
  const activeLocale = useLocale();
  const t = useTranslations('Course');

  return (
    <div className="mb-2 flex flex-col items-start">
      <Link href={`/${activeLocale}/u/groups/${group.id}`}>
        <a className="text-blue-500 hover:underline">{group.name}</a>
      </Link>
      <Button
        onClick={() => onDelete(group.id)}
        gradientMonochrome="failure"
        size="xs"
      >
        <span className="text-white">{t('deleteBtn')}</span>
      </Button>
    </div>
  );
};

export { CourseGroup };

