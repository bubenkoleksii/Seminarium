'use client';

import { UploadFile } from '@/components/file-upload';
import { Tiptap } from '@/components/rich-text';
import { createTheoryLessonItem } from '@/features/user/api/theoryLessonItemsApi';
import { userMutations } from '@/features/user/constants';
import { CreateTheoryLessonItemRequest } from '@/features/user/types/theoryLessonTypes';
import { useMutation } from '@tanstack/react-query';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC, useState } from 'react';
import toast from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './CreateTheoryLessonItemForm.module.scss';

type CreateTheoryLessonItemFormProps = {
  lessonId: string;
  courseId: string;
};

const CreateTheoryLessonItemForm: FC<CreateTheoryLessonItemFormProps> = ({
  lessonId,
  courseId,
}) => {
  const t = useTranslations('TheoryItem');
  const v = useTranslations('Validation');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const [uploadedFiles, setUploadedFiles] = useState<File[]>([]);

  const validationSchema = Yup.object().shape({
    title: Yup.string().max(250, v('max')).required(v('required')),
    text: Yup.string().max(1500, v('max')),
    deadline: Yup.date().nullable(),
    isGraded: Yup.boolean().required(v('required')),
    isArchived: Yup.boolean().required(v('required')),
  });

  const initialValues: CreateTheoryLessonItemRequest = {
    lessonId,
    title: '',
    text: '',
    deadline: null,
    isGraded: false,
    isArchived: false,
  };

  const { mutate: mutateCreateLessonItem } = useMutation({
    mutationFn: createTheoryLessonItem,
    mutationKey: ['createTheoryLessonItem'],
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

  const handleSubmit = async (values: CreateTheoryLessonItemRequest) => {
    const formData = new FormData();

    if (uploadedFiles) {
      uploadedFiles.forEach((file) => {
        formData.append('attachments', file);
      });
    }

    formData.append('lessonId', lessonId);
    formData.append('title', values.title);
    formData.append('text', values.text);
    if (values.deadline)
      formData.append('deadline', values.deadline.toString());
    formData.append('isGraded', values.isGraded.toString());

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
                id="isGraded"
                name="isGraded"
                className={styles.input}
              />
              <label htmlFor="isGraded" className={`${styles.label} ml-1`}>
                {t('labels.isGraded')}
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

export { CreateTheoryLessonItemForm };
