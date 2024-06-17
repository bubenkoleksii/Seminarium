'use client';

import { UploadFile } from '@/components/file-upload';
import { Loader } from '@/components/loader';
import { Tiptap } from '@/components/rich-text';
import { createPracticalLessonItemSubmit } from '@/features/user/api/practicalLessonItemSubmitApi';
import { useProfiles } from '@/features/user/hooks';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { ErrorMessage, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC, useState } from 'react';
import toast from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './CreatePracticalLessonItemSubmit.module.scss';

type CreatePracticalLessonItemSubmitProps = {
  itemId: string;
};

const CreatePracticalLessonItemSubmit: FC<
  CreatePracticalLessonItemSubmitProps
> = ({ itemId }) => {
  const t = useTranslations('PracticalItemSubmit');
  const v = useTranslations('Validation');

  const activeLocale = useLocale();
  const { replace } = useRouter();

  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const isMutating = useIsMutating();

  const [uploadedFiles, setUploadedFiles] = useState<File[]>([]);

  const validationSchema = Yup.object().shape({
    text: Yup.string().max(1500, v('max')),
  });

  const initialValues = {
    practicalLessonItemId: itemId,
    text: '',
  };

  const { mutate: mutateCreateLessonItemSubmit } = useMutation({
    mutationFn: createPracticalLessonItemSubmit,
    mutationKey: ['createPracticalLessonItemSubmit'],
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          400: v('validation'),
          401: v('unauthorized'),
          403: v('forbidden'),
          415: v('invalidFormat'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('createSuccess'), {
          duration: 2500,
        });

        replace(
          `/${activeLocale}/u/practical-item-submit/getOne/?itemId=${itemId}&studentId=${activeProfile.id}`,
        );
      }
    },
  });

  const handleFilesSubmit = (values: { files: File[] }) => {
    setUploadedFiles(values.files);
  };

  const handleSubmit = async (values: typeof initialValues) => {
    const formData = new FormData();

    if (uploadedFiles) {
      uploadedFiles.forEach((file) => {
        formData.append('attachments', file);
      });
    }

    formData.append('practicalLessonItemId', itemId);
    formData.append('text', values.text || '');

    mutateCreateLessonItemSubmit(formData);
  };

  if (isMutating || profilesLoading) {
    return (
      <>
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
      {({ setFieldValue }) => (
        <div className={styles.container}>
          <h2 className="md:text p-3 text-center text-lg font-semibold text-gray-950 lg:text-xl">
            {t('createTitle')}
          </h2>

          <div className="mt-2 flex flex-col items-center justify-center">
            <UploadFile
              isImage={false}
              isMultiple={true}
              label={t('addAttachments')}
              onSubmit={handleFilesSubmit}
            />
          </div>

          <Form className={styles.form}>
            <div>
              <label htmlFor="text" className={styles.label}>
                {t('labels.text')}
              </label>
              <div>
                <Tiptap
                  content={initialValues.text}
                  limit={1500}
                  onChange={(newContent: string) =>
                    setFieldValue('text', newContent)
                  }
                />
              </div>
              <ErrorMessage
                name="text"
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
      )}
    </Formik>
  );
};

export { CreatePracticalLessonItemSubmit };

