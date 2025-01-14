'use client';

import { Loader } from '@/components/loader';
import { useProfiles } from '@/features/user';
import { create } from '@/features/user/api/groupsApi';
import { userMutations } from '@/features/user/constants';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import { toast } from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './CreateGroupForm.module.scss';

const CreateGroupForm: FC = () => {
  const activeLocale = useLocale();
  const { replace } = useRouter();
  const v = useTranslations('Validation');
  const t = useTranslations('Group');

  const isMutating = useIsMutating();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');

  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const { mutate, reset: resetMutation } = useMutation({
    mutationFn: create,
    mutationKey: [userMutations.createGroup],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
      if (response && response.error) {
        if (response.error.detail.includes('school_profile')) {
          toast.error(t('labels.invalid_school_profile'));

          return;
        }

        const errorMessages = {
          409: t('labels.alreadyExists'),
          400: t('validation'),
          401: t('labels.unauthorized'),
          403: t('labels.forbidden'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
        );
      } else {
        toast.success(
          t('labels.createSuccess', {
            name: response.name,
          }),
          { duration: 2500 },
        );

        const url = `/${activeLocale}/u/groups/?studyPeriodNumber=${response.studyPeriodNumber}`;
        replace(url);
      }
    },
  });

  const validationSchema = Yup.object().shape({
    name: Yup.string().max(250, v('max')),
    studyPeriodNumber: Yup.number()
      .min(0, v('minNumber'))
      .max(100, v('maxNumber')),
  });

  const initialValues = {
    name: '',
    studyPeriodNumber: null,
  };

  const handleSubmit = (values) => {
    mutate({
      data: values,
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

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
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
                const url = `/${activeLocale}/u/groups`;

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
            <label htmlFor="studyPeriodNumber" className={styles.label}>
              {t('labels.studyPeriodNumber')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder="8"
              type="number"
              id="studyPeriodNumber"
              name="studyPeriodNumber"
              className={styles.input}
            />
            <ErrorMessage
              name="studyPeriodNumber"
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
    </Formik>
  );
};

export { CreateGroupForm };
