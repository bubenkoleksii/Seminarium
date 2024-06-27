'use client';

import { Loader } from '@/components/loader';
import { Tiptap } from '@/components/rich-text';
import { updatePracticalLessonItem } from '@/features/user/api/practicalLessonItemsApi';
import { userMutations } from '@/features/user/constants';
import { UpdatePracticalLessonItemRequest } from '@/features/user/types/practicalLessonItemTypes';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import toast from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './UpdatePracticalLessonItemForm.module.scss';

type UpdatePracticalLessonItemFormProps = {
  id: string;
  practicalLessonItem: UpdatePracticalLessonItemRequest;
};

const UpdatePracticalLessonItemForm: FC<UpdatePracticalLessonItemFormProps> = ({
  id,
  practicalLessonItem,
}) => {
  const t = useTranslations('PracticalItem');
  const v = useTranslations('Validation');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  practicalLessonItem.allowSubmitAfterDeadline =
    practicalLessonItem.allowSubmitAfterDeadline == 'true' ||
    practicalLessonItem.allowSubmitAfterDeadline == true;

  const validationSchema = Yup.object().shape({
    title: Yup.string().max(250, v('max')).required(v('required')),
    text: Yup.string().max(1500, v('max')),
    deadline: Yup.date().nullable(),
    allowSubmitAfterDeadline: Yup.boolean().required(v('required')),
  });

  const isMutating = useIsMutating();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const { mutate: mutateUpdatePracticalLessonItem } = useMutation({
    mutationFn: updatePracticalLessonItem,
    mutationKey: ['updatePracticalLessonItem'],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('updateSuccess'), { duration: 2500 });

        const url = `/${activeLocale}/u/practical-item/getAll/?courseId=${practicalLessonItem.courseId}&lessonId=${practicalLessonItem.lessonId}`;
        replace(url);
      }
    },
  });

  if (isMutating || isUserLoading) {
    return (
      <>
        <h2 className="md:text p-3 text-center text-lg font-semibold text-gray-950 lg:text-xl">
          {t('updateTitle')}
        </h2>
        <Loader />
      </>
    );
  }

  const handleSubmit = (values) => {
    const request = {
      id: values.id,
      deadline: values.deadline,
      title: values.title,
      text: values.text,
      allowSubmitAfterDeadline: values.allowSubmitAfterDeadline,
    };

    mutateUpdatePracticalLessonItem(request);
  };

  return (
    <Formik
      initialValues={practicalLessonItem}
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
              <label htmlFor="title" className={styles.label}>
                {t('labels.title')}
                <span className="text-md ml-1 text-red-500">*</span>
              </label>
              <Field
                type="text"
                id="title"
                name="title"
                className={styles.input}
              />
              <ErrorMessage
                name="title"
                component="div"
                className={styles.error}
              />
            </div>

            <div>
              <label htmlFor="text" className={styles.label}>
                {t('labels.text')}
              </label>
              <Tiptap
                content={practicalLessonItem.text}
                limit={500}
                onChange={(newContent: string) =>
                  setFieldValue('text', newContent)
                }
              />
              <ErrorMessage
                name="text"
                component="div"
                className={styles.error}
              />
            </div>

            <div>
              <label htmlFor="deadline" className={styles.label}>
                {t('labels.deadline')}
              </label>
              <Field
                type="datetime-local"
                id="deadline"
                name="deadline"
                className={styles.input}
              />
              <ErrorMessage
                name="deadline"
                component="div"
                className={styles.error}
              />
            </div>

            <div className={styles.checkbox}>
              <Field
                type="checkbox"
                id="allowSubmitAfterDeadline"
                name="allowSubmitAfterDeadline"
                className={styles.input}
              />
              <label
                htmlFor="allowSubmitAfterDeadline"
                className={`${styles.label} ml-1`}
              >
                {t('labels.allowSubmitAfterDeadline')}
              </label>
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

export { UpdatePracticalLessonItemForm };
