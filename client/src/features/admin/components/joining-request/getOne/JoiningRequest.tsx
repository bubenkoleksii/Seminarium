'use client';

import { FC, useState } from 'react';
import type { ApiResponse } from '@/shared/types';
import type {
  JoiningRequestResponse,
  RejectRequest,
} from '@/features/admin/types/joiningRequestTypes';
import { useLocale, useTranslations } from 'next-intl';
import {
  useIsMutating,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { getOne, reject } from '@/features/admin/api/joiningRequestApi';
import { Loader } from '@/components/loader';
import { Error } from '@/components/error';
import { useAuthRedirectByRole, useSetCurrentTab } from '@/shared/hooks';
import {
  AdminClientPaths,
  adminMutations,
  adminQueries,
  CurrentTab,
  inRegisterSchoolClientPath,
} from '@/features/admin/constants';
import { buildQueryString, getColorByStatus } from '@/shared/helpers';
import { DateTime } from '@/components/date-time';
import { Button } from 'flowbite-react';
import { useRouter } from 'next/navigation';
import { toast } from 'react-hot-toast';
import { ProveModal, InputModal } from '@/components/modal';
import { school as schoolRoutes } from '@/features/admin/routes';
import Link from 'next/link';

interface JoiningRequestProps {
  id: string;
}

const JoiningRequest: FC<JoiningRequestProps> = ({ id }) => {
  const t = useTranslations('JoiningRequest');

  const queryClient = useQueryClient();
  const isMutating = useIsMutating();

  const { replace } = useRouter();
  const activeLocale = useLocale();

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'admin');

  const [textOpenModal, setTextOpenModal] = useState(false);
  const handleOpenTextModal = () => {
    setTextOpenModal(true);
  };
  const handleCloseTextModal = ({
    proved,
    text,
  }: {
    proved: boolean;
    text: string | null;
  }) => {
    setTextOpenModal(false);

    if (proved) {
      const request: {
        id: string;
        data: RejectRequest;
      } = {
        id: id,
        data: {
          message: text,
        },
      };

      disproveMutate(request);

      resetDisproveMutation();
    }
  };

  const [disproveOpenModal, setDisproveOpenModal] = useState(false);
  const handleOpenDisproveModal = () => {
    setDisproveOpenModal(true);
  };
  const handleCloseDisproveModal = (confirmed: boolean) => {
    setDisproveOpenModal(false);

    if (confirmed) {
      handleOpenTextModal();
    }
  };

  const { data, isLoading } = useQuery<ApiResponse<JoiningRequestResponse>>({
    queryKey: [adminQueries.getOneJoiningRequest, id],
    queryFn: () => getOne(id),
    enabled: !!id,
    retry: adminQueries.options.retry,
  });

  const { mutate: disproveMutate, reset: resetDisproveMutation } = useMutation({
    mutationFn: reject,
    mutationKey: [adminMutations.rejectJoiningRequest],
    retry: adminMutations.options.retry,
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          400: t('labels.validation'),
          404: t('labels.oneNotFound'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
        );
      } else {
        toast.success(t('labels.rejectSuccess'), { duration: 4000 });

        queryClient.invalidateQueries({
          queryKey: [adminQueries.getOneJoiningRequest, id],
        });
      }
    },
  });

  useSetCurrentTab(CurrentTab.JoiningRequest);

  const navigateToCreateSchool = () => {
    const query = buildQueryString({
      registerCode: data.registerCode,
      name: data.name,
      shortName: data.shortName,
      gradingSystem: data.gradingSystem,
      type: data.type,
      postalCode: data.postalCode,
      ownershipType: data.ownershipType,
      studentsQuantity: data.studentsQuantity,
      region: data.region,
      territorialCommunity: data.territorialCommunity,
      address: data.address,
      areOccupied: data.areOccupied,
    });

    const link = `/${activeLocale}/${AdminClientPaths.CreateSchool}/${data.id}?${query}`;
    replace(link);
  };

  if (data && data.error) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('oneTitle')}
          <span
            onClick={() =>
              replace(`/${activeLocale}/${AdminClientPaths.JoiningRequests}/`)
            }
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('labels.toMain')}
          </span>
        </h2>
        <Error error={data.error} />
      </>
    );
  }

  if (isLoading || isMutating || isUserLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('oneTitle')}
          <span
            onClick={() =>
              replace(`/${activeLocale}/${AdminClientPaths.JoiningRequests}/`)
            }
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('labels.toMain')}
          </span>
        </h2>
        <Loader />
      </>
    );
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  }

  const statusColor = getColorByStatus(data.status);
  const occupiedColor = getColorByStatus(data.areOccupied ? 'danger' : 'ok');

  return (
    <div className="mb-4 p-3">
      <h2 className="mb-4 text-center text-xl font-bold">
        {t('oneTitle')}
        <span
          onClick={() =>
            replace(`/${activeLocale}/${AdminClientPaths.JoiningRequests}/`)
          }
          className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
        >
          {t('labels.toMain')}
        </span>
      </h2>

      <ProveModal
        open={disproveOpenModal}
        text={t('labels.rejectMessage')}
        onClose={handleCloseDisproveModal}
      />

      <InputModal
        open={textOpenModal}
        label={t('labels.disproveMessage')}
        onClose={handleCloseTextModal}
      />

      <h6 className="py-2 text-center font-bold">
        <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
          {t('labels.name')}
        </p>
        <span className="text-purple-950 lg:text-2xl">{data.name}</span>
      </h6>

      <h6 className="py-2 text-center font-bold">
        <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
          {t('labels.detailInfo')}
        </p>
      </h6>

      <div className="mb-2 flex w-[100%] justify-center">
        <Button gradientMonochrome="purple" size="md">
          <span className="text-white">
            <Link
              href={`/${activeLocale}/${inRegisterSchoolClientPath}/${data.registerCode}`}
            >
              {t('labels.register.labelBtn')}
            </Link>
          </span>
        </Button>
      </div>

      <div className="mt-2 flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.status.label')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span className={`${statusColor} font-bold`}>
            {t(`labels.status.${data.status}`)}
          </span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.id')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.id}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.createdAt')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>
            <DateTime date={data.createdAt} />
          </span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.registerCode')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.registerCode}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.requesterFullName')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.requesterFullName}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.requesterEmail')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.requesterEmail}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.requesterPhone')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.requesterPhone}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.shortName')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.shortName || '-'}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.gradingSystem')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.gradingSystem}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.type')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{t(`types.${data.type}`)}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.ownershipType')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{t(`ownershipTypes.${data.ownershipType}`)}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.postalCode')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.postalCode}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.studentsQuantity')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.studentsQuantity}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.areOccupied')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span className={occupiedColor}>
            {t(data.areOccupied ? 'labels.yes' : 'labels.no')}
          </span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.region')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>
            {data.region === 'none' ? '-' : t(`regions.${data.region}`)}
          </span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">
            {t('labels.territorialCommunity')}
          </span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.territorialCommunity || ''}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.address')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.address || ''}</span>
        </div>
      </div>

      <div className="flex justify-center pt-4 text-xs lg:text-lg">
        <div className="flex w-full justify-center">
          {data.status === 'created' ? (
            <>
              <div className="flex w-1/2 pr-2">
                <Button
                  onClick={handleOpenDisproveModal}
                  gradientMonochrome="failure"
                  fullSized
                >
                  <span className="text-white">{t('labels.rejectBtn')}</span>
                </Button>
              </div>

              <div className="flex w-1/2 pl-2">
                <Button
                  onClick={navigateToCreateSchool}
                  gradientMonochrome="success"
                  fullSized
                >
                  <span className="text-white">{t('labels.approveBtn')}</span>
                </Button>
              </div>
            </>
          ) : (
            data.status !== 'approved' && (
              <div className="flex w-full justify-center">
                <Button
                  onClick={navigateToCreateSchool}
                  gradientMonochrome="success"
                  fullSized
                >
                  <span className="text-white">{t('labels.approveBtn')}</span>
                </Button>
              </div>
            )
          )}
          {data.schoolId && (
            <div className="flex w-full justify-center">
              <Button gradientMonochrome="purple" fullSized>
                <Link
                  href={`/${activeLocale}/${schoolRoutes.getOne(data.schoolId)}`}
                  className="text-white"
                >
                  {t('labels.toSchool')}
                </Link>
              </Button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export { JoiningRequest };
