'use client';

import { Loader } from '@/components/loader';
import { updateCourse } from '@/features/user/api/coursesApi';
import { StudyPeriodsDropdown } from '@/features/user/components/study-period/getAll/StudyPeriodDropdown';
import { userMutations } from '@/features/user/constants';
import { UpdateCourseRequest } from '@/features/user/types/courseTypes';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import toast from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './UpdateCourseForm.module.scss';

type UpdateCourseFormProps = {
  id: string;
  course: UpdateCourseRequest;
};

const UpdateCourseForm: FC<UpdateCourseFormProps> = ({ id, course }) => {
  const t = useTranslations('Course');
  const v = useTranslations('Validation');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const validationSchema = Yup.object().shape({
    studyPeriodId: Yup.string().required(t('required')),
    name: Yup.string().max(250, t('Vmax')).required(t('required')),
    description: Yup.string().optional().max(1024, t('max')),
  });

  const isMutating = useIsMutating();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const { mutate: mutateUpdateCourse } = useMutation({
    mutationFn: updateCourse,
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
        toast.success(t('labels.updateSuccess'), { duration: 2500 });

        const url = `/${activeLocale}/u/courses/${course.id}`;
        replace(url);
      }
    },
  });

  if (isMutating || isUserLoading) {
    return (
      <>
        <h2 className="md:text p-3 text-center text-lg font-semibold text-gray-950 lg:text-xl">
          {t('updateTitle')}

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
    const request: UpdateCourseRequest = {
      id: values.id,
      studyPeriodId: values.studyPeriodId,
      name: values.name,
      description: values.name,
    };

    mutateUpdateCourse(request);
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
            {t('updateTitle')}
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
                {t('labels.updateSubmit')}
              </button>
            </div>
          </Form>
        </div>
      )}
    </Formik>
  );
};

export { UpdateCourseForm };
