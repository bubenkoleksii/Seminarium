import { DateOnly } from '@/components/date-time';
import { ProveModal } from '@/components/modal';
import { removeStudyPeriod } from '@/features/user/api/studyPeriodApi';
import { userMutations, userQueries } from '@/features/user/constants';
import { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { StudyPeriodResponse } from '@/features/user/types/studyPeriodTypes';
import { buildQueryString } from '@/shared/helpers';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC, useState } from 'react';
import toast from 'react-hot-toast';

type StudyPeriodsItemProps = {
  studyPeriod: StudyPeriodResponse;
  activeProfile: SchoolProfileResponse;
  index: number;
};

const StudyPeriodsItem: FC<StudyPeriodsItemProps> = ({
  studyPeriod,
  index,
  activeProfile,
}) => {
  const t = useTranslations('StudyPeriod');
  const v = useTranslations('Validation');

  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const queryClient = useQueryClient();

  const { mutate: deleteStudyPeriod } = useMutation({
    mutationFn: removeStudyPeriod,
    mutationKey: [userMutations.deleteStudyPeriod, studyPeriod.id],
    onSuccess: (response) => {
      if (response && response.error) {
        if (
          response.error.detail.includes('school_profile') ||
          response.error.detail.includes('school_id')
        ) {
          toast.error(v('invalid_school_profile'));
          return;
        }

        const errorMessages = {
          404: t('labels.oneNotFound'),
          400: v('invitationValidation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('labels.deleteSuccess'), { duration: 2500 });
      }
    },
    onSettled: async () => {
      queryClient.invalidateQueries({
        queryKey: [userQueries.getStudyPeriods],
        refetchType: 'all',
      });
    },
  });

  const buildUpdateQuery = () =>
    buildQueryString({
      id: studyPeriod.id,
      startDate: studyPeriod.startDate,
      endDate: studyPeriod.endDate,
    });

  const handleOpenDeleteModal = () => {
    setIsDeleteModalOpen(true);
  };

  const handleCloseDeleteModal = (confirmed: boolean) => {
    setIsDeleteModalOpen(false);

    if (!confirmed) return;

    deleteStudyPeriod({
      id: studyPeriod.id,
      schoolProfileId: activeProfile.id,
    });
  };

  const canModify =
    activeProfile.type === 'school_admin' &&
    activeProfile.schoolId == studyPeriod.schoolId;

  const { startDate, endDate } = studyPeriod;

  return (
    <div className="mb-4 mt-3 rounded-lg bg-white p-4 text-center shadow-md">
      <h2 className="mb-4 text-xl font-bold">
        {t('period')} N{index}
      </h2>

      <div className="flex flex-col items-center gap-4">
        <div className="flex flex-col items-center gap-2">
          <span className="font-semibold">{t('labels.startDate')}</span>
          <span className="font-bold">
            <DateOnly date={startDate} />
          </span>
        </div>
        <div className="flex flex-col items-center gap-2">
          <span className="font-semibold">{t('labels.endDate')}</span>
          <span className="font-bold">
            <DateOnly date={endDate} />
          </span>
        </div>
      </div>

      {canModify && (
        <div className="mt-4 flex flex-col justify-center gap-4 sm:flex-row">
          <ProveModal
            open={isDeleteModalOpen}
            text={t('confirmDeleteMsg')}
            onClose={handleCloseDeleteModal}
          />

          <Button
            onClick={handleOpenDeleteModal}
            size="md"
            gradientMonochrome="failure"
            fullSized
          >
            <span className="text-white">{t('deleteBtn')}</span>
          </Button>
          <Button
            onClick={() =>
              replace(
                `/${activeLocale}/u/study-periods/update/${studyPeriod.id}/?${buildUpdateQuery()}`,
              )
            }
            size="md"
            gradientMonochrome="lime"
            fullSized
          >
            <span className="text-gray-700">{t('updateBtn')}</span>
          </Button>
        </div>
      )}
    </div>
  );
};

export { StudyPeriodsItem };
