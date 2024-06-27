'use client';

import { Loader } from '@/components/loader';
import { copyCourse } from '@/features/user/api/coursesApi';
import { userMutations } from '@/features/user/constants';
import { UpdateOrCopyCourseRequest } from '@/features/user/types/courseTypes';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import toast from 'react-hot-toast';
import * as Yup from 'yup';
import { StudyPeriodsDropdown } from '../../study-period/getAll/StudyPeriodDropdown';
import styles from './CopyCourseForm.module.scss';

type CopyCourseFormProps = {
  id: string;
  course: UpdateOrCopyCourseRequest;
};

const CopyCourseForm: FC<CopyCourseFormProps> = ({ id, course }) => {
  const t = useTranslations('Course');
  const v = useTranslations('Validation');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const validationSchema = Yup.object().shape({
    studyPeriodId: Yup.string().required(v('required')),
    name: Yup.string().max(250, v('max')).required(v('required')),
    description: Yup.string().optional().max(1024, v('max')),
  });

  const isMutating = useIsMutating();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const { mutate: mutateCopyCourse } = useMutation({
    mutationFn: copyCourse,
    mutationKey: [userMutations.updateCourse],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
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
        toast.success(t('copySuccess'), { duration: 2500 });

        const url = `/${activeLocale}/u/courses/${response.id}`;
        replace(url);
      }
    },
  });

  if (isMutating || isUserLoading) {
    return (
      <>
        <h2 className="md:text p-3 text-center text-lg font-semibold text-gray-950 lg:text-xl">
          {t('copyTitle')}

          <div>
            <span
              onClick={() => {
                const url = `/${activeLocale}/u/courses/${course.id}`;

                replace(url);
              }}
              className="cursor-pointer text-sm font-semibold text-purple-700 hover:text-red-700"
            >
              {t('labels.toCourse')}
            </span>
          </div>
        </h2>

        <Loader />
      </>
    );
  }

  const handleSubmit = (values) => {
    const request: UpdateOrCopyCourseRequest = {
      id: values.id,
      studyPeriodId: values.studyPeriodId,
      name: values.name,
      description: values.name,
    };

    mutateCopyCourse(request);
  };

  return (
    <Formik
      initialValues={course}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      {({ setFieldValue }) => (
        <div className={styles.container}>
          <h2 className="md:text p-3 text-center text-lg font-semibold text-gray-950 lg:text-xl">
            {t('copyTitle')}

            <div>
              <span
                onClick={() => {
                  const url = `/${activeLocale}/u/courses/${course.id}`;

                  replace(url);
                }}
                className="cursor-pointer text-sm font-semibold text-purple-700 hover:text-red-700"
              >
                {t('labels.toCourse')}
              </span>
            </div>
          </h2>

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
                  defaultPeriodId={course.studyPeriodId}
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
              <button type="submit" className={styles.button}>
                {t('labels.copySubmit')}
              </button>
            </div>
          </Form>
        </div>
      )}
    </Formik>
  );
};

export { CopyCourseForm };
