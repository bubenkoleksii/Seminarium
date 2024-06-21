'use client';

import { DateTime } from '@/components/date-time';
import { removeTheoryLessonItem } from '@/features/user/api/theoryLessonItemsApi';
import { TheoryLessonItemResponse } from '@/features/user/types/theoryLessonItemTypes';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { Button } from 'flowbite-react';
import parse from 'html-react-parser';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import toast from 'react-hot-toast';

type TheoryLessonItemProps = {
  lesson: TheoryLessonItemResponse;
  canModify: boolean;
  courseId: string;
};

const TheoryLessonItem: FC<TheoryLessonItemProps> = ({
  lesson,
  canModify,
  courseId,
}) => {
  const t = useTranslations('TheoryItem');
  const v = useTranslations('Validation');

  const {
    id,
    createdAt,
    lastUpdatedAt,
    title,
    text,
    isGraded,
    isArchived,
    author,
    attachments,
  } = lesson;

  const queryClient = useQueryClient();

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const lessonClass = isGraded
    ? 'bg-gradient-to-r from-green-50 to-green-100 p-4 rounded-lg shadow-md mb-4'
    : 'bg-gradient-to-r from-gray-50 to-gray-100 p-4 rounded-lg shadow-md mb-4';

  const { mutate: deleteTheoryItem } = useMutation({
    mutationFn: removeTheoryLessonItem,
    mutationKey: ['deleteTheoryItem'],
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          404: t('labels.oneNotFound'),
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('deleteSuccess'), { duration: 2500 });

        replace(
          `/${activeLocale}/u/theory-item/getAll?courseId=${courseId}&lessonId=${id}`,
        );
      }
    },
    onSettled: async () => {
      queryClient.invalidateQueries({
        queryKey: ['theoryItems', lesson.id],
        refetchType: 'all',
      });
    },
  });

  const handleDelete = (id: string) => {
    deleteTheoryItem(id);
  };

  return (
    <div className="mb-4 mt-4 flex w-[100%] justify-center">
      <div className="mt-2 flex w-[100%] justify-center sm:w-[80%]">
        <div className="w-7/8 md:w-5/6 lg:w-2/3">
          <div className={lessonClass}>
            <h3 className="mb-2 text-center text-xl font-bold">{title}</h3>
            {text && (
              <div className="text-center text-gray-700">{parse(text)}</div>
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
            {isGraded && (
              <p className="font-semibold text-red-600">{t('graded')}</p>
            )}

            {canModify && isArchived && (
              <p className="font-semibold text-red-600">{t('archived')}</p>
            )}

            {author && (
              <div className="mt-2 text-sm">
                <span className="font-semibold">{t('author')}</span>{' '}
                {author.name}
              </div>
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
                      {`${t('attachments')}${index + 1}`}
                    </a>
                  ))
                  .reduce((prev, curr) => [prev, ', ', curr])}
              </div>
            )}
            {canModify && (
              <div>
                <Button
                  className="mt-2 flex w-[100%] justify-center"
                  onClick={() => handleDelete(id)}
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

export default TheoryLessonItem;
