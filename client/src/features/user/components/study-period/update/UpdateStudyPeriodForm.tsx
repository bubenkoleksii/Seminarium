'use client';

import { Loader } from '@/components/loader';
import { useProfiles } from '@/features/user';
import { updateStudyPeriod } from '@/features/user/api/studyPeriodApi';
import { userMutations } from '@/features/user/constants';
import { UpdateStudyPeriodRequest } from '@/features/user/types/studyPeriodTypes';
import { replaceEmptyStringsWithNull } from '@/shared/helpers';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import toast from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './UpdateStudyPeriodForm.module.scss';

type UpdateStudyPeriodProps = {
  id: string;
  studyPeriod: UpdateStudyPeriodRequest;
};

const UpdateStudyPeriodForm: FC<UpdateStudyPeriodProps> = ({
  id,
  studyPeriod,
}) => {
  const activeLocale = useLocale();
  const { replace } = useRouter();

  const v = useTranslations('Validation');
  const t = useTranslations('StudyPeriod');

  const isMutating = useIsMutating();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const { mutate } = useMutation({
    mutationFn: updateStudyPeriod,
    mutationKey: [userMutations.updateStudyPeriod],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
      console.log('res', response);

      if (response && response.error) {
        if (response.error.detail.includes('school_profile')) {
          toast.error(v('invalid_school_profile'));

          return;
        }

        const errorMessages = {
          409: t('labels.alreadyExists'),
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('labels.updateSuccess'), { duration: 2500 });

        const url = `/${activeLocale}/u/study-periods/`;
        replace(url);
      }
    },
  });

  const validationSchema = Yup.object().shape({
    startDate: Yup.date().required(t('validation.required')),
    endDate: Yup.date()
      .required(t('validation.required'))
      .min(Yup.ref('startDate'), t('validation.endDateAfterStartDate')),
  });

  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);

    const request: UpdateStudyPeriodRequest = {
      id: studyPeriod.id,
      startDate: values.startDate,
      endDate: values.endDate,
    };

    mutate({
      data: request,
      schoolProfileId: activeProfile?.id,
    });
  };

  if (profilesLoading || isMutating || isUserLoading) {
    return (
      <>
        <h2 className="md:text mb-4 pt-6 text-center text-2xl font-semibold text-gray-950">
          {t('update.title')}
        </h2>

        <Loader />
      </>
    );
  }

  return (
    <Formik
      initialValues={studyPeriod}
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

export { UpdateStudyPeriodForm };
