'use client';

import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { ProveModal } from '@/components/modal';
import { useProfiles } from '@/features/user';
import { CurrentTab, userQueries } from '@/features/user/constants';
import { CourseResponse } from '@/features/user/types/courseTypes';
import { mediaQueries } from '@/shared/constants';
import { useAuthRedirectByRole, useSetCurrentTab } from '@/shared/hooks';
import { ApiResponse } from '@/shared/types';
import { useIsMutating, useQuery } from '@tanstack/react-query';
import { Button } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { FC, useState } from 'react';
import { useMediaQuery } from 'react-responsive';
import { getOneCourse } from '../../../api/coursesApi';
import { CourseGroup } from './CourseGroup';
import { CourseTeacher } from './CourseTeacher';

interface OneCourseProps {
  id: string;
}

const OneCourse: FC<OneCourseProps> = ({ id }) => {
  const t = useTranslations('Course');
  const v = useTranslations('Validation');

  const isMutating = useIsMutating();

  const { replace } = useRouter();
  const activeLocale = useLocale();

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

  const canModify =
    activeProfile?.type === 'school_admin' || activeProfile?.type === 'teacher';

  const handleOpenDeleteModal = () => {
    setDeleteOpenModal(true);
  };

  const handleCloseDeleteModal = (confirmed: boolean) => {
    setDeleteOpenModal(false);

    if (!confirmed) return;

    // deleteCourse API call
    // remove(data.id)
    //   .then(() => {
    //     toast.success(t('labels.deleteSuccess'), { duration: 2500 });
    //     replace(`/${activeLocale}/u/courses`);
    //   })
    //   .catch((error) => {
    //     toast.error(error.message || v('internal'));
    //   });
  };

  const handleDeleteGroup = (groupId: string) => {
    // handle delete group logic here
  };

  const handleDeleteTeacher = (teacherId: string) => {
    // handle delete teacher logic here
  };

  const renderGroups = () => {
    if (!data.groups || data.groups.length === 0) {
      return <p>{t('labels.noGroups')}</p>;
    }
    return (
      <div className="flex flex-wrap gap-2">
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
      <div className="flex flex-wrap gap-2">
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

      <div className="my-4">
        <h3 className="text-center font-bold">{t('labels.groups')}</h3>
        {canModify && (
          <div className="mt-2 flex justify-center">
            <Button gradientMonochrome="teal" size="md">
              <Link href={`/${activeLocale}/u/courses/group-create/${data.id}`}>
                <span className="text-white">{t('addGroupBtn')}</span>
              </Link>
            </Button>
          </div>
        )}
        <div className="text-center">{renderGroups()}</div>
      </div>

      <div className="my-4">
        <h3 className="text-center font-bold">{t('labels.teachers')}</h3>
        {canModify && (
          <div className="mt-2 flex justify-center">
            <Button gradientMonochrome="purple" size="md">
              <Link href={`/${activeLocale}/u/courses/teacher-create/${data.id}`}>
                <span className="text-white">{t('addTeacherBtn')}</span>
              </Link>
            </Button>
          </div>
        )}
        <div className="text-center">{renderTeachers()}</div>
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
              <Link href={`/${activeLocale}/u/courses/update/${id}`}>
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

