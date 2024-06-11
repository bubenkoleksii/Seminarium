'use client';

import { Loader } from '@/components/loader';
import { Tiptap } from '@/components/rich-text';
import { useProfiles } from '@/features/user';
import { createGroupNotice } from '@/features/user/api/groupNoticesApi';
import { userMutations } from '@/features/user/constants';
import { CreateGroupNoticeRequest } from '@/features/user/types/groupNoticesTypes';
import { replaceEmptyStringsWithNull } from '@/shared/helpers';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import { toast } from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './CreateGroupNoticeForm.module.scss';

type CreateGroupNoticeFormProps = {
  groupId: string;
};

const CreateGroupNoticeForm: FC<CreateGroupNoticeFormProps> = ({ groupId }) => {
  const activeLocale = useLocale();
  const { replace } = useRouter();
  const v = useTranslations('Validation');
  const t = useTranslations('GroupNotice');

  const isMutating = useIsMutating();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const { mutate, reset: resetMutation } = useMutation({
    mutationFn: createGroupNotice,
    mutationKey: [userMutations.createGroupNotice],
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
        toast.success(
          t('labels.createSuccess', {
            title: response.title,
          }),
          { duration: 2500 },
        );

        const url = `/${activeLocale}/u/group-notices/${groupId}`;
        replace(url);
      }
    },
  });

  const validationSchema = Yup.object().shape({
    isCrucial: Yup.boolean().required(v('required')),
    title: Yup.string().max(250, v('max')).required(v('required')),
    text: Yup.string().optional().max(1000, v('max')),
  });

  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);

    const request: CreateGroupNoticeRequest = {
      text: values.text,
      isCrucial: values.isCrucial,
      title: values.title,
      groupId,
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
          {t('create.title')}
        </h2>

        <Loader />
      </>
    );
  }

  const initialValues = {
    isCrucial: false,
    title: '',
    text: '',
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
            {t('create.title')}
          </h2>

          <div className="mb-5 flex flex-col items-center justify-center gap-2">
            <div>
              <p className="text-center text-lg font-semibold text-purple-950">
                {activeProfile.schoolName}
              </p>
            </div>

            <div>
              <span
                onClick={() => {
                  const url = `/${activeLocale}/u/group-notices/${groupId}`;

                  replace(url);
                }}
                className="cursor-pointer text-sm font-semibold text-purple-700 hover:text-red-700"
              >
                {t('labels.toList')}
              </span>
            </div>
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

            <div className={styles.checkbox}>
              <Field
                type="checkbox"
                id="isCrucial"
                name="isCrucial"
                className={styles.input}
              />

              <label htmlFor="isCrucial" className={`${styles.label} ml-1`}>
                {t('labels.isCrucial')}
              </label>
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

export { CreateGroupNoticeForm };
