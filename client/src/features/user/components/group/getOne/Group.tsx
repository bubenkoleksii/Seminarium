'use client';

import { CustomImage } from '@/components/custom-image';
import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { CopyTextModal, ProveModal } from '@/components/modal';
import { useProfiles } from '@/features/user';
import ClassTeacherInfo from '@/features/user/components/group/getOne/ClassTeacherInfo';
import { GroupInfo } from '@/features/user/components/group/getOne/GroupInfo';
import { GroupStudents } from '@/features/user/components/group/getOne/GroupStudents';
import {
  CurrentTab,
  userMutations,
  userQueries,
} from '@/features/user/constants';
import { OneGroupResponse } from '@/features/user/types/groupTypes';
import { mediaQueries } from '@/shared/constants';
import { buildQueryString } from '@/shared/helpers';
import { useAuthRedirectByRole, useSetCurrentTab } from '@/shared/hooks';
import { ApiResponse } from '@/shared/types';
import { useIsMutating, useMutation, useQuery } from '@tanstack/react-query';
import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { FC, useState } from 'react';
import { toast } from 'react-hot-toast';
import { useMediaQuery } from 'react-responsive';
import {
  createClassTeacherInvitation,
  createStudentInvitation,
  getOne,
  remove,
} from '../../../api/groupsApi';

interface GroupProps {
  id: string;
}

const Group: FC<GroupProps> = ({ id }) => {
  const t = useTranslations('Group');
  const v = useTranslations('Validation');

  const isMutating = useIsMutating();

  const { replace } = useRouter();
  const activeLocale = useLocale();

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');
  const { activeProfile, isLoading: profilesLoading } = useProfiles();
  const isPhone = useMediaQuery({ query: mediaQueries.phone });

  const [deleteOpenModal, setDeleteOpenModal] = useState(false);

  const { data, isLoading } = useQuery<ApiResponse<OneGroupResponse>>({
    queryKey: [userQueries.getOneGroup, id],
    queryFn: () => getOne(id),
    enabled: !!id,
    retry: userQueries.options.retry,
  });

  const { mutate: generateClassTeacherInvitation } = useMutation({
    mutationFn: createClassTeacherInvitation,
    mutationKey: [userMutations.createClassTeacherInvitation, id],
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

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
        );
      } else {
        setInvitationClassTeacherCode(
          response.replace(`/uk/`, `/${activeLocale}/`),
        );
        setCopyClassTeacherInvitationOpenModal(true);
      }
    },
  });

  const { mutate: generateStudentInvitation } = useMutation({
    mutationFn: createStudentInvitation,
    mutationKey: [userMutations.createStudentInvitation, id],
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
        setInvitationStudentCode(response.replace(`/uk/`, `/${activeLocale}/`));
        setCopyStudentInvitationOpenModal(true);
      }
    },
  });

  const { mutate: deleteGroup } = useMutation({
    mutationFn: remove,
    mutationKey: [userMutations.deleteGroup, id],
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

        replace(`/${activeLocale}/u/groups`);
      }
    },
  });

  useSetCurrentTab(CurrentTab.Group);

  const [
    copyClassTeacherInvitationOpenModal,
    setCopyClassTeacherInvitationOpenModal,
  ] = useState(false);
  const [invitationClassTeacherCode, setInvitationClassTeacherCode] =
    useState<string>(null);

  const [copyStudentInvitationOpenModal, setCopyStudentInvitationOpenModal] =
    useState(false);
  const [invitationStudentCode, setInvitationStudentCode] =
    useState<string>(null);

  if (isLoading || isMutating || isUserLoading || profilesLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('oneTitle')}
          <span
            onClick={() => replace(`/${activeLocale}/u/groups`)}
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('toMain')}
          </span>
        </h2>

        <Loader />
      </>
    );
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  }

  if (data && data.error) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('oneTitle')}
          <span
            onClick={() => replace(`/${activeLocale}/u/groups`)}
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('toMain')}
          </span>
        </h2>

        <Error error={data.error} />
      </>
    );
  }

  const buildUpdateQuery = () => {
    return buildQueryString({
      id: data.id,
      name: data.name,
      studyPeriodNumber: data.studyPeriodNumber,
      img: data.img,
    });
  };

  const canModify =
    (activeProfile?.type === 'school_admin' &&
      activeProfile?.schoolId === data.schoolId) ||
    (activeProfile?.type === 'class_teacher' &&
      activeProfile?.groupId === data.id);

  const canDeleteClassTeacher =
    activeProfile.id == data.classTeacher?.id ||
    (activeProfile.type === 'school_admin' &&
      activeProfile.school === data.classTeacher?.schoolId);

  const handleOpenDeleteModal = () => {
    setDeleteOpenModal(true);
  };
  const handleCloseDeleteModal = (confirmed: boolean) => {
    setDeleteOpenModal(false);

    if (!confirmed) return;

    deleteGroup({
      id: data.id,
      schoolProfileId: activeProfile?.id,
    });
  };

  return (
    <div className="mb-2 p-3">
      <h2 className="mb-4 text-center text-xl font-bold">
        {t('oneTitle')}
        <span
          onClick={() => replace(`/${activeLocale}/u/groups`)}
          className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
        >
          {t('toMain')}
        </span>
      </h2>

      <h6 className="text-center font-bold">
        <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
          {t('labels.name')}
        </p>
        <span className="text-purple-950 lg:text-2xl">{data.name}</span>
      </h6>

      {invitationStudentCode && (
        <CopyTextModal
          open={copyStudentInvitationOpenModal}
          text={invitationStudentCode}
          label={t('studentInvitationBtn')}
          onClose={() => setCopyStudentInvitationOpenModal(false)}
        />
      )}

      <div className="mb-4 mt-4 flex flex-col items-center justify-center lg:flex-row">
        <div className="flex justify-center lg:w-1/2">
          <CustomImage
            src={data.img || `/group/group.png`}
            alt="Group image"
            width={isPhone ? 150 : 300}
            height={isPhone ? 100 : 200}
          />
        </div>

        <div className="mt-4 flex flex-col items-start justify-center lg:mt-0 lg:w-1/4">
          <p className="color-gray-500 text mr-1 w-[350px] text-center font-semibold lg:text-lg">
            {t('classTeacher')}
          </p>

          {data.classTeacher && canDeleteClassTeacher && (
            <p className="color-gray-500 text mb-4 mt-2 w-[350px] text-xs font-semibold">
              {t('deleteClassTeacherLabel')}
            </p>
          )}

          {data.classTeacher ? (
            <>
              <ClassTeacherInfo classTeacher={data.classTeacher} />
            </>
          ) : (
            <>
              <p className="text-md  w-[350px] text-center text-red-900">
                {t('labels.noClassTeacher')}
              </p>

              {canModify && (
                <div className="mb-4 flex w-[100%] items-center justify-center">
                  <div className="mt-2 flex w-[350px] justify-center">
                    <Button
                      onClick={() => {
                        setInvitationClassTeacherCode(null);

                        generateClassTeacherInvitation({
                          id: data.id,
                          schoolProfileId: activeProfile?.id,
                        });
                      }}
                      gradientMonochrome="teal"
                      size="md"
                    >
                      <span className="text-white">
                        {t('classTeacherInvitationBtn')}
                      </span>
                    </Button>
                  </div>
                </div>
              )}

              {invitationClassTeacherCode && (
                <CopyTextModal
                  open={copyClassTeacherInvitationOpenModal}
                  text={invitationClassTeacherCode}
                  label={t('classTeacherInvitationBtn')}
                  onClose={() => setCopyClassTeacherInvitationOpenModal(false)}
                />
              )}
            </>
          )}
        </div>
      </div>

      <div className="mt-4 flex w-[100%] flex-col justify-center lg:flex-row">
        <GroupInfo group={data} />
      </div>

      {canModify && (
        <div className="mb-4 mt-2 flex w-[350px] justify-center pt-3">
          <div className="flex w-[350px] justify-center">
            <Button
              onClick={() => {
                setInvitationStudentCode(null);

                generateStudentInvitation({
                  id: data.id,
                  schoolProfileId: activeProfile?.id,
                });
              }}
              gradientMonochrome="pink"
              size="md"
            >
              <span className="text-white">{t('studentInvitationBtn')}</span>
            </Button>
          </div>
        </div>
      )}

      <GroupStudents students={data.students} />

      {canModify && (
        <div
          className={`mt-3 flex ${isPhone ? 'flex-col' : 'flex-row justify-center'}`}
        >
          <div
            className={`flex pl-2 pr-2 pt-2 ${isPhone ? 'order-2 w-full' : 'w-1/3'} justify-center`}
          >
            <ProveModal
              open={deleteOpenModal}
              text={t('deleteMsg')}
              onClose={handleCloseDeleteModal}
            />

            <Button
              onClick={handleOpenDeleteModal}
              gradientMonochrome="failure"
              fullSized
            >
              <span className="text-white">{t('deleteBtn')}</span>
            </Button>
          </div>

          <div
            className={`flex pl-2 pr-2 pt-2 ${isPhone ? 'order-1 w-full' : 'w-1/3'} justify-center`}
          >
            <Button gradientMonochrome="lime" fullSized>
              <Link
                href={`/${activeLocale}/u/groups/update/${id}?${buildUpdateQuery()}`}
              >
                {t('updateBtn')}
              </Link>
            </Button>
          </div>
        </div>
      )}
    </div>
  );
};

export { Group };

