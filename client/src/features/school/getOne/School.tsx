'use client';

import { FC, useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useRouter } from 'next/navigation';
import { useIsMutating, useMutation, useQuery } from '@tanstack/react-query';
import { ApiResponse } from '@/shared/types';
import { getOne, remove } from '../api';
import {
  removeSchoolRoute,
  getOneSchoolRoute,
  joiningRequestClientPath,
  schoolsClientPath,
  updateSchoolClientPath,
} from '../constants';
import { SchoolResponse } from '../types';
import { Loader } from '@/components/loader';
import { buildQueryString, getColorByStatus } from '@/shared/helpers';
import { Error } from '@/components/error';
import { CustomImage } from '@/components/custom-image';
import { mediaQueries } from '@/shared/constants';
import { useMediaQuery } from 'react-responsive';
import { DateTime } from '@/components/date-time';
import { Button } from 'flowbite-react';
import Link from 'next/link';
import { ProveModal } from '@/components/modal';
import { toast } from 'react-hot-toast';

interface SchoolProps {
  id: string;
}

const School: FC<SchoolProps> = ({ id }) => {
  const t = useTranslations('School');

  const { replace } = useRouter();
  const activeLocale = useLocale();
  const isMutating = useIsMutating();

  const { isUserLoading, user } = useAuthRedirectByRole(activeLocale, 'user');

  const { data, isLoading } = useQuery<ApiResponse<SchoolResponse>>({
    queryKey: [getOneSchoolRoute, id],
    queryFn: () => getOne(id),
    enabled: !!id,
  });

  const { mutate: deleteMutate } = useMutation({
    mutationFn: remove,
    mutationKey: [removeSchoolRoute, id],
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          404: t('labels.oneNotFound'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal')
        );
      } else {
        toast.success(t('labels.deleteSuccess'), { duration: 2500 });

        replace(`/${activeLocale}/${schoolsClientPath}`);
      }
    }
  });

  const [deleteOpenModal, setDeleteOpenModal] = useState(false);

  const isPhone = useMediaQuery({ query: mediaQueries.phone });

  if (isLoading || isUserLoading || isMutating) {
    return (
      <>
        <h2 className="mb-4 mt-2 text-center text-xl font-bold">
          {t('oneTitle')}

          {user?.role === 'admin' &&
            <span
              onClick={() =>
                replace(`/${activeLocale}/${schoolsClientPath}`)
              }
              className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
            >
            {t('labels.toMain')}
          </span>
          }
        </h2>
        <Loader />
      </>
    )
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  }

  if (data && data.error) {
    return (
      <div className="flex flex-col justify-center">
        <h2 className="mb-4 mt-2 text-center text-xl font-bold">
          {t('oneTitle')}

          {user?.role === 'admin' &&
            <span
              onClick={() =>
                replace(`/${activeLocale}/${schoolsClientPath}`)
              }
              className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
            >
            {t('labels.toMain')}
          </span>
          }
        </h2>

        <Error error={data.error} />
      </div>
    );
  }

  const handleOpenDeleteModal = () => {
    setDeleteOpenModal(true);
  };
  const handleCloseDeleteModal = (confirmed: boolean) => {
    setDeleteOpenModal(false);

    if (!confirmed)
      return;

    deleteMutate(data.id)
  };

  const occupiedColor = getColorByStatus(data.areOccupied ? 'danger' : 'ok');

  const buildUpdateQuery = () => {
    return buildQueryString({
      id: data.id,
      registerCode: data.registerCode,
      name: data.name,
      shortName: data.shortName,
      gradingSystem: data.gradingSystem,
      email: data.email,
      phone: data.phone,
      type: data.type,
      postalCode: data.postalCode,
      ownershipType: data.ownershipType,
      studentsQuantity: data.studentsQuantity,
      region: data.region,
      territorialCommunity: data.territorialCommunity,
      address: data.address,
      areOccupied: data.areOccupied,
      siteUrl: data.siteUrl,
      img: data.img,
    });
  }

  return (
    <div className="p-3 w-[90%] mb-4">
      <h2 className="mb-4 mt-2 text-center text-xl font-bold">
        {t('oneTitle')}

        {user?.role === 'admin' &&
          <span
            onClick={() =>
              replace(`/${activeLocale}/${schoolsClientPath}`)
            }
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('labels.toMain')}
          </span>
        }
      </h2>

      <h6 className="py-2 text-center font-bold">
        <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
          {t('labels.name')}
        </p>
        <span className="text-purple-950 lg:text-2xl">{data.name}</span>
      </h6>

      <div className="flex justify-center items-center">
        <CustomImage
          src={data.img || `/school/school.jpg`}
          alt='School image'
          width={isPhone ? 200 : 500}
          height={isPhone ? 150 : 300}
        />
      </div>

      <h6 className="pt-4 pb-2 text-center font-bold">
        <p className="color-gray-500 mr-1 text-sm font-normal lg:text-lg">
          {t('labels.detailInfo')}
        </p>
      </h6>

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

      {data.lastUpdatedAt &&
        <div className="flex text-xs lg:text-lg">
          <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
            <span className="text-center">{t('labels.updatedAt')}</span>
          </div>
          <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>
            <DateTime date={data.lastUpdatedAt} />
          </span>
          </div>
        </div>
      }

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
          <span className="text-center">{t('labels.email')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.email || '-'}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.phone')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>{data.phone || '-'}</span>
        </div>
      </div>

      <div className="flex text-xs lg:text-lg">
        <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 font-semibold">
          <span className="text-center">{t('labels.siteUrl')}</span>
        </div>
        <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 font-medium">
          <span>
            {data.siteUrl
              ? <Link
                href={data.siteUrl}
                className="text-blue-500 hover:text-blue-700 underline"
              >
                {data.siteUrl}
              </Link>
             : '-'
            }
          </span>
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

      <ProveModal
        open={deleteOpenModal}
        text={t('labels.proveDelete')}
        onClose={handleCloseDeleteModal}
      />

      {user?.role === 'admin' ? (
        <div className={`flex ${isPhone ? 'flex-col' : 'flex-row'}`}>
          <div className={`pt-2 pr-2 pl-2 flex ${isPhone ? 'w-full order-3' : 'w-1/3'} justify-center`}>
            <Button onClick={handleOpenDeleteModal} gradientMonochrome="failure" fullSized>
              <span
                className="text-white"
              >
                {t('labels.delete')}
              </span>
            </Button>
          </div>

          <div className={`pt-2 pr-2 pl-2 flex ${isPhone ? 'w-full order-2' : 'w-1/3'} justify-center`}>
            <Button gradientMonochrome="purple" fullSized>
              <Link
                href={`/${activeLocale}/${joiningRequestClientPath}/${data.joiningRequestId}`}
                className="text-white"
              >
                {t('labels.toRequest')}
              </Link>
            </Button>
          </div>

          <div className={`pt-2 pr-2 pl-2 flex ${isPhone ? 'w-full order-1' : 'w-1/3'} justify-center`}>
            <Button
              gradientMonochrome="lime"
              fullSized
            >
              <Link
                href={`/${activeLocale}/${updateSchoolClientPath}/${data.id}?${buildUpdateQuery()}`}
              >
                {t('labels.update')}
              </Link>
            </Button>
          </div>
        </div>
      ) : (
        <div className="flex w-full justify-center">
          <Button
            gradientMonochrome="lime"
            fullSized
          >
            <Link
              href={`/${activeLocale}/${updateSchoolClientPath}/${data.id}?${buildUpdateQuery()}`}
            >
              {t('labels.update')}
            </Link>
          </Button>
        </div>
      )}
    </div>
  );
};

export { School };
