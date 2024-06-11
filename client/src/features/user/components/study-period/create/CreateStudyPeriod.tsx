'use client';

import { Loader } from '@/components/loader';
import { createStudyPeriod } from '@/features/user/api/studyPeriodApi';
import { userMutations } from '@/features/user/constants';
import { useProfiles } from '@/features/user/hooks';
import { CreateStudyPeriodRequest } from '@/features/user/types/studyPeriodTypes';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import { toast } from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './CreateStudyPeriod.module.scss';
import { getEndDate, getStartDate } from './helpers';

const CreateStudyPeriod: FC = () => {
  const activeLocale = useLocale();
  const { replace } = useRouter();
  const t = useTranslations('StudyPeriod');

  const isMutating = useIsMutating();

  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const { mutate } = useMutation({
    mutationFn: createStudyPeriod,
    mutationKey: [userMutations.createStudyPeriod],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
      if (response.error) {
        const errorMessages = {
          400: t('badRequest'),
          409: t('alreadyExists'),
        };

        toast.error(errorMessages[response.error.status] || t('internalError'));
      } else {
        toast.success(
          t(
            response.incrementGroups
              ? 'createSuccessIncrementGroups'
              : 'createSuccess',
          ),
          { duration: 1500 },
        );

        replace(`/${activeLocale}/u/study-periods`);
      }
    },
  });

  const validationSchema = Yup.object().shape({
    incrementGroups: Yup.boolean().required(t('validation.required')),
    startDate: Yup.date().required(t('validation.required')),
    endDate: Yup.date()
      .required(t('validation.required'))
      .min(Yup.ref('startDate'), t('validation.endDateAfterStartDate')),
  });

  const initialValues = {
    incrementGroups: false,
    startDate: getStartDate(),
    endDate: getEndDate(),
  };

  const handleSubmit = (values) => {
    const request: CreateStudyPeriodRequest = {
      incrementGroups: values.incrementGroups,
      startDate: values.startDate,
      endDate: values.endDate,
    };

    mutate({
      data: request,
      schoolProfileId: activeProfile?.id,
    });
  };

  if (isMutating || profilesLoading) {
    return <Loader />;
  }

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      <div className={styles.container}>
        <h2 className="mb-4 p-3 text-center text-2xl font-semibold text-gray-950">
          {t('create.title')}
          <p
            onClick={() => replace(`/${activeLocale}/u/study-periods`)}
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('backTo')}
          </p>
        </h2>
        <Form className={styles.form}>
          <div>
            <label htmlFor="startDate" className={styles.label}>
              <span>{t('labels.startDate')}</span>
            </label>
            <Field
              type="date"
              id="startDate"
              name="startDate"
              autoComplete="startDate"
              className={styles.input}
            />
            <ErrorMessage
              name="startDate"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="endDate" className={styles.label}>
              <span>{t('labels.endDate')}</span>
            </label>
            <Field
              type="date"
              id="endDate"
              name="endDate"
              autoComplete="endDate"
              className={styles.input}
            />
            <ErrorMessage
              name="endDate"
              component="div"
              className={styles.error}
            />
          </div>
          <div className={styles.checkbox}>
            <Field
              type="checkbox"
              id="incrementGroups"
              name="incrementGroups"
              className={styles.input}
            />
            <label
              htmlFor="incrementGroups"
              className={`${styles.label} ml-2 cursor-pointer`}
            >
              {t('labels.incrementGroups')}
            </label>
            <ErrorMessage
              name="incrementGroups"
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

export { CreateStudyPeriod };
