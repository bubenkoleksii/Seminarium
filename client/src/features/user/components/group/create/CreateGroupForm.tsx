'use client';

import styles from './CreateGroupForm.module.scss';
import { FC } from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { toast } from 'react-hot-toast';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { Loader } from '@/components/loader';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { useProfiles } from '@/features/user';
import { useRouter } from 'next/navigation';
import { create } from '@/features/user/api/groupsApi';
import { userMutations } from '@/features/user/constants';

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
        const errorMessages = {
          409: t('labels.alreadyExists'),
          400: t('labels.invitationValidation'),
          401: t('labels.unauthorized'),
          403: t('labels.forbidden'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
        );
      } else {
        toast.success(t('labels.createSuccess'), { duration: 2500 });

        const url = `/${activeLocale}/u/groups/?studyPeriodNumber=${response.studyPeriodNumber}`;
        replace(url);
      }
    }
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
      schoolProfileId: activeProfile.id,
    });
  }

  if (profilesLoading || isMutating || isUserLoading) {
    return (
      <>
        <h2 className="md:text mb-4 pt-6 text-center text-2xl font-semibold text-gray-950">
          {t('create.title')}
        </h2>

        <Loader />
      </>
    )
  }

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      <div className={styles.container}>
        <h2 className="p-3 text-center text-lg md:text lg:text-xl font-semibold text-gray-950">
          {t('create.title')}
        </h2>

        <div className="flex flex-col justify-center items-center gap-2 mb-5">
          <div>
            <p className="text-center font-semibold text-lg text-purple-950" >
              {activeProfile.schoolName}
            </p>
          </div>

          <div>
            <span
              onClick={() => {
                const url = `/${activeLocale}/u/groups`;

                replace(url);
              }}
              className="cursor-pointer font-semibold text-sm text-purple-700 hover:text-red-700"
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
