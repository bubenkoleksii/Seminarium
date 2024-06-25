'use client';

import { addResults } from '@/features/user/api/practicalLessonItemSubmitApi';
import { userMutations } from '@/features/user/constants';
import type { AddPracticalItemSubmitResultsRequest } from '@/features/user/types/practicalLessonItemSubmitTypes';
import { replaceEmptyStringsWithNull } from '@/shared/helpers';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import toast from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './AcceptPracticalLessonItemSubmitForm.module.scss';

type AcceptPracticalLessonItemSubmitFormProps = {
  submitId: string;
  itemId: string;
  studentId: string;
};

const AcceptPracticalLessonItemSubmitForm: FC<
  AcceptPracticalLessonItemSubmitFormProps
> = ({ submitId, itemId, studentId }) => {
  const v = useTranslations('Validation');
  const t = useTranslations('PracticalItemSubmit');

  const activeLocale = useLocale();
  const { replace } = useRouter();
  const queryClient = useQueryClient();

  const validationSchema = Yup.object().shape({
    mark: Yup.number().required(v('required')),
    text: Yup.string().optional().max(1024, v('max')),
  });

  const initialValues = {
    submitId,
    mark: null,
    text: '',
  };

  const url = `/${activeLocale}/u/practical-item-submit/getOne/?studentId=${studentId}&itemId=${itemId}`;

  const { mutate: mutateAcceptResults } = useMutation({
    mutationFn: addResults,
    mutationKey: ['acceptPracticalLessonItemSubmit', submitId],
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
          404: v('notFound'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        toast.success(t('resultsSuccess'), {
          duration: 2500,
        });

        replace(url);
      }
    },
    onSettled: async () => {
      queryClient.invalidateQueries({
        queryKey: ['practicalLessonItemSubmit', submitId],
        refetchType: 'all',
      });
    },
  });

  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);

    const request: AddPracticalItemSubmitResultsRequest = {
      id: values.submitId,
      isAccept: true,
      mark: values.mark,
      text: values.text,
    };

    mutateAcceptResults(request);
  };

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      <div className={styles.container}>
        <h2 className="md:text p-2 text-center text-lg font-semibold text-gray-950 lg:text-xl">
          {t('acceptTitle')}

          <p
            onClick={() => replace(url)}
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('acceptBackTo')}
          </p>
        </h2>

        <Form className={styles.form}>
          <div>
            <label htmlFor="mark" className={styles.label}>
              {t('labels.mark')}
            </label>
            <Field
              type="number"
              id="mark"
              name="mark"
              min="1"
              className={styles.input}
            />
          </div>
          <div>
            <label htmlFor="text" className={styles.label}>
              {t('labels.comment')}
            </label>
            <Field
              as="textarea"
              rows="5"
              id="text"
              name="text"
              className={styles.input}
            />
          </div>

          <div>
            <button type="submit" className={styles.button}>
              {t('labels.submit')}
            </button>
          </div>
        </Form>
      </div>
    </Formik>
  );
};

export { AcceptPracticalLessonItemSubmitForm };
