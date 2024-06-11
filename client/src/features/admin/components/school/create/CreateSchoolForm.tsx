'use client';

import { Loader } from '@/components/loader';
import { create } from '@/features/admin/api/schoolApi';
import { AdminClientPaths, adminMutations } from '@/features/admin/constants';
import { school as schoolRoutes } from '@/features/admin/routes';
import type {
  CreateSchoolRequest,
  CreateSchoolRequestWithId,
} from '@/features/admin/types/schoolTypes';
import { school as schoolConstants } from '@/shared/constants';
import { replaceEmptyStringsWithNull } from '@/shared/helpers';
import { useAuthRedirectByRole } from '@/shared/hooks';
import { useIsMutating, useMutation } from '@tanstack/react-query';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { FC } from 'react';
import { toast } from 'react-hot-toast';
import * as Yup from 'yup';
import styles from './CreateSchoolForm.module.scss';

type CreateSchoolFormProps = {
  joiningRequestId: string;
  school: CreateSchoolRequest;
};

const CreateSchoolForm: FC<CreateSchoolFormProps> = ({
  joiningRequestId,
  school,
}) => {
  const t = useTranslations('School');
  const v = useTranslations('Validation');
  const activeLocale = useLocale();
  const { replace } = useRouter();

  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'admin');
  const isMutating = useIsMutating();

  const validationSchema = Yup.object().shape({
    registerCode: Yup.string().required(v('required')),
    name: Yup.string().required(v('required')).max(250, v('max')),
    shortName: Yup.string().max(250, v('max')),
    gradingSystem: Yup.number()
      .required(v('required'))
      .max(10000, v('maxNumber')),
    postalCode: Yup.string()
      .required(v('required'))
      .max(999999, v('maxNumber')),
    studentsQuantity: Yup.number()
      .required(v('required'))
      .max(1000000, v('maxNumber')),
    territorialCommunity: Yup.string().max(250, v('max')),
    address: Yup.string().max(250, v('max')),
    type: Yup.string().required(v('required')),
    region: Yup.string().required(v('required')),
    ownershipType: Yup.string().required(v('required')),
  });

  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);

    const request: CreateSchoolRequestWithId = {
      joiningRequestId,
      registerCode: values.registerCode,
      name: values.name,
      shortName: values.shortName,
      gradingSystem: values.gradingSystem,
      studentsQuantity: values.studentsQuantity,
      type: values.type,
      postalCode: values.postalCode,
      ownershipType: values.ownershipType,
      region: values.region,
      territorialCommunity: values.territorialCommunity,
      address: values.address,
      areOccupied: values.areOccupied === `true`,
    };

    mutate(request);
  };

  const {
    mutate,
    isPending,
    reset: resetMutation,
  } = useMutation({
    mutationFn: create,
    mutationKey: [adminMutations.createSchool],
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          400: t('labels.validation'),
          409: t('labels.alreadyExists'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
          { duration: 4000 },
        );
      } else {
        toast.success(t('labels.createSuccess'), { duration: 1500 });

        const { id } = response;
        const successRoute = `/${activeLocale}/${schoolRoutes.getOne(id)}`;
        replace(successRoute);
      }
    },
  });

  if (isPending || isMutating || isUserLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('createTitle')}
          <span
            onClick={() =>
              replace(
                `/${activeLocale}/${AdminClientPaths.JoiningRequests}/${joiningRequestId}`,
              )
            }
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('labels.toRequest')}
          </span>
        </h2>
        <Loader />
      </>
    );
  }

  return (
    <Formik
      initialValues={school}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      <div className={styles.container}>
        <h2 className="mb-4 text-center text-xl font-bold">
          {t('createTitle')}
          <span
            onClick={() =>
              replace(
                `/${activeLocale}/${AdminClientPaths.JoiningRequests}/${joiningRequestId}`,
              )
            }
            className="ml-2 cursor-pointer pt-1 text-sm text-purple-700 hover:text-red-700"
          >
            {t('labels.toRequest')}
          </span>
        </h2>

        <Form className={styles.form}>
          <div>
            <label htmlFor="registerCode" className={styles.label}>
              {t('labels.registerCode')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.registerCode')}
              type="text"
              id="registerCode"
              name="registerCode"
              className={styles.input}
            />
            <ErrorMessage
              name="registerCode"
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
            <label htmlFor="shortName" className={styles.label}>
              {t('labels.shortName')}
            </label>
            <Field
              placeholder={t('placeholders.shortName')}
              type="text"
              id="shortName"
              name="shortName"
              className={styles.input}
            />
            <ErrorMessage
              name="shortName"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="gradingSystem" className={styles.label}>
              {t('labels.gradingSystem')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.gradingSystem')}
              type="text"
              id="gradingSystem"
              name="gradingSystem"
              className={styles.input}
            />
            <ErrorMessage
              name="gradingSystem"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="type" className={styles.label}>
              {t('labels.type')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field as="select" id="type" className={styles.select} name="type">
              <option value=""></option>
              {Object.values(schoolConstants.type).map((value, index) => (
                <option key={index} value={value}>
                  {t(`types.${value}`)}
                </option>
              ))}
            </Field>
            <ErrorMessage
              name="type"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="postalCode" className={styles.label}>
              {t('labels.postalCode')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.postalCode')}
              type="text"
              id="postalCode"
              name="postalCode"
              className={styles.input}
            />
            <ErrorMessage
              name="postalCode"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="ownershipType" className={styles.label}>
              {t('labels.ownershipType')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              as="select"
              id="ownershipType"
              className={styles.select}
              name="ownershipType"
            >
              <option value=""></option>
              {Object.values(schoolConstants.ownershipType).map(
                (value, index) => (
                  <option key={index} value={value}>
                    {t(`ownershipTypes.${value}`)}
                  </option>
                ),
              )}
            </Field>
            <ErrorMessage
              name="ownershipType"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="studentsQuantity" className={styles.label}>
              {t('labels.studentsQuantity')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.studentsQuantity')}
              type="number"
              id="studentsQuantity"
              name="studentsQuantity"
              className={styles.input}
            />
            <ErrorMessage
              name="studentsQuantity"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="region" className={styles.label}>
              {t('labels.region')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              as="select"
              id="region"
              className={styles.select}
              name="region"
            >
              <option value=""></option>
              {Object.values(schoolConstants.region).map((value, index) => (
                <option key={index} value={value}>
                  {t(`regions.${value}`)}
                </option>
              ))}
            </Field>
            <ErrorMessage
              name="region"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="territorialCommunity" className={styles.label}>
              {t('labels.territorialCommunity')}
            </label>
            <Field
              placeholder={t('placeholders.territorialCommunity')}
              type="text"
              id="territorialCommunity"
              name="territorialCommunity"
              className={styles.input}
            />
            <ErrorMessage
              name="territorialCommunity"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="address" className={styles.label}>
              {t('labels.address')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.address')}
              type="text"
              id="address"
              name="address"
              className={styles.input}
            />
            <ErrorMessage
              name="address"
              component="div"
              className={styles.error}
            />
          </div>
          <div className={styles.checkbox}>
            <Field
              type="checkbox"
              id="areOccupied"
              name="areOccupied"
              className={styles.input}
            />
            <label
              htmlFor="areOccupied"
              className={`${styles.label} ml-2 cursor-pointer`}
            >
              {t('labels.areOccupied')}
            </label>
          </div>
          <div>
            <button
              onClick={() => resetMutation()}
              type="submit"
              className={styles.button}
            >
              {t('labels.submit')}
            </button>
          </div>
        </Form>
      </div>
    </Formik>
  );
};

export { CreateSchoolForm };
