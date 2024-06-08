import { FC } from 'react';
import type { GroupResponse } from '@/features/user/types/groupTypes';
import { useLocale, useTranslations } from 'next-intl';
import { CustomImage } from '@/components/custom-image';
import { Button } from 'flowbite-react';
import { useRouter } from 'next/navigation';

type GroupsItemProps = {
  group: GroupResponse;
};

const GroupsItem: FC<GroupsItemProps> = ({ group }) => {
  const t = useTranslations('Group');
  const activeLocale = useLocale();
  const { replace } = useRouter();

  const image = group.img || '/group/group.png';

  return (
    <div
      className="relative m-4 w-[300px] min-w-min max-w-md rounded-lg
                     p-4 shadow-xl md:max-w-lg lg:max-w-lg xl:max-w-xl"
    >
      <div className="mt-3 flex justify-center">
        <CustomImage src={image} width={120} height={100} alt={group.name} />
      </div>

      <h2 className="text-center text-xl font-semibold">{group.name}</h2>

      <p className="mt-2 text-center text-sm font-medium text-gray-600">
        {t(`labels.studyPeriodNumberShort`)}: {group.studyPeriodNumber}
      </p>

      <div className="mt-2 flex w-full flex-wrap justify-center gap-4 md:flex-nowrap">
        <Button
          onClick={() => replace(`/${activeLocale}/u/groups/${group.id}`)}
          gradientMonochrome="success"
          size="md"
        >
          <span className="text-white">{t('detailsBtn')}</span>
        </Button>
      </div>
    </div>
  );
};

export { GroupsItem };
