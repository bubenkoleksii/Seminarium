import { FC } from 'react';
import type { GroupResponse } from '@/features/user/types/groupTypes';
import { useTranslations } from 'next-intl';
import { CustomImage } from '@/components/custom-image';
import { Button } from 'flowbite-react';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';

type GroupsItemProps = {
  group: GroupResponse;
  activeProfile: SchoolProfileResponse
};

const GroupsItem: FC<GroupsItemProps> = ({ group, activeProfile }) => {
  const t = useTranslations('Group');

  const image = group.img || '/group/group.png';

  return (
    <div
      className="relative m-4 min-w-min max-w-md rounded-lg p-4
                     shadow-xl w-[300px] md:max-w-lg lg:max-w-lg xl:max-w-xl"
    >
      <div className="flex justify-center">
        <CustomImage src={image} width={120} height={100} alt={group.name} />
      </div>

      <h2 className="text-center text-xl font-semibold">{group.name}</h2>

      <p className="mt-2 text-sm text-center font-medium text-gray-600">
        {t(`labels.studyPeriodNumberShort`)}: {group.studyPeriodNumber}
      </p>

      {activeProfile?.type === 'school_admin' &&
        <div className="flex justify-center mt-2">
          <Button
            onClick={() => console.log('2')}
            gradientDuoTone="redToYellow"
            size="xs"
          >
            <span className="text-gray-900">{t('classTeacherInvitationBtn')}</span>
          </Button>
        </div>
      }

      <div className="flex justify-center mt-2">
        <Button
          onClick={() => console.log('2')}
          gradientMonochrome="purple"
          size="xs"
        >
          <span className="text-white">{t('studentInvitationBtn')}</span>
        </Button>
      </div>

      <div className="mt-2 flex w-full flex-wrap justify-center gap-4 md:flex-nowrap">
        <Button gradientMonochrome="failure" size="xs">
          <span className="text-white">{t('deleteBtn')}</span>
        </Button>

        <Button gradientMonochrome="success" size="xs">
          <span className="text-white">{t('detailsBtn')}</span>
        </Button>

        <Button gradientMonochrome="lime" size="xs">
          <span>{t('updateBtn')}</span>
        </Button>
      </div>
    </div>
  );
};

export { GroupsItem };
