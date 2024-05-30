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
import { useIsMutating } from '@tanstack/react-query';

interface CreateDefaultSchoolProfileProps {
  type: string;
  invitationCode: string;
}

const CreateDefaultSchoolProfile: FC<CreateDefaultSchoolProfileProps> = ({
  invitationCode,
  type,
}) => {
  const activeLocale = useLocale();
  const v = useTranslations('Validation');
  const t = useTranslations('SchoolProfile');

  const title = t(`create.${type}`);

  const isMutating = useIsMutating();
  const { isUserLoading, user } = useAuthRedirectByRole(activeLocale);

  const validationSchema = Yup.object().shape({
    phone: Yup.string().max(50, v('max')).matches(phoneRegExp, v('phone')),
    email: Yup.string().email(v('email')).max(250, v('max')),
    details: Yup.string().max(1024, v('max')),
  });

  const initialValues: CreateSchoolProfileRequest = {
    invitationCode,
    phone: '',
    email: '',
    details: '',
  };

  if (isMutating || isUserLoading) {
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

  const handleSubmit = (values) => {
    console.log(values);
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
            <label htmlFor="phone" className={styles.label}>
              <span>{t('labels.phone')}</span>
            </label>
            <Field
              placeholder={t('placeholders.phone')}
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
              placeholder={t('placeholders.email')}
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
              name="email"
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
