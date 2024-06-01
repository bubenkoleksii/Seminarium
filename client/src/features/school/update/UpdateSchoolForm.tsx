'use client';

import { FC, useState } from 'react';
import type { UpdateSchoolRequest } from '@/features/school/types';
import { useLocale, useTranslations } from 'next-intl';
import { useAuthRedirectByRole } from '@/shared/hooks';
import {
  useIsMutating,
  useMutation,
  useQueryClient,
} from '@tanstack/react-query';
import { useRouter } from 'next/navigation';
import * as Yup from 'yup';
import { Loader } from '@/components/loader';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import styles from '@/features/admin/components/school/create/CreateSchoolForm.module.scss';
import { mediaQueries, school as schoolConstants } from '@/shared/constants';
import { removeImage, update, updateImage } from '@/features/school/api';
import {
  getOneSchoolRoute,
  imageRoute,
  updateSchoolRoute,
} from '@/features/school/constants';
import { toast } from 'react-hot-toast';
import { replaceEmptyStringsWithNull } from '@/shared/helpers';
import { CustomImage } from '@/components/custom-image';
import { useMediaQuery } from 'react-responsive';
import { UploadFile } from '@/components/file-upload';
import { Button } from 'flowbite-react';
import { useProfiles } from '@/features/user';

type UpdateSchoolFormProps = {
  id: string;
  school: UpdateSchoolRequest;
};

const UpdateSchoolForm: FC<UpdateSchoolFormProps> = ({ id, school }) => {
  const t = useTranslations('School');
  const v = useTranslations('Validation');
  const activeLocale = useLocale();
  const { replace } = useRouter();

  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const queryClient = useQueryClient();

  const { isUserLoading, user } = useAuthRedirectByRole(activeLocale, 'user');
  const isMutating = useIsMutating();

  const [img, setImg] = useState<string | undefined>(school.img);

  const validationSchema = Yup.object().shape({
    email: Yup.string().email(v('email')).max(250, v('max')),
    phone: Yup.string().max(250, v('max')),
    site: Yup.string().max(250, v('max')),
    registerCode: Yup.string().required(v('required')).max(50, v('max')),
    name: Yup.string().required(v('required')).max(250, v('max')),
    shortName: Yup.string().max(250, v('max')),
    gradingSystem: Yup.number()
      .required(v('required'))
      .max(10000, v('maxNumber')),
    postalCode: Yup.string().required(v('required')).max(50, v('max')),
    studentsQuantity: Yup.number()
      .required(v('required'))
      .max(1000000, v('maxNumber')),
    territorialCommunity: Yup.string().max(250, v('max')),
    address: Yup.string().max(250, v('max')),
    type: Yup.string().required(v('required')),
    region: Yup.string().required(v('required')),
    ownershipType: Yup.string().required(v('required')),
  });

  const {
    mutate,
    isPending,
    reset: resetMutation,
  } = useMutation({
    mutationFn: update,
    mutationKey: [updateSchoolRoute],
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          404: t('labels.updateNotFound'),
          409: t('labels.updateAlreadyExists'),
          400: t('labels.validation'),
          401: t('labels.unauthorized'),
          403: t('labels.forbidden'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
          {
            duration: 6000,
          },
        );
      } else {
        toast.success(t('labels.updateSuccess'), {
          duration: 2000,
        });

        queryClient.invalidateQueries({
          queryKey: [getOneSchoolRoute, id],
        });

        const url = user?.role === 'user'
          ? `/${activeLocale}/u/my-school/${id}`
          : `/${activeLocale}/school/${id}`;

        replace(url);
      }
    },
  });

  const { mutate: imageMutate, isPending: imagePending } = useMutation({
    mutationFn: updateImage,
    mutationKey: [imageRoute + 'update', id],
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          404: t('labels.updateNotFound'),
          400: t('labels.validation'),
          401: t('labels.unauthorized'),
          403: t('labels.forbidden'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
          {
            duration: 6000,
          },
        );
      } else {
        toast.success(t('labels.updateImageSuccess'), {
          duration: 2000,
        });

        queryClient.invalidateQueries({
          queryKey: [getOneSchoolRoute, id],
        });

        setImg(response.url);
      }
    },
  });

  const { mutate: imageDeleteMutate, isPending: imageDeletePending } =
    useMutation({
      mutationFn: removeImage,
      mutationKey: [imageRoute + 'delete', id],
      onSuccess: (response) => {
        if (response && response.error) {
          const errorMessages = {
            404: t('labels.updateNotFound'),
            400: t('labels.validation'),
            401: t('labels.unauthorized'),
            403: t('labels.forbidden'),
          };

          toast.error(
            errorMessages[response.error.status] || t('labels.internal'),
            {
              duration: 6000,
            },
          );
        } else {
          toast.success(t('labels.deleteImageSuccess'), {
            duration: 2000,
          });

          queryClient.invalidateQueries({
            queryKey: [getOneSchoolRoute, id],
          });

          setImg('');
        }
      },
    });

  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);

    const request: UpdateSchoolRequest = {
      id: values.id,
      registerCode: values.registerCode,
      name: values.name,
      shortName: values.shortName,
      gradingSystem: values.gradingSystem,
      email: values.email,
      phone: values.phone,
      type: values.type,
      postalCode: values.postalCode,
      ownershipType: values.ownershipType,
      studentsQuantity: values.studentsQuantity,
      region: values.region,
      territorialCommunity: values.territorialCommunity,
      address: values.address,
      areOccupied: values.areOccupied === `true`,
      siteUrl: values.siteUrl,
    };

    mutate({
      data: request,
      schoolProfileId: activeProfile.id,
    });
  };

  const handleImageSubmit = (values: { files: File }) => {
    const formData = new FormData();
    formData.append('Image', values.files);

    imageMutate({
      id: school.id,
      data: formData,
      schoolProfileId: activeProfile.id,
    });
  };

  const isPhone = useMediaQuery({ query: mediaQueries.phone });

  if (
    isPending ||
    imagePending ||
    imageDeletePending ||
    isMutating ||
    isUserLoading ||
    profilesLoading
  ) {
    return (
      <>
        <h2 className="mb-4 mt-2 text-center text-xl font-bold">
          {t('updateTitle')}
          <span
            onClick={() => {
              const url = user?.role === 'user'
                ? `/${activeLocale}/u/my-school/${id}`
                : `/${activeLocale}/school/${id}`;

              replace(url);
            }}
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('back')}
          </span>
        </h2>
        <Loader />
      </>
    );
  }

  return (
    <Formik
      initialValues={school}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      <div className={styles.container}>
        <h2 className="mb-4 mt-2 text-center text-xl font-bold">
          {t('updateTitle')}
          <span
            onClick={() => {
              const url = user?.role === 'user'
                ? `/${activeLocale}/u/my-school/${id}`
                : `/${activeLocale}/school/${id}`;

              replace(url);
            }}
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('back')}
          </span>
        </h2>

        <div className="mb-4 flex flex-col items-center justify-center">
          <CustomImage
            src={img || `/school/school.jpg`}
            alt="School image"
            width={isPhone ? 200 : 500}
            height={isPhone ? 150 : 300}
          />

          <div className="flex flex-col items-center justify-center">
            <UploadFile
              isImage={true}
              label={t('updateImage')}
              onSubmit={handleImageSubmit}
            />

            <Button
              onClick={() => imageDeleteMutate({
                id,
                schoolProfileId: activeProfile.id,
              })}
              gradientMonochrome="failure"
            >
              <span className="text-white">{t('labels.deleteImage')}</span>
            </Button>
          </div>
        </div>

        <Form className={styles.form}>
          <div>
            <label htmlFor="registerCode" className={styles.label}>
              {t('labels.registerCode')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.registerCode')}
              type="text"
              id="registerCode"
              name="registerCode"
              className={styles.input}
            />
            <ErrorMessage
              name="registerCode"
              component="div"
              className={styles.error}
            />
          </div>
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
            <label htmlFor="shortName" className={styles.label}>
              {t('labels.shortName')}
            </label>
            <Field
              placeholder={t('placeholders.shortName')}
              type="text"
              id="shortName"
              name="shortName"
              className={styles.input}
            />
            <ErrorMessage
              name="shortName"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="gradingSystem" className={styles.label}>
              {t('labels.gradingSystem')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.gradingSystem')}
              type="text"
              id="gradingSystem"
              name="gradingSystem"
              className={styles.input}
            />
            <ErrorMessage
              name="gradingSystem"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="type" className={styles.label}>
              {t('labels.type')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field as="select" id="type" className={styles.select} name="type">
              <option value=""></option>
              {Object.values(schoolConstants.type).map((value, index) => (
                <option key={index} value={value}>
                  {t(`types.${value}`)}
                </option>
              ))}
            </Field>
            <ErrorMessage
              name="type"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="postalCode" className={styles.label}>
              {t('labels.postalCode')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.postalCode')}
              type="text"
              id="postalCode"
              name="postalCode"
              className={styles.input}
            />
            <ErrorMessage
              name="postalCode"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="ownershipType" className={styles.label}>
              {t('labels.ownershipType')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              as="select"
              id="ownershipType"
              className={styles.select}
              name="ownershipType"
            >
              <option value=""></option>
              {Object.values(schoolConstants.ownershipType).map(
                (value, index) => (
                  <option key={index} value={value}>
                    {t(`ownershipTypes.${value}`)}
                  </option>
                ),
              )}
            </Field>
            <ErrorMessage
              name="ownershipType"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="studentsQuantity" className={styles.label}>
              {t('labels.studentsQuantity')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.studentsQuantity')}
              type="number"
              id="studentsQuantity"
              name="studentsQuantity"
              className={styles.input}
            />
            <ErrorMessage
              name="studentsQuantity"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="region" className={styles.label}>
              {t('labels.region')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              as="select"
              id="region"
              className={styles.select}
              name="region"
            >
              <option value=""></option>
              {Object.values(schoolConstants.region).map((value, index) => (
                <option key={index} value={value}>
                  {t(`regions.${value}`)}
                </option>
              ))}
            </Field>
            <ErrorMessage
              name="region"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="territorialCommunity" className={styles.label}>
              {t('labels.territorialCommunity')}
            </label>
            <Field
              placeholder={t('placeholders.territorialCommunity')}
              type="text"
              id="territorialCommunity"
              name="territorialCommunity"
              className={styles.input}
            />
            <ErrorMessage
              name="territorialCommunity"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="address" className={styles.label}>
              {t('labels.address')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.address')}
              type="text"
              id="address"
              name="address"
              className={styles.input}
            />
            <ErrorMessage
              name="address"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="phone" className={styles.label}>
              {t('labels.phone')}
            </label>
            <Field
              type="text"
              id="phone"
              name="phone"
              className={styles.input}
            />
            <ErrorMessage
              name="phone"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="email" className={styles.label}>
              {t('labels.email')}
            </label>
            <Field
              type="text"
              id="email"
              name="email"
              className={styles.input}
            />
            <ErrorMessage
              name="email"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="siteUrl" className={styles.label}>
              {t('labels.siteUrl')}
            </label>
            <Field
              type="text"
              id="siteUrl"
              name="siteUrl"
              className={styles.input}
            />
            <ErrorMessage
              name="siteUrl"
              component="div"
              className={styles.error}
            />
          </div>
          <div className={styles.checkbox}>
            <Field
              type="checkbox"
              id="areOccupied"
              name="areOccupied"
              className={styles.input}
            />
            <label
              htmlFor="areOccupied"
              className={`${styles.label} ml-2 cursor-pointer`}
            >
              {t('labels.areOccupied')}
            </label>
          </div>
          <div>
            <button
              onClick={() => resetMutation()}
              type="submit"
              className={styles.button}
            >
              {t('labels.updateSubmit')}
            </button>
          </div>
        </Form>
      </div>
    </Formik>
  );
};

export { UpdateSchoolForm };
