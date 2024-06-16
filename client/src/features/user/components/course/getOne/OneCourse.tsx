'use client';

import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { ProveModal } from '@/components/modal';
import { useProfiles } from '@/features/user';
import {
  CurrentTab,
  userMutations,
  userQueries,
} from '@/features/user/constants';
import { CourseResponse } from '@/features/user/types/courseTypes';
import { mediaQueries } from '@/shared/constants';
import { buildQueryString } from '@/shared/helpers';
import { useAuthRedirectByRole, useSetCurrentTab } from '@/shared/hooks';
import { ApiResponse } from '@/shared/types';
import {
  useIsMutating,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { FC, useState } from 'react';
import toast from 'react-hot-toast';
import { useMediaQuery } from 'react-responsive';
import {
  getOneCourse,
  removeCourse,
  removeCourseGroup,
  removeCourseTeacher,
} from '../../../api/coursesApi';
import { CourseGroup } from './CourseGroup';
import { CourseLessons } from './CourseLessons';
import { CourseTeacher } from './CourseTeacher';

interface OneCourseProps {
  id: string;
}

const OneCourse: FC<OneCourseProps> = ({ id }) => {
  const t = useTranslations('Course');
  const v = useTranslations('Validation');
  const l = useTranslations('Lesson');

  const isMutating = useIsMutating();

  const { replace } = useRouter();
  const activeLocale = useLocale();

  const queryClient = useQueryClient();

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');
  const { activeProfile, isLoading: profilesLoading } = useProfiles();
  const isPhone = useMediaQuery({ query: mediaQueries.phone });

  const [deleteOpenModal, setDeleteOpenModal] = useState(false);

  const { data, isLoading } = useQuery<ApiResponse<CourseResponse>>({
    queryKey: [userQueries.getOneCourse, id],
    queryFn: () => getOneCourse(id),
    enabled: !!id,
    retry: userQueries.options.retry,
  });

  const { mutate: deleteCourse } = useMutation({
    mutationFn: removeCourse,
    mutationKey: [userMutations.deleteCourse, id],
    onSuccess: (response) => {
      if (response && response.error) {
        if (
          response.error.detail.includes('course_profile') ||
          response.error.detail.includes('course_id')
        ) {
          toast.error(v('invalid_course_profile'));
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
        const url =
          activeProfile.type == 'teacher'
            ? `/${activeLocale}/u/courses?teacherId=${activeProfile.id}`
            : `/${activeLocale}/u/courses`;

        replace(url);

        toast.success(t('labels.deleteSuccess'), { duration: 2500 });
      }
    },
    onSettled: async () => {
      queryClient.invalidateQueries({
        queryKey: [userQueries.getCourses],
        refetchType: 'all',
      });
    },
  });

  const { mutate: deleteCourseGroup } = useMutation({
    mutationFn: removeCourseGroup,
    mutationKey: [userMutations.deleteCourseGroup, id],
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
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('deleteGroupSuccess'), { duration: 2500 });
      }
    },
    onSettled: async () => {
      queryClient.invalidateQueries({
        queryKey: [userQueries.getOneCourse, id],
        refetchType: 'all',
      });
    },
  });

  const { mutate: deleteCourseTeacher } = useMutation({
    mutationFn: removeCourseTeacher,
    mutationKey: [userMutations.deleteCourseTeacher, id],
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
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('deleteTeacherSuccess'), { duration: 2500 });
      }
    },
    onSettled: async () => {
      queryClient.invalidateQueries({
        queryKey: [userQueries.getOneCourse, id],
        refetchType: 'all',
      });
    },
  });

  useSetCurrentTab(CurrentTab.Course);

  if (isLoading || isMutating || isUserLoading || profilesLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('oneTitle')}
          <span
            onClick={() => replace(`/${activeLocale}/u/courses`)}
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
            onClick={() => replace(`/${activeLocale}/u/courses`)}
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
      studyPeriodId: data.studyPeriodId,
      name: data.name,
      description: data.description,
    });
  };

  const canModify =
    activeProfile?.type === 'school_admin' || activeProfile?.type === 'teacher';

  const canModifyLessons = activeProfile?.type === 'teacher';

  const handleOpenDeleteModal = () => {
    setDeleteOpenModal(true);
  };

  const handleCloseDeleteModal = (confirmed: boolean) => {
    setDeleteOpenModal(false);

    if (!confirmed) return;

    deleteCourse(id);
  };

  const handleDeleteGroup = (groupId: string) => {
    deleteCourseGroup({
      id: groupId,
      courseId: id,
    });
  };

  const handleDeleteTeacher = (teacherId: string) => {
    deleteCourseTeacher({
      id: teacherId,
      courseId: id,
    });
  };

  const renderGroups = () => {
    if (!data.groups || data.groups.length === 0) {
      return <p>{t('labels.noGroups')}</p>;
    }
    return (
      <div className="flex flex-wrap justify-center gap-2">
        {data.groups.map((group) => (
          <CourseGroup
            key={group.id}
            group={group}
            onDelete={handleDeleteGroup}
          />
        ))}
      </div>
    );
  };

  const renderTeachers = () => {
    if (!data.teachers || data.teachers.length === 0) {
      return <p>{t('labels.noTeachers')}</p>;
    }
    return (
      <div className="flex flex-wrap justify-center gap-2">
        {data.teachers.map((teacher) => (
          <CourseTeacher
            key={teacher.id}
            teacher={teacher}
            onDelete={handleDeleteTeacher}
          />
        ))}
      </div>
    );
  };

  return (
    <div className="mb-2 p-3">
      <h2 className="mb-4 text-center text-xl font-bold">
        {t('oneTitle')}
        <span
          onClick={() => replace(`/${activeLocale}/u/courses`)}
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

      <p className="my-4 text-center">{data.description || ''}</p>

      <div className="my-4 flex flex-col lg:flex-row lg:justify-between">
        <div className="lg:w-1/2">
          <h3 className="text-center font-bold">{t('labels.groups')}</h3>
          {canModify && (
            <div className="mt-2 flex justify-center">
              <Button gradientMonochrome="teal" size="md">
                <Link
                  href={`/${activeLocale}/u/courses/group-create/?schoolId=${activeProfile.schoolId}&courseId=${id}`}
                >
                  <span className="text-white">{t('addGroupBtn')}</span>
                </Link>
              </Button>
            </div>
          )}
          <div className="mt-2 text-center">{renderGroups()}</div>
        </div>

        <div className="lg:w-1/2">
          <h3 className="text-center font-bold">{t('labels.teachers')}</h3>
          {canModify && (
            <div className="mt-2 flex justify-center">
              <Button gradientMonochrome="purple" size="md">
                <Link
                  href={`/${activeLocale}/u/courses/teacher-create/?schoolId=${activeProfile.schoolId}&courseId=${id}`}
                >
                  <span className="text-white">{t('addTeacherBtn')}</span>
                </Link>
              </Button>
            </div>
          )}
          <div className="mt-2 text-center">{renderTeachers()}</div>
        </div>
      </div>

      <div className="my-4">
        <h3 className="text-center text-2xl font-bold">{l('listTitle')}</h3>
        {canModifyLessons && (
          <div className="mt-2 flex justify-center">
            <Button gradientMonochrome="success" size="md">
              <Link href={`/${activeLocale}/u/lessons/create/${id}`}>
                <span className="text-white">{l('createBtn')}</span>
              </Link>
            </Button>
          </div>
        )}

        <CourseLessons courseId={id} />
      </div>

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
                href={`/${activeLocale}/u/courses/update/${id}/?${buildUpdateQuery()}`}
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

export { OneCourse };

