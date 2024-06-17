'use client';

import { DateTime } from '@/components/date-time';
import { Error } from '@/components/error';
import { Loader } from '@/components/loader';
import {
  getOnePracticalLessonItemSubmit,
  removePracticalLessonItemSubmit,
} from '@/features/user/api/practicalLessonItemSubmitApi';
import { useProfiles } from '@/features/user/hooks';
import { PracticalLessonItemSubmitResponse } from '@/features/user/types/practicalLessonItemSubmitTypes';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { Button } from 'flowbite-react';
import parse from 'html-react-parser';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
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
        queryKey: ['practicalLessonItemSubmit', id],
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
  } = data;

  console.log('data', data);

  const statusClass =
    status === 'completed'
      ? 'bg-gradient-to-r from-green-50 to-green-100 p-4 rounded-lg shadow-md mb-4'
      : 'bg-gradient-to-r from-yellow-50 to-yellow-100 p-4 rounded-lg shadow-md mb-4';

  const handleDelete = (id: string) => {
    deletePracticalItemSubmit(id);
  };

  return (
    <div className="mb-4 mt-4 flex w-[100%] justify-center">
      <div className="mt-2 flex w-[100%] justify-center sm:w-[80%]">
        <div className="w-7/8 md:w-5/6 lg:w-2/3">
          <div className={statusClass}>
            <h3 className="mb-2 text-center text-xl font-bold">{t('title')}</h3>
            {text && (
              <div className="text-center text-gray-700">{parse(text)}</div>
            )}
            {attachments && attachments.length > 0 && (
              <div className="mt-2 text-sm">
                <span className="font-semibold">{t('attachments')}</span>{' '}
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
            <div className="mt-2 text-sm text-gray-500">
              <span>{t('createdAt')}</span> <DateTime date={createdAt} />
            </div>
            {lastUpdatedAt && (
              <div className="mt-1 text-sm text-gray-500">
                <span>{t('lastUpdatedAt')}</span>{' '}
                <DateTime date={lastUpdatedAt} />
              </div>
            )}
            {studentName && (
              <div className="mt-2 text-sm">
                <span className="font-semibold">{t('studentName')}</span>{' '}
                {studentName}
              </div>
            )}
            <div className="mt-2 text-sm">
              <span className="font-semibold">{t('status')}</span>{' '}
              {t(`statuses.${data.status}`)}
            </div>

            {data.status === 'submitted' && (
              <div className="flex flex-[100%] justify-center">
                <Button
                  className="mt-2 flex w-[200px] justify-center"
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

