import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import Link from 'next/link';
import { FC } from 'react';

interface CourseGroupProps {
  group: { id: string; name: string };
  onDelete: (groupId: string) => void;
  canModify: boolean;
}

const CourseGroup: FC<CourseGroupProps> = ({ group, onDelete, canModify }) => {
  const activeLocale = useLocale();
  const t = useTranslations('Course');

  return (
    <div className="mb-2 flex flex-col items-center p-4 shadow-md">
      <Link
        className="mb-3 text-center"
        href={`/${activeLocale}/u/groups/${group.id}`}
      >
        {group.name}
      </Link>
      {canModify &&
        <Button
          onClick={() => onDelete(group.id)}
          gradientMonochrome="failure"
          size="xs"
        >
          <span className="text-white">{t('deleteBtn')}</span>
        </Button>
      }
    </div>
  );
};

export { CourseGroup };

