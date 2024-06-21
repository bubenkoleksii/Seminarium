'use client';

import { Loader } from '@/components/loader';
import { useProfiles } from '@/features/user';
import { createCourse } from '@/features/user/api/coursesApi';
import { userMutations } from '@/features/user/constants';
import type { CreateCourseRequest } from '@/features/user/types/courseTypes';
import { replaceEmptyStringsWithNull } from '@/shared/helpers';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import toast from 'react-hot-toast';
import * as Yup from 'yup';
import { StudyPeriodsDropdown } from '../../study-period/getAll/StudyPeriodDropdown';
import styles from './CreateCourseForm.module.scss';

const CreateCourseForm: FC = () => {
  const activeLocale = useLocale();
  const { replace } = useRouter();

  const v = useTranslations('Validation');
  const t = useTranslations('Course');

  const isMutating = useIsMutating();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const { mutate, reset: resetMutation } = useMutation({
    mutationFn: createCourse,
    mutationKey: [userMutations.createCourse],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
      if (response && response.error) {
        if (response.error.detail.includes('school_profile')) {
          toast.error(v('invalid_school_profile'));
          return;
        }

        const errorMessages = {
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('createSuccess'), {
          duration: 2500,
        });

        const url = `/${activeLocale}/u/courses/?studyPeriodId=${response.studyPeriodId}`;
        replace(url);
      }
    },
  });

  if (isMutating || isUserLoading || profilesLoading) {
    return (
      <>
        <h2 className="md:text p-3 text-center text-lg font-semibold text-gray-950 lg:text-xl">
          {t('createTitle')}
        </h2>

        <Loader />
      </>
    );
  }

  const validationSchema = Yup.object().shape({
    studyPeriodId: Yup.string().required(v('required')),
    name: Yup.string().max(250, v('max')).required(v('required')),
    description: Yup.string().optional().max(1024, v('max')),
  });

  const initialValues = {
    studyPeriodId: '',
    name: '',
    description: '',
  };

  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);

    const request: CreateCourseRequest = {
      studyPeriodId: values.studyPeriodId,
      name: values.name,
      description: values.description,
    };

    mutate(request);
  };

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      {({ setFieldValue }) => (
        <div className={styles.container}>
          <h2 className="md:text p-2 text-center text-lg font-semibold text-gray-950 lg:text-xl">
            {t('createTitle')}
          </h2>

          <div>
            <span
              onClick={() => {
                const url = `/${activeLocale}/u/courses`;

                replace(url);
              }}
              className="cursor-pointer text-sm font-semibold text-purple-700 hover:text-red-700"
            >
              {t('labels.toList')}
            </span>
          </div>

          <div className="mb-5 flex flex-col items-center justify-center gap-2">
            <div>
              <p className="text-center text-lg font-semibold text-purple-950">
                {activeProfile.schoolName}
              </p>
            </div>
          </div>

          <Form className={styles.form}>
            <div>
              <label htmlFor="studyPeriodId" className={styles.label}>
                {t('labels.studyPeriod')}
                <span className="text-md ml-1 text-red-500">*</span>
              </label>
              <div className="rounded-md border border-gray-300">
                <StudyPeriodsDropdown
                  onSelect={(id) => {
                    setFieldValue('studyPeriodId', id);
                  }}
                />
              </div>
              <ErrorMessage
                name="studyPeriodId"
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
              <label htmlFor="description" className={styles.label}>
                {t('labels.description')}
              </label>
              <Field
                placeholder={t('placeholders.description')}
                type="text"
                as="textarea"
                rows="5"
                id="description"
                name="description"
                className={styles.input}
              />
              <ErrorMessage
                name="description"
                component="div"
                className={styles.error}
              />
            </div>

            <div>
              <button
                onClick={() => resetMutation()}
                type="submit"
                className={styles.button}
              >
                {t('labels.createSubmit')}
              </button>
            </div>
          </Form>
        </div>
      )}
    </Formik>
  );
};

export { CreateCourseForm };
