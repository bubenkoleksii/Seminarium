'use client';

import { UploadFile } from '@/components/file-upload';
import { Tiptap } from '@/components/rich-text';
import { createPracticalLessonItem } from '@/features/user/api/practicalLessonItemsApi';
import { userMutations } from '@/features/user/constants';
import { CreatePracticalLessonItemRequest } from '@/features/user/types/practicalLessonItemTypes';
import { useMutation } from '@tanstack/react-query';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC, useState } from 'react';
import toast from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './CreatePracticalLessonItemForm.module.scss';

type CreatePracticalLessonItemFormProps = {
  lessonId: string;
  courseId: string;
};

const CreatePracticalLessonItemForm: FC<CreatePracticalLessonItemFormProps> = ({
  lessonId,
  courseId,
}) => {
  const t = useTranslations('PracticalItem');
  const v = useTranslations('Validation');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const [uploadedFiles, setUploadedFiles] = useState<File[]>([]);

  const validationSchema = Yup.object().shape({
    title: Yup.string().max(250, v('max')).required(v('required')),
    text: Yup.string().max(1500, v('max')),
    deadline: Yup.date().nullable(),
    attempts: Yup.number().nullable().min(0, v('min')),
    allowSubmitAfterDeadline: Yup.boolean().required(v('required')),
    isArchived: Yup.boolean().required(v('required')),
  });

  const initialValues: CreatePracticalLessonItemRequest = {
    lessonId,
    title: '',
    text: '',
    deadline: null,
    attempts: undefined,
    allowSubmitAfterDeadline: false,
    isArchived: false,
  };

  const { mutate: mutateCreateLessonItem } = useMutation({
    mutationFn: createPracticalLessonItem,
    mutationKey: ['createPracticalLessonItem'],
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

        const url = `/${activeLocale}/u/courses/${courseId}`;
        replace(url);
      }
    },
  });

  const handleFilesSubmit = (values: { files: File[] }) => {
    setUploadedFiles(values.files);
  };

  const handleSubmit = async (values: CreatePracticalLessonItemRequest) => {
    const formData = new FormData();

    if (uploadedFiles) {
      uploadedFiles.forEach((file) => {
        formData.append('attachments', file);
      });
    }

    formData.append('lessonId', lessonId);
    formData.append('title', values.title);
    formData.append('text', values.text || '');
    if (values.deadline) formData.append('deadline', values.deadline.toString());
    if (values.attempts) formData.append('attempts', values.attempts?.toString() || '');
    formData.append('allowSubmitAfterDeadline', values.allowSubmitAfterDeadline.toString());
    formData.append('isArchived', values.isArchived.toString());

    mutateCreateLessonItem(formData);
  };

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      {({ setFieldValue }) => (
        <div className={styles.container}>
          <h2 className="md:text p-3 text-center text-lg font-semibold text-gray-950 lg:text-xl">
            {t('createTitle')}
          </h2>

          <Form className={styles.form}>
            <div>
              <label htmlFor="title" className={styles.label}>
                {t('labels.title')}
                <span className="text-md ml-1 text-red-500">*</span>
              </label>
              <Field
                placeholder={t('placeholders.title')}
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
                content={initialValues.text}
                limit={1500}
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

            <div>
              <label htmlFor="attempts" className={styles.label}>
                {t('labels.attempts')}
              </label>
              <Field
                placeholder={t('placeholders.attempts')}
                type="number"
                id="attempts"
                name="attempts"
                className={styles.input}
              />
              <ErrorMessage
                name="attempts"
                component="div"
                className={styles.error}
              />
            </div>

            <div className="mt-2 flex flex-col items-center justify-center">
              <UploadFile
                isImage={false}
                isMultiple={true}
                label={t('addAttachments')}
                onSubmit={handleFilesSubmit}
              />
            </div>

            <div className={styles.checkbox}>
              <Field
                type="checkbox"
                id="allowSubmitAfterDeadline"
                name="allowSubmitAfterDeadline"
                className={styles.input}
              />
              <label htmlFor="allowSubmitAfterDeadline" className={`${styles.label} ml-1`}>
                {t('labels.allowSubmitAfterDeadline')}
              </label>
            </div>

            <div className={styles.checkbox}>
              <Field
                type="checkbox"
                id="isArchived"
                name="isArchived"
                className={styles.input}
              />
              <label htmlFor="isArchived" className={`${styles.label} ml-1`}>
                {t('labels.isArchived')}
              </label>
            </div>

            <div>
              <button type="submit" className={styles.button}>
                {t('labels.createSubmit')}
              </button>
            </div>
          </Form>
        </div>
      )}
    </Formik>
  );
};

export { CreatePracticalLessonItemForm };

