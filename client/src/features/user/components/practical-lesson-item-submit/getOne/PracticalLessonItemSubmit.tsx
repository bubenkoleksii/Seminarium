'use client';

import { DateTime } from '@/components/date-time';
import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import { InputTextModal } from '@/components/modal/InputTextModal';
import {
  addResults,
  getOnePracticalLessonItemSubmit,
  removePracticalLessonItemSubmit,
} from '@/features/user/api/practicalLessonItemSubmitApi';
import { userMutations } from '@/features/user/constants';
import { useProfiles } from '@/features/user/hooks';
import {
  AddPracticalItemSubmitResultsRequest,
  PracticalLessonItemSubmitResponse,
} from '@/features/user/types/practicalLessonItemSubmitTypes';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { Button } from 'flowbite-react';
import parse from 'html-react-parser';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC, useState } from 'react';
import toast from 'react-hot-toast';

type PracticalLessonItemSubmitProps = {
  itemId: string;
  studentId: string;
};

const PracticalLessonItemSubmit: FC<PracticalLessonItemSubmitProps> = ({
  itemId,
  studentId,
}) => {
  const t = useTranslations('PracticalItemSubmit');
  const v = useTranslations('Validation');

  const queryClient = useQueryClient();
  const activeLocale = useLocale();
  const { replace } = useRouter();

  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const [teacherRejectModalOpen, setTeacherRejectModalOpen] = useState(false);

  const { mutate: mutateRejectResults } = useMutation({
    mutationFn: addResults,
    mutationKey: ['rejectPracticalLessonItemSubmit', itemId, studentId],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
      if (response && response.error) {
        if (response.error.detail.includes('school_profile')) {
          toast.error(v('invalid_school_profile'));
          return;
        }

        const errorMessages = {
          400: v('validation'),
          401: v('unauthorized'),
          404: v('notFound'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('resultsSuccess'), {
          duration: 2500,
        });
      }
    },
    onSettled: async () => {
      queryClient.invalidateQueries({
        queryKey: ['practicalLessonItemSubmit', itemId, studentId],
        refetchType: 'all',
      });
    },
  });

  const { data, isLoading } = useQuery<PracticalLessonItemSubmitResponse>({
    queryKey: ['practicalLessonItemSubmit', itemId, studentId],
    queryFn: () => getOnePracticalLessonItemSubmit(studentId, itemId),
    enabled: !!itemId,
  });

  const { mutate: deletePracticalItemSubmit } = useMutation({
    mutationFn: removePracticalLessonItemSubmit,
    mutationKey: ['deletePracticalItemSubmit'],
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          404: t('oneNotFound'),
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('deleteSuccess'), { duration: 2500 });

        replace(
          `/${activeLocale}/u/practical-item-submit/getOne/?itemId=${itemId}&studentId=${studentId}`,
        );
      }
    },
    onSettled: async () => {
      queryClient.invalidateQueries({
        queryKey: ['practicalLessonItemSubmit', itemId, studentId],
        refetchType: 'all',
      });
    },
  });

  if (isLoading || profilesLoading) {
    return <Loader />;
  }

  if (data && data.error) {
    if (data.error.status === 404) {
      return (
        <div className="mt-2 flex w-full flex-col items-center justify-center">
          <h2 className="mb-4 text-center text-xl font-bold">
            {t('oneTitle')}
          </h2>
          <p className="w-full text-center">{t('notUploaded')}</p>

          {activeProfile.type === 'student' && (
            <div className="flex flex-[100%] justify-center">
              <Button
                className="mt-2 flex w-[200px] justify-center"
                onClick={() =>
                  replace(
                    `/${activeLocale}/u/practical-item-submit/create?itemId=${itemId}`,
                  )
                }
                gradientMonochrome="pink"
                size="sm"
              >
                <span className="text-white">{t('uploadWork')}</span>
              </Button>
            </div>
          )}
        </div>
      );
    }

    return <Error error={data.error} />;
  }

  const {
    id,
    createdAt,
    lastUpdatedAt,
    studentName,
    text,
    attachments,
    status,
    attempt,
    teacherComment,
    mark,
  } = data;

  const statusClasses = {
    submitted:
      'bg-gradient-to-r from-yellow-50 to-yellow-100 p-4 rounded-lg shadow-md mb-4',
    rejected:
      'bg-gradient-to-r from-red-50 to-red-100 p-4 rounded-lg shadow-md mb-4',
    accepted:
      'bg-gradient-to-r from-green-50 to-green-100 p-4 rounded-lg shadow-md mb-4',
  };

  const textColorClasses = {
    submitted: 'font-semibold text-yellow-800',
    rejected: 'font-semibold text-red-800',
    accepted: 'font-semibold text-green-800',
  };

  const statusClass = statusClasses[status];
  const textColorClass = textColorClasses[status];

  const handleTeacherReject = (confirmed: boolean, text: string) => {
    setTeacherRejectModalOpen(false);

    if (!confirmed) return;

    const request: AddPracticalItemSubmitResultsRequest = {
      id,
      isAccept: false,
      text: text ?? undefined,
    };

    mutateRejectResults(request);
  };

  const handleDelete = (id: string) => {
    deletePracticalItemSubmit(id);
  };

  const isTeacher = activeProfile.type === 'teacher';

  console.log('status', status);

  return (
    <div className="mb-4 mt-4 flex w-full justify-center px-2">
      <div className="mt-2 flex w-full justify-center sm:w-4/5 md:w-3/4 lg:w-2/3">
        <div className="w-full">
          {isTeacher && (
            <div>
              <p
                onClick={() =>
                  replace(
                    `/${activeLocale}/u/practical-item-submit/getTeacherAll?itemId=${itemId}`,
                  )
                }
                className="mb-2 ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
              >
                {t('teacherBackTo')}
              </p>
            </div>
          )}

          <div className={statusClass}>
            <h3 className="mb-2 text-center text-xl font-bold">{t('title')}</h3>
            {text && (
              <div className="text-center text-gray-700">{parse(text)}</div>
            )}
            {attachments && attachments.length > 0 && (
              <div className="mt-2 text-left text-sm">
                <span className="font-semibold">{t('attachments')}:</span>{' '}
                {attachments
                  .map((attachment, index) => (
                    <a
                      key={index}
                      href={attachment}
                      target="_blank"
                      rel="noopener noreferrer"
                      className="attachment-link text-blue-500 underline"
                    >
                      {`${t('attachments')}${index + 1} `}
                    </a>
                  ))
                  .reduce((prev, curr) => [prev, ', ', curr])}
              </div>
            )}
            <div className="mt-2 text-left text-sm text-gray-500">
              <span className="font-semibold">{t('createdAt')}:</span>{' '}
              <DateTime date={createdAt} />
            </div>
            {lastUpdatedAt && (
              <div className="mt-1 text-left text-sm text-gray-500">
                <span className="font-semibold">{t('lastUpdatedAt')}:</span>{' '}
                <DateTime date={lastUpdatedAt} />
              </div>
            )}
            {studentName && (
              <div className="mt-2 text-left text-sm">
                <span className="font-semibold">{t('studentName')}:</span>{' '}
                {studentName}
              </div>
            )}
            <div className="mt-2 text-left text-sm">
              <span className="font-semibold">{t('status')}:</span>{' '}
              <span className={textColorClass}>
                {t(`statuses.${data.status}`)}
              </span>
            </div>
            {status === 'accepted' && (
              <div className="mt-2 text-left text-sm">
                <span className="font-semibold">{t('labels.mark')}:</span>{' '}
                <span>{mark}</span>
              </div>
            )}
            {teacherComment && (
              <div className="mt-2 text-left text-sm">
                <span className="font-semibold">{t('teacherComment')}:</span>{' '}
                <span className={textColorClass}>{teacherComment}</span>
              </div>
            )}

            {activeProfile.type === 'teacher' && (
              <>
                <div className="mb-4 flex justify-center gap-4">
                  <Button
                    className="mt-2 w-[200px] justify-center"
                    onClick={() => setTeacherRejectModalOpen(true)}
                    gradientMonochrome="pink"
                    size="xs"
                  >
                    <span className="text-white">
                      {status === 'rejected' ? t('updateReject') : t('reject')}
                    </span>
                  </Button>
                  <InputTextModal
                    open={teacherRejectModalOpen}
                    text={t('comment')}
                    isTextarea={true}
                    onClose={handleTeacherReject}
                  />

                  <Button
                    className="mt-2 w-[200px] justify-center"
                    gradientMonochrome="success"
                    onClick={() =>
                      replace(
                        `/${activeLocale}/u/practical-item-submit/accept/?submitId=${id}&itemId=${itemId}&studentId=${studentId}`,
                      )
                    }
                    size="xs"
                  >
                    <span className="text-white">
                      {status === 'accepted' ? t('updateAccept') : t('accept')}
                    </span>
                  </Button>
                </div>
              </>
            )}

            {((status === 'submitted' && activeProfile.id === data.studentId) ||
              isTeacher) && (
              <div className="flex justify-center">
                <Button
                  className="mt-2 w-[200px] justify-center"
                  onClick={() => handleDelete(data.id)}
                  gradientMonochrome="failure"
                  size="xs"
                >
                  <span className="text-white">{t('deleteBtn')}</span>
                </Button>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export { PracticalLessonItemSubmit };
