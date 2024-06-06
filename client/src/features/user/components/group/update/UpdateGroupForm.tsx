'use client';

import styles from './UpdateGroupForm.module.scss';
import { FC, useState } from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import type { UpdateGroupRequest } from '@/features/user/types/groupTypes';
import { useLocale, useTranslations } from 'next-intl';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useProfiles } from '@/features/user';
import { useRouter } from 'next/navigation';
import { userMutations, userQueries } from '@/features/user/constants';
import {
  useMutation,
  useQueryClient,
  useIsMutating,
} from '@tanstack/react-query';
import {
  update,
  updateImage,
  removeImage,
} from '@/features/user/api/groupsApi';
import { toast } from 'react-hot-toast';
import { Loader } from '@/components/loader';
import { replaceEmptyStringsWithNull } from '@/shared/helpers';
import { CustomImage } from '@/components/custom-image';
import { UploadFile } from '@/components/file-upload';
import { mediaQueries } from '@/shared/constants';
import { useMediaQuery } from 'react-responsive';
import { Button } from 'flowbite-react';

type UpdateGroupFormProps = {
  id: string;
  group: UpdateGroupRequest;
};

const UpdateGroupForm: FC<UpdateGroupFormProps> = ({ id, group }) => {
  const activeLocale = useLocale();
  const { replace } = useRouter();
  const v = useTranslations('Validation');
  const t = useTranslations('Group');

  const isMutating = useIsMutating();
  const { isUserLoading, user } = useAuthRedirectByRole(
    activeLocale,
    'userOnly',
  );
  const queryClient = useQueryClient();

  const isPhone = useMediaQuery({ query: mediaQueries.phone });

  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const { mutate: updateMutate } = useMutation({
    mutationFn: update,
    mutationKey: [userMutations.updateGroup, id],
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
          404: t('labels.notFound'),
          409: t('labels.alreadyExists'),
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'), {
          duration: 6000,
        });
      } else {
        toast.success(t('updateSuccess'), {
          duration: 2000,
        });

        queryClient.invalidateQueries({
          queryKey: [userQueries.getOneGroup, id],
        });

        const url = `/${activeLocale}/u/groups/${id}`;
        replace(url);
      }
    },
  });

  const [img, setImg] = useState<string | null>(group.img);

  const { mutate: imageMutate } = useMutation({
    mutationFn: updateImage,
    mutationKey: [userMutations.updateGroupImage, id],
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
          404: t('labels.updateNotFound'),
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'), {
          duration: 6000,
        });
      } else {
        toast.success(t('updateImageSuccess'), {
          duration: 2000,
        });

        queryClient.invalidateQueries({
          queryKey: [userQueries.getOneGroup, id],
        });

        setImg(response.url);
      }
    },
  });

  const { mutate: imageDeleteMutate } = useMutation({
    mutationFn: removeImage,
    mutationKey: [userMutations.deleteGroupImage, id],
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
          404: t('labels.updateNotFound'),
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'), {
          duration: 6000,
        });
      } else {
        toast.success(t('deleteImageSuccess'), {
          duration: 2000,
        });

        queryClient.invalidateQueries({
          queryKey: [userQueries.getOneGroup, id],
        });

        setImg('');
      }
    },
  });

  if (isMutating || isUserLoading || profilesLoading) {
    return (
      <>
        <h2 className="md:text mb-4 pt-6 text-center text-2xl font-semibold text-gray-950">
          {t('update.title')}
        </h2>
        <Loader />
      </>
    );
  }

  if (user && user.role == 'admin') {
    return (
      <div className="p-3">
        <h2 className="mb-4 pt-6 text-center text-2xl font-semibold text-gray-950">
          {t('update.title')}
        </h2>

        <p className="font-medium text-red-600">
          {t(`admin.${activeProfile?.type}`)}
        </p>
      </div>
    );
  }

  const validationSchema = Yup.object().shape({
    name: Yup.string().max(250, v('max')),
    studyPeriodNumber: Yup.number()
      .min(0, v('minNumber'))
      .max(100, v('maxNumber')),
  });

  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);

    const request: UpdateGroupRequest = {
      id: id,
      name: values.name,
      studyPeriodNumber: values.studyPeriodNumber,
    };

    updateMutate({
      data: request,
      schoolProfileId: activeProfile?.id,
    });
  };

  const handleImageSubmit = (values: { files: File }) => {
    const formData = new FormData();
    formData.append('Image', values.files);

    imageMutate({
      id: id,
      data: formData,
      schoolProfileId: activeProfile?.id,
    });
  };

  return (
    <Formik
      initialValues={group}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      <div className={styles.container}>
        <h2 className="md:text p-3 text-center text-lg font-semibold text-gray-950 lg:text-xl">
          {t('update.title')}
        </h2>

        <div className="mb-5 flex flex-col items-center justify-center gap-2">
          <div>
            <span
              onClick={() => {
                const url = `/${activeLocale}/u/groups/${id}`;

                replace(url);
              }}
              className="cursor-pointer text-sm font-semibold text-purple-700 hover:text-red-700"
            >
              {t('labels.backTo')}
            </span>
          </div>
        </div>

        <CustomImage
          src={img || `/group/group.png`}
          alt="Group image"
          width={isPhone ? 150 : 300}
          height={isPhone ? 100 : 200}
        />

        <div className="mt-2 flex flex-col items-center justify-center">
          <UploadFile
            isImage={true}
            label={t('updateImage')}
            onSubmit={handleImageSubmit}
          />

          <Button
            className="mb-4"
            onClick={() =>
              imageDeleteMutate({
                id,
                schoolProfileId: activeProfile?.id,
              })
            }
            gradientMonochrome="failure"
          >
            <span className="text-white">{t('deleteImageBtn')}</span>
          </Button>
        </div>

        <Form className={styles.form}>
          <div>
            <label htmlFor="name" className={styles.label}>
              {t('labels.name')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.name')}
              type="text"
              id="name"
              name="name"
              className={styles.input}
            />
            <ErrorMessage
              name="name"
              component="div"
              className={styles.error}
            />
          </div>

          <div>
            <label htmlFor="studyPeriodNumber" className={styles.label}>
              {t('labels.studyPeriodNumber')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder="8"
              type="number"
              id="studyPeriodNumber"
              name="studyPeriodNumber"
              className={styles.input}
            />
            <ErrorMessage
              name="studyPeriodNumber"
              component="div"
              className={styles.error}
            />
          </div>

          <div>
            <button type="submit" className={styles.button}>
              {t('updateBtn')}
            </button>
          </div>
        </Form>
      </div>
    </Formik>
  );
};

export { UpdateGroupForm };
