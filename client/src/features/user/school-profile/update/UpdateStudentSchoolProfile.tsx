'use client';

import styles from './UpdateSchoolProfile.module.scss';
import { FC, useState } from 'react';
import * as Yup from 'yup';
import type { UpdateSchoolProfileRequest } from '@/features/user/types/schoolProfileTypes';
import { useLocale, useTranslations } from 'next-intl';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { toast } from 'react-hot-toast';
import { phoneRegExp } from '@/shared/regexp';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { Loader } from '@/components/loader';
import {
  useIsMutating,
  useMutation,
  useQueryClient,
} from '@tanstack/react-query';
import {
  update,
  updateImage,
  removeImage,
} from '@/features/user/api/schoolProfilesApi';
import {
  studentHealthGroups,
  userMutations,
  userQueries,
} from '@/features/user/constants';
import { useRouter } from 'next/navigation';
import {
  getDefaultProfileImgByType,
  replaceEmptyStringsWithNull,
} from '@/shared/helpers';
import { useProfiles, useSchoolProfilesStore } from '@/features/user';
import { mediaQueries } from '@/shared/constants';
import { useMediaQuery } from 'react-responsive';
import { CustomImage } from '@/components/custom-image';
import { UploadFile } from '@/components/file-upload';
import { Button } from 'flowbite-react';

type UpdateStudentSchoolProfileProps = {
  id: string;
  schoolProfile: UpdateSchoolProfileRequest;
};

const UpdateStudentSchoolProfile: FC<UpdateStudentSchoolProfileProps> = ({
  id,
  schoolProfile,
}) => {
  const activeLocale = useLocale();
  const { replace } = useRouter();
  const v = useTranslations('Validation');
  const t = useTranslations('SchoolProfile');
  const clearSchoolProfiles = useSchoolProfilesStore((store) => store.clear);
  const changeImg = useSchoolProfilesStore((store) => store.changeImg);

  const title = t(`update.${schoolProfile.type}`);

  const { activeProfile, isLoading: profilesLoading } = useProfiles();
  const queryClient = useQueryClient();

  const isMutating = useIsMutating();
  const { isUserLoading, user } = useAuthRedirectByRole(
    activeLocale,
    'userOnly',
  );

  const isPhone = useMediaQuery({ query: mediaQueries.phone });

  schoolProfile.studentIsIndividually =
    schoolProfile.studentIsIndividually === 'true' ||
    schoolProfile.studentIsIndividually === true;
  schoolProfile.studentIsClassLeader =
    schoolProfile.studentIsClassLeader === 'true' ||
    schoolProfile.studentIsClassLeader === true;

  const [img, setImg] = useState<string | undefined>(schoolProfile.img);

  const { mutate: updateMutate } = useMutation({
    mutationFn: update,
    mutationKey: [userMutations.updateSchoolProfile, id],
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
          409: t('labels.updateAlreadyExists'),
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'), {
          duration: 6000,
        });
      } else {
        clearSchoolProfiles();

        toast.success(t('labels.updateSuccess'), {
          duration: 2000,
        });

        queryClient.invalidateQueries({
          queryKey: [userQueries.getSchoolProfile, id],
        });
      }

      const url = `/${activeLocale}/u/school-profile/${id}`;
      replace(url);
    },
  });

  const { mutate: imageMutate } = useMutation({
    mutationFn: updateImage,
    mutationKey: [userMutations.updateSchoolProfileImage, id],
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
          queryKey: [userQueries.getSchoolProfile, id],
        });

        changeImg(id, response.url);
        setImg(response.url);
      }
    },
  });

  const { mutate: imageDeleteMutate } = useMutation({
    mutationFn: removeImage,
    mutationKey: [userMutations.deleteSchoolProfileImage, id],
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
          400: v('labels.validation'),
          401: v('labels.unauthorized'),
          403: v('labels.forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('labels'), {
          duration: 6000,
        });
      } else {
        toast.success(t('deleteImageSuccess'), {
          duration: 2000,
        });

        queryClient.invalidateQueries({
          queryKey: [userQueries.getSchoolProfile, id],
        });

        changeImg(id, '');
        setImg('');
      }
    },
  });

  const validationSchema = Yup.object().shape({
    name: Yup.string().required(v('required')).max(250, v('max')),
    phone: Yup.string().max(50, v('max')).matches(phoneRegExp, v('phone')),
    email: Yup.string().email(v('email')).max(250, v('max')),
    details: Yup.string().max(1024, v('max')),
    parentAddress: Yup.string().max(250, v('max')),
    studentDateOfBirth: Yup.date(),
    studentAptitudes: Yup.string().max(1024, v('max')),
    studentHealthGroup: Yup.string()
      .required(v('required'))
      .max(1024, v('max')),
  });

  if (isMutating || isUserLoading || profilesLoading) {
    return (
      <>
        <h2 className="md:text mb-4 pt-6 text-center text-2xl font-semibold text-gray-950">
          {title}
        </h2>
        <Loader />
      </>
    );
  }

  if (user && user.role == 'admin') {
    return (
      <div className="p-3">
        <h2 className="mb-4 pt-6 text-center text-2xl font-semibold text-gray-950">
          {title}
        </h2>

        <p className="font-medium text-red-600">
          {t(`admin.${schoolProfile.type}`)}
        </p>
      </div>
    );
  }

  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);

    const request: UpdateSchoolProfileRequest = {
      id: id,
      name: values.name,
      phone: values.phone,
      email: values.email,
      details: values.details,
      studentDateOfBirth: values.studentDateOfBirth,
      studentAptitudes: values.studentAptitudes,
      studentIsClassLeader: values.studentIsClassLeader,
      studentIsIndividually: values.studentIsIndividually,
      studentHealthGroup: values.studentHealthGroup,
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
      initialValues={schoolProfile}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      <div className={styles.container}>
        <h2 className="mb-4 p-3 text-center text-2xl font-semibold text-gray-950">
          {title}

          <p
            onClick={() => replace(`/${activeLocale}/u/school-profile/${id}`)}
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('backTo')}
          </p>
        </h2>

        <CustomImage
          src={img || getDefaultProfileImgByType(schoolProfile.type)}
          alt={schoolProfile.name}
          width={isPhone ? 120 : 200}
          height={isPhone ? 120 : 200}
        />

        <div className="flex flex-col items-center justify-center">
          <UploadFile
            isImage={true}
            label={t('updateImage')}
            onSubmit={handleImageSubmit}
          />

          <Button
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
              <span>{t('labels.name')}</span>
            </label>
            <Field
              type="text"
              id="name"
              name="name"
              autoComplete="name"
              className={styles.input}
            />
            <ErrorMessage
              name="name"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="phone" className={styles.label}>
              <span>{t('labels.phone')}</span>
            </label>
            <Field
              type="text"
              id="phone"
              name="phone"
              autoComplete="phone"
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
              <span>{t('labels.email')}</span>
            </label>
            <Field
              type="email"
              id="email"
              name="email"
              autoComplete="email"
              className={styles.input}
            />
            <ErrorMessage
              name="email"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="details" className={styles.label}>
              <span>{t('labels.details')}</span>
            </label>
            <Field
              as="textarea"
              placeholder={t('placeholders.details')}
              type="text"
              id="details"
              rows="4"
              name="details"
              autoComplete="details"
              className={styles.input}
            />
            <ErrorMessage
              name="details"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="" className={styles.label}>
              <span>{t('labels.studentDateOfBirth')}</span>
            </label>
            <Field
              type="date"
              id="studentDateOfBirth"
              name="studentDateOfBirth"
              autoComplete="studentDateOfBirth"
              className={styles.input}
            />
            <ErrorMessage
              name="studentDateOfBirth"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="details" className={styles.label}>
              <span>{t('labels.studentAptitudes')}</span>
            </label>
            <Field
              as="textarea"
              placeholder={t('placeholders.studentAptitudes')}
              type="text"
              id="studentAptitudes"
              rows="4"
              name="studentAptitudes"
              autoComplete="studentAptitudes"
              className={styles.input}
            />
            <ErrorMessage
              name="studentAptitudes"
              component="div"
              className={styles.error}
            />
          </div>
          <div className={styles.checkbox}>
            <Field
              type="checkbox"
              id="studentIsClassLeader"
              name="studentIsClassLeader"
              className={styles.input}
            />
            <label
              htmlFor="studentIsClassLeader"
              className={`${styles.label} ml-2 cursor-pointer`}
            >
              {t('labels.studentIsClassLeader')}
            </label>
          </div>
          <div className={styles.checkbox}>
            <Field
              type="checkbox"
              id="studentIsIndividually"
              name="studentIsIndividually"
              className={styles.input}
            />
            <label
              htmlFor="studentIsIndividually"
              className={`${styles.label} ml-2 cursor-pointer`}
            >
              {t('labels.studentIsIndividually')}
            </label>
          </div>
          <div>
            <label htmlFor="studentHealthGroup" className={styles.label}>
              {t('labels.studentHealthGroup')}
            </label>

            <Field
              as="select"
              id="studentHealthGroup"
              className={styles.select}
              name="studentHealthGroup"
            >
              <option value=""></option>
              {Object.keys(studentHealthGroups).map((value, index) => (
                <option key={index} value={value}>
                  {t(`item.healthGroup.${value}`)}
                </option>
              ))}
            </Field>
            <ErrorMessage
              name="studentHealthGroup"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <button type="submit" className={styles.button}>
              {t('labels.updateSubmit')}
            </button>
          </div>
        </Form>
      </div>
    </Formik>
  );
};

export { UpdateStudentSchoolProfile };
