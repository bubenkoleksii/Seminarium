import { FC, useState } from 'react';
import {
  CreateSchoolProfileRequest,
  SchoolProfileResponse,
} from '@/features/user/types/schoolProfileTypes';
import { useLocale, useTranslations } from 'next-intl';
import { Table } from 'flowbite-react';
import { DateOnly } from '@/components/date-time';
import { Button } from 'flowbite-react';
import { useRouter } from 'next/navigation';
import toast from 'react-hot-toast';
import { InputTextModal } from '@/components/modal/InputTextModal';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { addChild } from '../../api/schoolProfilesApi';
import { userMutations } from '../../constants';
import { useSchoolProfilesStore } from '../../store/schoolProfilesStore';

type SchoolProfileDetailsInfoProps = {
  profile: SchoolProfileResponse;
};

const SchoolProfileDetailsInfo: FC<SchoolProfileDetailsInfoProps> = ({
  profile,
}) => {
  const t = useTranslations('SchoolProfile');
  const v = useTranslations('Validation');

  const { replace } = useRouter();
  const activeLocale = useLocale();
  const clearSchoolProfiles = useSchoolProfilesStore((store) => store.clear);

  const isMutating = useIsMutating();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const { mutate: addNewChild } = useMutation({
    mutationFn: addChild,
    mutationKey: [userMutations.createSchoolProfile],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
      if (response && response.error) {
        if (response.error.detail.includes('max_profiles_count')) {
          toast.error(t('max_profiles_count'));

          replace(`/${activeLocale}/u`);
          return;
        }

        const errorMessages = {
          400: t('badRequest'),
          409: t('alreadyExists'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        clearSchoolProfiles();

        toast.success(t('createChildSuccess'), { duration: 1500 });
      }

      replace(`/${activeLocale}/u/children/${profile.id}`);
    },
  });

  const [invitationCodeModalOpen, setInvitationCodeModalOpen] = useState(false);
  const handleInvitationCodeModalOpen = () => {
    setInvitationCodeModalOpen(true);
  };
  const handleInvitationCodeModalClose = (confirmed: boolean, text: string) => {
    setInvitationCodeModalOpen(false);

    if (!confirmed) {
      return;
    }

    if (!text.includes('/u/school-profile/create/parent')) {
      toast.error(t('invalidInvitationLink'));

      return;
    } else {
      const request: CreateSchoolProfileRequest = {
        invitationCode: decodeURIComponent(text.split('parent/')[1]),
        name: profile.name,
      };

      addNewChild({
        data: request,
        schoolProfileId: profile.id,
      });
    }
  };

  if (isMutating || isUserLoading) return null;

  const renderRow = (label: string, value: any) => (
    <Table.Row key={label}>
      <Table.Cell className="w-1/2 bg-purple-100 px-4 py-2 text-center font-semibold">
        {t(label)}
      </Table.Cell>
      <Table.Cell className="w-1/2 px-4 py-2 text-center font-medium">
        {value ? (
          label.includes('Date') || label.includes('At') ? (
            <DateOnly date={value} />
          ) : (
            value
          )
        ) : (
          '-'
        )}
      </Table.Cell>
    </Table.Row>
  );

  return (
    <>
      <div className="mt-3 flex justify-center px-4">
        <Table className="sm:max-w-3/4 md:max-w-2/3 lg:max-w-1/2 w-full max-w-full">
          <Table.Body>
            {renderRow('item.name', profile.name)}
            {profile.type !== 'parent' &&
              renderRow('item.school', profile.schoolName)}
            {renderRow('item.createdAt', profile.createdAt)}
            {renderRow('item.lastUpdatedAt', profile.lastUpdatedAt)}
            {renderRow('item.type', t(`type.${profile.type}`))}
            {renderRow('item.phone', profile.phone)}
            {renderRow('item.email', profile.email)}
            {renderRow('item.details', profile.details)}

            {profile.type === 'teacher' && (
              <>
                {renderRow('item.teacherSubjects', profile.teacherSubjects)}
                {renderRow('item.teacherExperience', profile.teacherExperience)}
                {renderRow('item.teacherEducation', profile.teacherEducation)}
                {renderRow(
                  'item.teacherQualification',
                  profile.teacherQualification,
                )}
                {renderRow(
                  'item.teacherLessonsPerCycle',
                  profile.teacherLessonsPerCycle,
                )}
              </>
            )}

            {profile.type === 'student' && (
              <>
                {renderRow(
                  'item.studentHealthGroup',
                  profile.studentHealthGroup
                    ? t(`item.healthGroup.${profile.studentHealthGroup}`)
                    : '-',
                )}
                {renderRow(
                  'item.studentDateOfBirth',
                  profile.studentDateOfBirth,
                )}
                {renderRow('item.studentAptitudes', profile.studentAptitudes)}
                {renderRow(
                  'item.studentIsClassLeader',
                  profile.studentIsClassLeader ? t('yes') : t('no'),
                )}
                {renderRow(
                  'item.studentIsIndividually',
                  profile.studentIsIndividually ? t('yes') : t('no'),
                )}
              </>
            )}

            {profile.type === 'parent' && (
              <>{renderRow('item.parentAddress', profile.parentAddress)}</>
            )}
          </Table.Body>
        </Table>
      </div>

      {profile.type === 'student' && (
        <>
          <h2 className="mt-6 text-center text-xl font-bold">
            {t('parentsLabel')}
          </h2>

          {profile.parents?.length > 0 ? (
            <div className="flex flex-wrap justify-center">
              {profile.parents?.map((parent, idx) => (
                <div
                  key={idx}
                  className="relative m-4 w-full min-w-[200px] max-w-[200px] rounded-lg p-4 shadow-xl"
                >
                  <p className="mt-1 text-center text-lg font-bold">
                    {parent.name}
                  </p>

                  <div className="mt-2 flex w-full flex-wrap justify-center gap-4 md:flex-nowrap">
                    <Button
                      onClick={() =>
                        replace(
                          `/${activeLocale}/u/school-profile/${parent.id}`,
                        )
                      }
                      gradientMonochrome="success"
                      size="xs"
                    >
                      <span className="text-white">{t('detailsBtn')}</span>
                    </Button>
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-center">{t('noParents')}</p>
          )}
        </>
      )}

      {profile.type === 'parent' && (
        <>
          <h2 className="mt-6 text-center text-xl font-bold">
            {t('childrenLabel')}
          </h2>

          <InputTextModal
            open={invitationCodeModalOpen}
            text={t('invitationLabelModal')}
            required={true}
            isTextarea={true}
            onClose={handleInvitationCodeModalClose}
          />

          <div className="mt-2 flex items-center justify-center">
            <Button
              onClick={handleInvitationCodeModalOpen}
              gradientMonochrome="purple"
            >
              <span className="text-white">{t('addChild')}</span>
            </Button>
          </div>

          {profile.children?.length > 0 ? (
            <div className="flex flex-wrap justify-center">
              {profile.children?.map((child, idx) => (
                <div
                  key={idx}
                  className="relative m-4 min-w-min max-w-xs rounded-lg p-4 shadow-xl md:max-w-xs lg:max-w-xs xl:max-w-xs"
                >
                  <p className="mt-1 text-center text-lg font-bold">
                    {child.name}
                  </p>

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
              ))}
            </div>
          ) : (
            <p className="text-center">{t('noChildren')}</p>
          )}
        </>
      )}
    </>
  );
};

export default SchoolProfileDetailsInfo;
