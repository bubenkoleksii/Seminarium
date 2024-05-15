'use client';

import { FC } from 'react';
import { useTranslations } from 'next-intl';
import { school } from '@/shared/constants';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';

import styles from './JoiningRequestForm.module.scss';
import { replaceEmptyStringsWithNull } from '@/shared/helpers';

const JoiningRequestForm: FC = () => {
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
    ownershipType: Yup.string().required(v('required'))
  });

  const handleSubmit = (values) => {
    replaceEmptyStringsWithNull(values);

    // TODO: send request to server
  };

  return (
    <Formik
      initialValues={{
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
      }}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      <div className={styles.container}>
        <h2 className="mb-4 text-center text-2xl text-gray-950 font-semibold">
          {t('title')}
        </h2>
        <Form className={styles.form}>
          <div>
            <label htmlFor="requesterEmail" className={styles.label}>
              {t('labels.requesterEmail')}
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
            </label>
            <Field as="select" id="type" className={styles.select} name="type">
              <option value="">
              </option>
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
            </label>
            <Field
              as="select"
              id="ownershipType"
              className={styles.select}
              name="ownershipType"
            >
              <option value="">
              </option>
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
            </label>
            <Field
              as="select"
              id="region"
              className={styles.select}
              name="region"
            >
              <option value="">
              </option>
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
            <button type="submit" className={styles.button}>
              {t('labels.submit')}
            </button>
          </div>
        </Form>
      </div>
    </Formik>
  );
};

export { JoiningRequestForm };
