'use client';

import { CustomImage } from '@/components/custom-image';
import { DateTime } from '@/components/date-time';
import { ProveModal } from '@/components/modal';
import {
  changeCrucial,
  removeGroupNotice,
} from '@/features/user/api/groupNoticesApi';
import { userMutations, userQueries } from '@/features/user/constants';
import type { GroupNoticeResponse } from '@/features/user/types/groupNoticesTypes';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { mediaQueries } from '@/shared/constants';
import { buildQueryString } from '@/shared/helpers';
import {
  useIsMutating,
  useMutation,
  useQueryClient,
} from '@tanstack/react-query';
import { Button } from 'flowbite-react';
import parse from 'html-react-parser';
import { useLocale, useTranslations } from 'next-intl';
import Link from 'next/link';
import { FC, useState } from 'react';
import toast from 'react-hot-toast';
import { useMediaQuery } from 'react-responsive';

type GroupNoticeProps = {
  notice: GroupNoticeResponse;
  groupId: string;
  activeProfile: SchoolProfileResponse;
};

const GroupNoticeItem: FC<GroupNoticeProps> = ({
  notice,
  activeProfile,
  groupId,
}) => {
  const t = useTranslations('GroupNotice');
  const s = useTranslations('SchoolProfile');
  const v = useTranslations('Validation');

  const activeLocale = useLocale();

  const queryClient = useQueryClient();

  const isPhone = useMediaQuery({ query: mediaQueries.phone });
  const isMutating = useIsMutating();

  const [deleteOpenModal, setDeleteOpenModal] = useState(false);
  const [isCrucial, setIsCrucial] = useState(notice.isCrucial);

  const { mutate: deleteGroupNotice } = useMutation({
    mutationFn: removeGroupNotice,
    mutationKey: [userMutations.deleteGroupNotice, notice.id],
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
        queryKey: [userQueries.getGroupNotices],
        refetchType: 'all',
      });

      queryClient.invalidateQueries({
        queryKey: [userQueries.getOneGroup],
        refetchType: 'all',
      });
    },
  });

  const { mutate: mutateChangeCrucialGroupNotice } = useMutation({
    mutationFn: changeCrucial,
    mutationKey: [userMutations.changeCrucial, notice.id],
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
        setIsCrucial((prevCrucial) => !prevCrucial);

        toast.success(t('labels.crucialSuccess'), { duration: 2500 });
      }
    },
    onSettled: async () => {
      queryClient.invalidateQueries({
        queryKey: [userQueries.getGroupNotices],
        refetchType: 'all',
      });

      queryClient.invalidateQueries({
        queryKey: [userQueries.getOneGroup],
        refetchType: 'all',
      });
    },
  });

  const handleOpenDeleteModal = () => setDeleteOpenModal(true);
  const handleCloseDeleteModal = (confirmed: boolean) => {
    setDeleteOpenModal(false);

    if (!confirmed) return;

    deleteGroupNotice({
      id: notice.id,
      schoolProfileId: activeProfile.id,
    });
  };

  const { title, text, author, createdAt, lastUpdatedAt } = notice;

  const canModify =
    activeProfile.type !== 'student' ||
    (activeProfile.type == 'student' && activeProfile.id === notice.authorId);

  const noticeClass = isCrucial
    ? 'bg-gradient-to-r from-red-50 to-red-100 p-4 rounded-lg shadow-md mb-4'
    : 'bg-gradient-to-r from-gray-50 to-gray-100 p-4 rounded-lg shadow-md mb-4';

  const handleToggleCrucial = () => {
    const request = {
      id: notice.id,
      isCrucial: !isCrucial,
    };

    mutateChangeCrucialGroupNotice({
      data: request,
      schoolProfileId: activeProfile.id,
    });
  };

  if (isMutating) return null;

  const buildUpdateQuery = () => {
    return buildQueryString({
      groupId,
      id: notice.id,
      isCrucial: notice.isCrucial,
      title: notice.title,
      text: notice.text
    });
  };

  return (
    <div className="mb-4 mt-4 flex w-[100%] justify-center">
      <div className="mt-2 flex w-[100%] justify-center sm:w-[80%]">
        <div className="w-7/8 md:w-5/6 lg:w-2/3">
          <div className={noticeClass}>
            <div
              className={`${isPhone ? 'flex-col items-start' : 'flex items-start justify-between'} mb-2 px-4`}
            >
              <div className="flex items-center">
                {author?.img && (
                  <div className="mr-2 rounded-full">
                    <CustomImage
                      src={author.img}
                      alt={`${author.name}'s profile`}
                      height={50}
                      width={50}
                    />
                  </div>
                )}
                <div>
                  <div className="text-sm font-semibold">{author?.name}</div>
                  <div className="text-xs font-medium text-gray-500">
                    {s(`type.${author?.type}`)}
                  </div>
                </div>
              </div>

              <div className={`${isPhone ? 'mt-2' : ''}`}>
                <div
                  className={`text-xs ${isCrucial ? 'text-red-500' : 'text-gray-500'} mb-1 font-semibold`}
                >
                  {isCrucial ? t('crucial') : t('regular')}
                </div>

                <div className="mb-1 text-sm font-semibold text-gray-500">
                  <DateTime date={createdAt} />
                </div>
                {lastUpdatedAt && (
                  <div className="text-xs font-medium text-gray-500">
                    <span className="mr-1">{t('updated')}</span>
                    <span className="text-xs text-gray-200">
                      <DateTime date={lastUpdatedAt} />
                    </span>
                  </div>
                )}
              </div>
            </div>
            <h3 className="mb-2 text-center text-xl font-bold">{title}</h3>
            {text && (
              <div className="text-center text-gray-700">{parse(text)}</div>
            )}

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
                      href={`/${activeLocale}/u/group-notices/update/${notice.id}/?${buildUpdateQuery()}`}
                    >
                      {t('updateBtn')}
                    </Link>
                  </Button>
                </div>
                <div
                  className={`flex pl-2 pr-2 pt-2 ${isPhone ? 'order-3 w-full' : 'w-1/3'} justify-center`}
                >
                  <Button
                    gradientMonochrome={isCrucial ? 'success' : 'purple'}
                    fullSized
                    onClick={handleToggleCrucial}
                  >
                    <span className="text-white">
                      {isCrucial ? t('regularBtn') : t('crucialBtn')}
                    </span>
                  </Button>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export { GroupNoticeItem };

