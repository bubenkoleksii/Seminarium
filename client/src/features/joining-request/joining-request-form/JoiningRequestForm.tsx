'use client';

import styles from './JoiningRequestForm.module.scss';

import { FC, useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { toast } from 'react-hot-toast';
import { school } from '@/shared/constants';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { replaceEmptyStringsWithNull } from '@/shared/helpers';
import { useMutation, useIsMutating } from '@tanstack/react-query';
import { createJoiningRequest } from '@/features/joining-request/api';
import { Loader } from '@/components/loader';
import { createJoiningRequestSuccessRoute } from '@/features/joining-request/constants';
import { useRouter } from 'next/navigation';
import { ProveModal } from '@/components/modal';

const JoiningRequestForm: FC = () => {
  const activeLocale = useLocale();
  const { replace } = useRouter();

  const isMutating = useIsMutating();

  const t = useTranslations('JoiningRequest');
  const v = useTranslations('Validation');

  const validationSchema = Yup.object().shape({
    requesterEmail: Yup.string()
      .email(v('email'))
      .required(v('required'))
      .max(50, v('max')),
    requesterPhone: Yup.string().required(v('required')).max(50, v('max')),
    requesterFullName: Yup.string().required(v('required')).max(250, v('max')),
    registerCode: Yup.string().required(v('required')),
    name: Yup.string().required(v('required')).max(250, v('max')),
    shortName: Yup.string().max(250, v('max')),
    gradingSystem: Yup.number()
      .required(v('required'))
      .max(10000, v('maxNumber')),
    postalCode: Yup.number()
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

  const [formValues, setFormValues] = useState(null);
  const [openModal, setOpenModal] = useState(false);
  const handleOpenModal = () => {
    setOpenModal(true);
  };
  const handleCloseModal = (confirmed: boolean) => {
    setOpenModal(false);

    if (formValues && confirmed) {
      mutate(formValues);
    } else {
      toast.success(t('labels.disproved'), { duration: 2000 });
    }
  };
  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);
    handleOpenModal();

    setFormValues(values);
  };

  const { mutate, reset: resetMutation } = useMutation({
    mutationFn: createJoiningRequest,
    mutationKey: ['createJoiningRequest'],
    onSuccess: (response) => {
      if (response && response.error) {
        const errorMessages = {
          400: t(`labels.validation`),
          409: t('labels.alreadyExists'),
        };

        toast.error(
          errorMessages[response.error.status] || t('labels.internal'),
          { duration: 10000 },
        );
      } else {
        toast.success(t('labels.success'), { duration: 1500 });

        const { id, requesterEmail: email } = response;
        const successRoute = `/${activeLocale}/${createJoiningRequestSuccessRoute}/${id}/${email}`;
        replace(successRoute);
      }
    },
  });

  if (isMutating) {
    return (
      <>
        <h2 className="mb-4 text-center text-2xl font-semibold text-gray-950">
          {t('title')}
        </h2>
        <Loader />
      </>
    );
  }

  return (
    <Formik
      initialValues={
        formValues
          ? formValues
          : {
              requesterEmail: '',
              requesterPhone: '',
              requesterFullName: '',
              registerCode: '',
              name: '',
              shortName: '',
              gradingSystem: '',
              type: '',
              postalCode: '',
              ownershipType: '',
              studentsQuantity: '',
              region: '',
              territorialCommunity: '',
              address: '',
              areOccupied: false,
            }
      }
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      <div className={styles.container}>
        <h2 className="mb-4 text-center text-2xl font-semibold text-gray-950">
          {t('title')}
        </h2>
        <ProveModal
          open={openModal}
          text={t('labels.prove')}
          onClose={handleCloseModal}
        />
        <Form className={styles.form}>
          <div>
            <label htmlFor="requesterEmail" className={styles.label}>
              <span>{t('labels.requesterEmail')}</span>
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.requesterEmail')}
              type="email"
              id="requesterEmail"
              name="requesterEmail"
              autoComplete="email"
              className={styles.input}
            />
            <ErrorMessage
              name="requesterEmail"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="requesterPhone" className={styles.label}>
              {t('labels.requesterPhone')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.requesterPhone')}
              type="tel"
              id="requesterPhone"
              name="requesterPhone"
              autoComplete="tel"
              className={styles.input}
            />
            <ErrorMessage
              name="requesterPhone"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="requesterFullName" className={styles.label}>
              {t('labels.requesterFullName')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.requesterFullName')}
              type="text"
              id="requesterFullName"
              name="requesterFullName"
              className={styles.input}
            />
            <ErrorMessage
              name="requesterFullName"
              component="div"
              className={styles.error}
            />
          </div>
          <div>
            <label htmlFor="registerCode" className={styles.label}>
              {t('labels.registerCode')}
              <span className="text-md ml-1 text-red-500">*</span>
            </label>
            <Field
              placeholder={t('placeholders.registerCode')}
              type="number"
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
              {Object.values(school.type).map((value, index) => (
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
              type="number"
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
              {Object.values(school.ownershipType).map((value, index) => (
                <option key={index} value={value}>
                  {t(`ownershipTypes.${value}`)}
                </option>
              ))}
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
              {Object.values(school.region).map((value, index) => (
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

export { JoiningRequestForm };
