import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import type { SchoolProfileResponse } from '../../types/schoolProfileTypes';

type ChildProps = {
  child: SchoolProfileResponse;
};

const Child: FC<ChildProps> = ({ child }) => {
  const t = useTranslations('SchoolProfile');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  return (
    <div className="relative m-4 w-full min-w-[200px] max-w-[200px] rounded-lg p-4 shadow-xl">
      <p className="mt-1 h-16 text-center text-lg font-bold">{child.name}</p>

      <div className="mt-2 flex w-full flex-col justify-center gap-4 md:flex-nowrap">
        <Button
          onClick={() =>
            replace(`/${activeLocale}/u/courses/?groupId=${child.groupId}`)
          }
          gradientMonochrome="pink"
          size="xs"
        >
          <span className="text-white">{t('courses')}</span>
        </Button>

        <Button
          onClick={() =>
            replace(
              `/${activeLocale}/u/practical-item/getStudentAll/?studentId=${child.id}/`,
            )
          }
          gradientMonochrome="purple"
          size="xs"
        >
          <span className="text-white">{t('practical')}</span>
        </Button>

        <Button
          onClick={() => replace(`/${activeLocale}/u/groups/${child.groupId}`)}
          gradientMonochrome="lime"
          size="xs"
        >
          <span>{t('group')}</span>
        </Button>

        <Button
          onClick={() =>
            replace(`/${activeLocale}/u/school-profile/${child.id}`)
          }
          gradientMonochrome="success"
          size="xs"
        >
          <span className="text-white">{t('detailsBtn')}</span>
        </Button>
      </div>
    </div>
  );
};

export { Child };
