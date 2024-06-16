'use client';

import { Loader } from '@/components/loader';
import { createLesson } from '@/features/user/api/lessonsApi';
import { userMutations } from '@/features/user/constants';
import { CreateLessonRequest } from '@/features/user/types/lessonTypes';
import { replaceEmptyStringsWithNull } from '@/shared/helpers';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import toast from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './CreateLessonForm.module.scss';

type CreateLessonFormProps = {
  courseId: string;
};

const CreateLessonForm: FC<CreateLessonFormProps> = ({ courseId }) => {
  const v = useTranslations('Validation');
  const t = useTranslations('Lesson');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const isMutating = useIsMutating();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const validationSchema = Yup.object().shape({
    number: Yup.number().required(v('required')),
    topic: Yup.string().max(250, v('max')).required(v('required')),
    homework: Yup.string().optional().max(1024, v('max')),
  });

  const initialValues = {
    courseId: courseId,
    number: null,
    topic: '',
    homework: '',
    needPracticalItem: false,
  };

  const { mutate: mutateCreateLesson, reset: resetCreateLesson } = useMutation({
    mutationFn: createLesson,
    mutationKey: [userMutations.createLesson],
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
        toast.success(t('createLessonSuccess'), {
          duration: 2500,
        });

        const url = `/${activeLocale}/u/courses/${courseId}`;
        replace(url);
      }
    },
  });

  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);

    const request: CreateLessonRequest = {
      courseId: values.courseId,
      number: values.number,
      topic: values.topic,
      homework: values.homework,
      needPracticalItem: values.needPracticalItem,
    };

    mutateCreateLesson(request);
  };

  if (isMutating || isUserLoading) {
    return (
      <>
        <h2 className="md:text p-2 text-center text-lg font-semibold text-gray-950 lg:text-xl">
          {t('createTitle')}
        </h2>

        <Loader />
      </>
    );
  }

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      <div className={styles.container}>
        <h2 className="md:text p-2 text-center text-lg font-semibold text-gray-950 lg:text-xl">
          {t('createTitle')}
        </h2>

        <div>
          <span
            onClick={() => {
              const url = `/${activeLocale}/u/courses/${courseId}`;

              replace(url);
            }}
            className="cursor-pointer text-sm font-semibold text-purple-700 hover:text-red-700"
          >
            {t('labels.toCourse')}
          </span>
        </div>
        <Form className={styles.form}>
          <div>
            <label htmlFor="number" className={styles.label}>
              {t('labels.number')}
              <span className={styles.required}>*</span>
            </label>
            <Field
              type="number"
              id="number"
              name="number"
              min="1"
              className={styles.input}
            />
          </div>
          <div>
            <label htmlFor="topic" className={styles.label}>
              {t('labels.topic')}
              <span className={styles.required}>*</span>
            </label>
            <Field
              type="text"
              id="topic"
              name="topic"
              className={styles.input}
            />
          </div>
          <div>
            <label htmlFor="homework" className={styles.label}>
              {t('labels.homework')}
            </label>
            <Field
              as="textarea"
              rows="5"
              id="homework"
              name="homework"
              className={styles.input}
            />
          </div>
          <div className={styles.checkbox}>
            <Field
              type="checkbox"
              id="needPracticalItem"
              name="needPracticalItem"
              className={styles.input}
            />
            <label
              htmlFor="needPracticalItem"
              className={`${styles.label} ml-2 cursor-pointer`}
            >
              {t('labels.createHomeworkTask')}
            </label>
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

export { CreateLessonForm };

