import { FC } from 'react';
import type { SchoolProfileResponse } from '../../types/schoolProfileTypes';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { Button } from 'flowbite-react';

type ChildProps = {
  child: SchoolProfileResponse;
};

const Child: FC<ChildProps> = ({ child }) => {
  const t = useTranslations('SchoolProfile');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  return (
    <div className="relative m-4 w-full min-w-[200px] max-w-[200px] rounded-lg p-4 shadow-xl">
      <p className="mt-1 text-center text-lg font-bold">{child.name}</p>

      <div className="mt-2 flex w-full flex-wrap justify-center gap-4 md:flex-nowrap">
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
