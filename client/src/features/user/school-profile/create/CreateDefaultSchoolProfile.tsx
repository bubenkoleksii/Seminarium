'use client';

import styles from './CreateSchoolProfile.module.scss';
import { FC } from 'react';
import * as Yup from 'yup';
import type { CreateSchoolProfileRequest } from '@/features/user/types/schoolProfileTypes';
import { useLocale, useTranslations } from 'next-intl';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { toast } from 'react-hot-toast';
import { phoneRegExp } from '@/shared/regexp';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { Loader } from '@/components/loader';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { create } from '@/features/user/api/schoolProfilesApi';
import { userMutations } from '@/features/user/constants';
import { useRouter } from 'next/navigation';
import { replaceEmptyStringsWithNull } from '@/shared/helpers';
import { useSchoolProfilesStore } from '@/features/user';
import { useSession } from 'next-auth/react';

interface CreateDefaultSchoolProfileProps {
  type: string;
  invitationCode: string;
}

const CreateDefaultSchoolProfile: FC<CreateDefaultSchoolProfileProps> = ({
  invitationCode,
  type,
}) => {
  const activeLocale = useLocale();
  const { replace } = useRouter();
  const { data: userData, status: userStatus } = useSession();
  const v = useTranslations('Validation');
  const t = useTranslations('SchoolProfile');
  const clearSchoolProfiles = useSchoolProfilesStore((store) => store.clear);

  const title = t(`create.${type}`);

  const isMutating = useIsMutating();
  const { isUserLoading, user } = useAuthRedirectByRole(activeLocale);

  const { mutate } = useMutation({
    mutationFn: create,
    mutationKey: [userMutations.createSchoolProfile],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
      if (response && response.error) {
        if (response.error.detail.includes('max_profiles_count')) {
          toast.error(t('max_profiles_count'));

          replace(`/${activeLocale}/u`);
          return;
        }

        const errorMessages = {
          400: t('badRequest'),
          409: t('alreadyExists'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
        );
      } else {
        clearSchoolProfiles();

        toast.success(t('createSuccess'), { duration: 1500 });
      }

      replace(`/${activeLocale}/u`);
    },
  });

  const validationSchema = Yup.object().shape({
    name: Yup.string().required(v('required')).max(250, v('max')),
    phone: Yup.string().max(50, v('max')).matches(phoneRegExp, v('phone')),
    email: Yup.string().email(v('email')).max(250, v('max')),
    details: Yup.string().max(1024, v('max')),
    parentAddress: Yup.string().max(250, v('max')),
  });

  if (isMutating || isUserLoading || userStatus === 'loading') {
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

        <p className="font-medium text-red-600">{t(`admin.${type}`)}</p>
      </div>
    );
  }

  const initialValues: CreateSchoolProfileRequest = {
    invitationCode,
    name: userData?.user.name,
    phone: '',
    email: '',
    details: '',
  };

  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);

    const request: CreateSchoolProfileRequest = {
      invitationCode: invitationCode,
      name: values.name,
      phone: values.phone,
      email: values.email,
      details: values.details,
    };

    mutate(request);
  };

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      <div className={styles.container}>
        <h2 className="mb-4 p-3 text-center text-2xl font-semibold text-gray-950">
          {title}
        </h2>
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
            <button type="submit" className={styles.button}>
              {t('labels.createSubmit')}
            </button>
          </div>
        </Form>
      </div>
    </Formik>
  );
};

export { CreateDefaultSchoolProfile };