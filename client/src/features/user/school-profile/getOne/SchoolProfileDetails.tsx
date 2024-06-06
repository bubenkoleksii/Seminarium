'use client';

import { FC, useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useProfiles, useSchoolProfilesStore } from '@/features/user';
import { mediaQueries } from '@/shared/constants';
import { useMediaQuery } from 'react-responsive';
import { useIsMutating, useQuery } from '@tanstack/react-query';
import {
  createParentInvitation,
  getOne,
  remove,
} from '@/features/user/api/schoolProfilesApi';
import { userMutations, userQueries } from '@/features/user/constants';
import { Loader } from '@/components/loader';
import { Error } from '@/components/error';
import { ApiResponse } from '@/shared/types';
import { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { CustomImage } from '@/components/custom-image';
import SchoolProfileDetailsInfo from '@/features/user/school-profile/getOne/SchoolProfileDetailsInfo';
import { Button } from 'flowbite-react';
import { useMutation } from '@tanstack/react-query';
import { toast } from 'react-hot-toast';
import { CopyTextModal, ProveModal } from '@/components/modal';
import { buildQueryString, getDefaultProfileImgByType } from '@/shared/helpers';
import Link from 'next/link';

type SchoolProfileDetailsProps = {
  id: string;
};

const SchoolProfileDetails: FC<SchoolProfileDetailsProps> = ({ id }) => {
  const t = useTranslations('SchoolProfile');
  const clearSchoolProfiles = useSchoolProfilesStore((store) => store.clear);

  const { replace } = useRouter();
  const activeLocale = useLocale();

  const isMutating = useIsMutating();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'user');
  const { activeProfile, profiles, isLoading: profilesLoading } = useProfiles();
  const isPhone = useMediaQuery({ query: mediaQueries.phone });

  const [copyParentInvitationOpenModal, setCopyParentInvitationOpenModal] =
    useState(false);
  const [invitationParentCode, setInvitationParentCode] =
    useState<string>(null);

  const [deleteOpenModal, setDeleteOpenModal] = useState(false);

  const { data, isLoading } = useQuery<ApiResponse<SchoolProfileResponse>>({
    queryKey: [userQueries.getSchoolProfile, id],
    queryFn: () => getOne(id),
    enabled: !!id,
    retry: userQueries.options.retry,
  });

  const { mutate: deleteProfile } = useMutation({
    mutationFn: remove,
    mutationKey: [userMutations.deleteProfile, id],
    onSuccess: (response) => {
      if (response && response.error) {
        if (
          response.error.detail.includes('school_profile') ||
          response.error.detail.includes('school_id')
        ) {
          toast.error(t('labels.invalid_school_profile'));

          return;
        }

        const errorMessages = {
          404: t('labels.oneNotFound'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
        );
      } else {
        clearSchoolProfiles();

        toast.success(t('labels.deleteSuccess'), { duration: 2500 });

        replace(`/${activeLocale}/u`);
      }
    },
  });

  const { mutate: generateParentInvitation } = useMutation({
    mutationFn: createParentInvitation,
    mutationKey: [userMutations.createClassTeacherInvitation, id],
    onSuccess: (response) => {
      if (response && response.error) {
        if (
          response.error.detail.includes('school_profile') ||
          response.error.detail.includes('school_id')
        ) {
          toast.error(t('labels.invalid_school_profile'));

          return;
        }

        const errorMessages = {
          404: t('labels.oneNotFound'),
          400: t('labels.invitationValidation'),
          401: t('labels.unauthorized'),
          403: t('labels.forbidden'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
        );
      } else {
        setInvitationParentCode(response.replace(`/uk/`, `/${activeLocale}/`));
        setCopyParentInvitationOpenModal(true);
      }
    },
  });

  if (isLoading || isMutating || isUserLoading || profilesLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('oneTitle')}</h2>

        <Loader />
      </>
    );
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  }

  if (data && data.error) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{t('oneTitle')}</h2>

        <Error error={data.error} />
      </>
    );
  }

  const buildUpdateQuery = () => {
    return buildQueryString({
      id: data.id,
      type: data.type,
      name: data.name,
      phone: data.phone,
      email: data.email,
      details: data.details,
      teacherSubjects: data.teacherSubjects,
      teacherExperience: data.teacherExperience,
      teacherEducation: data.teacherEducation,
      teacherQualification: data.teacherQualification,
      teacherLessonsPerCycle: data.teacherLessonsPerCycle,
      studentDateOfBirth: data.studentDateOfBirth,
      studentAptitudes: data.studentAptitudes,
      studentIsClassLeader: data.studentIsClassLeader?.toString(),
      studentIsIndividually: data.studentIsIndividually?.toString(),
      studentHealthGroup: data.studentHealthGroup,
      parentAddress: data.parentAddress,
      img: data.img,
    });
  };

  const handleOpenDeleteModal = () => {
    setDeleteOpenModal(true);
  };
  const handleCloseDeleteModal = (confirmed: boolean) => {
    setDeleteOpenModal(false);

    if (!confirmed) return;

    deleteProfile(data.id);
  };

  const canUpdate = profiles && profiles.some((profile) => profile.id === id);
  const canDelete =
    !(
      !activeProfile ||
      activeProfile?.type !== 'school_admin' ||
      activeProfile?.type !== 'class_teacher' ||
      (activeProfile?.type === 'school_admin' &&
        data.schoolId !== activeProfile?.schoolId) ||
      (activeProfile?.type === 'class_teacher' &&
        data.groupId !== activeProfile?.groupId)
    ) || canUpdate;
  const canGenerateParentCode =
    activeProfile?.type === 'school_admin' ||
    activeProfile?.type === 'class_teacher' ||
    canUpdate;

  return (
    <div className="mb-2 p-3">
      <h2 className="mb-4 text-center text-xl font-bold">{t('oneTitle')}</h2>

      <h6 className="text-center font-bold">
        <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
          {t('item.name')}
        </p>
        <span className="text-purple-950 lg:text-2xl">{data.name}</span>
      </h6>

      <div className="flex w-[100%] items-center justify-center">
        <CustomImage
          src={data.img || getDefaultProfileImgByType(data.type)}
          alt={data.name}
          width={isPhone ? 120 : 200}
          height={isPhone ? 120 : 200}
        />
      </div>

      {data.type === 'student' && canGenerateParentCode && (
        <>
          <div className="mb-4 mt-4 flex w-[100%] justify-center">
            <div className="w-[350px]">
              <Button
                onClick={() => {
                  setInvitationParentCode(null);

                  generateParentInvitation({
                    id: data.id,
                    schoolProfileId: activeProfile?.id,
                  });
                }}
                gradientDuoTone="pinkToOrange"
                size="md"
              >
                <span className="text-white">{t('parentModalBtn')}</span>
              </Button>
            </div>
          </div>

          {invitationParentCode && (
            <CopyTextModal
              open={copyParentInvitationOpenModal}
              label={t('labelParentModal')}
              text={invitationParentCode}
              onClose={() => setCopyParentInvitationOpenModal(false)}
            />
          )}
        </>
      )}
      <div className="mt-4 flex w-[100%] flex-col justify-center lg:flex-row">
        <SchoolProfileDetailsInfo profile={data} />
      </div>

      <div className="mt-3 flex justify-center gap-4">
        {canDelete && (
          <>
            <ProveModal
              open={deleteOpenModal}
              text={t('deleteMsg')}
              onClose={handleCloseDeleteModal}
            />

            <Button
              onClick={handleOpenDeleteModal}
              size="md"
              gradientMonochrome="failure"
            >
              <span className="text-white">{t('deleteBtn')}</span>
            </Button>
          </>
        )}

        {canUpdate && (
          <Button size="md" gradientMonochrome="lime">
            <Link
              href={`/${activeLocale}/u/school-profile/update/${id}?${buildUpdateQuery()}`}
            >
              {t('updateBtn')}
            </Link>
          </Button>
        )}
      </div>
    </div>
  );
};

export { SchoolProfileDetails };
