'use client';

import { Loader } from '@/components/loader';
import { SearchInput } from '@/components/search-input';
import { addCourseGroup } from '@/features/user/api/coursesApi';
import { userMutations } from '@/features/user/constants';
import { AddCourseGroupRequest } from '@/features/user/types/courseTypes';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import toast from 'react-hot-toast';

type CreateCourseGroupProps = {
  courseId: string;
  schoolId: string;
};

const CreateCourseGroup: FC<CreateCourseGroupProps> = ({
  courseId,
  schoolId,
}) => {
  const t = useTranslations('Course');
  const v = useTranslations('Validation');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const isMutating = useIsMutating();

  const { mutate: mutateCreateGroup } = useMutation({
    mutationFn: addCourseGroup,
    mutationKey: [userMutations.addCourseGroup],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
      if (response && response.error) {
        if (response.error.detail.includes('school_profile')) {
          toast.error(v('invalid_school_profile'));
          return;
        }

        const errorMessages = {
          400: v('validation'),
          404: t('groupNotFound'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('addGroupSuccess'), {
          duration: 2500,
        });

        const url = `/${activeLocale}/u/courses/${courseId}`;
        replace(url);
      }
    },
  });

  const handleSearch = (text) => {
    const request: AddCourseGroupRequest = {
      name: text,
      courseId,
      schoolId,
    };

    mutateCreateGroup(request);
  };

  if (isMutating) {
    return (
      <>
        <h2 className="md:text p-2 text-center text-lg font-semibold text-gray-950 lg:text-xl">
          {t('createGroupTitle')}
        </h2>

        <Loader />
      </>
    );
  }

  return (
    <div className="p-3">
      <h2 className="md:text p-2 text-center text-lg font-semibold text-gray-950 lg:text-xl">
        {t('createGroupTitle')}

        <div>
          <span
            onClick={() => {
              const url = `/${activeLocale}/u/courses/${courseId}`;

              replace(url);
            }}
            className="cursor-pointer text-sm font-semibold text-purple-700 hover:text-red-700"
          >
            {t('labels.toCourse')}
          </span>
        </div>
      </h2>

      <SearchInput
        maxLength={200}
        placeholder={t('placeholders.group')}
        onSubmit={handleSearch}
      />
    </div>
  );
};

export { CreateCourseGroup };
