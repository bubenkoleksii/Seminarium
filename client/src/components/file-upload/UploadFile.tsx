import { FC, ChangeEvent, useState } from 'react';
import { Formik, Form, ErrorMessage } from 'formik';
import { FileInput, Label } from 'flowbite-react';
import { useTranslations } from 'next-intl';

import {
  ALLOWED_IMAGE_EXTENSIONS,
  ALLOWED_FILE_EXTENSIONS,
  MAX_IMAGE_FILE_SIZE,
  MAX_FILE_SIZE,
} from './constants';
import { mediaQueries } from '@/shared/constants';
import { useMediaQuery } from 'react-responsive';

interface UploadFileProps {
  isRequired?: boolean;
  isMultiple?: boolean;
  isImage?: boolean;
  label?: string;
  onSubmit: (values: { files: File | File[] }) => void;
}

const UploadFile: FC<UploadFileProps> = ({
  isRequired = false,
  label,
  isMultiple = false,
  isImage = false,
  onSubmit,
}) => {
  const t = useTranslations('UploadFile');
  const [error, setError] = useState<string>('');

  const isPhone = useMediaQuery({ query: mediaQueries.phone });

  const allowedExtensions = isImage
    ? ALLOWED_IMAGE_EXTENSIONS
    : ALLOWED_FILE_EXTENSIONS;
  const maxSize = isImage ? MAX_IMAGE_FILE_SIZE : MAX_FILE_SIZE;

  const validateFile = (file: File) => {
    if (
      !allowedExtensions.some((ext) => file?.name?.toLowerCase().endsWith(ext))
    ) {
      return 'invalidExt';
    }

    if (file.size > maxSize) {
      return 'maxSize';
    }
  };

  const handleFileChange = (
    event: ChangeEvent<HTMLInputElement>,
    setFieldValue: (field: string, value: any) => void,
  ) => {
    const files = Array.from(event.currentTarget.files || []);

    if (isMultiple) {
      files.forEach((file) => {
        const error = validateFile(file);
        if (error) {
          setError(error);
          return;
        }
      });
    } else {
      const error = validateFile(files[0]);
      if (error) {
        setError(error);
        return;
      }
    }

    setFieldValue('files', isMultiple ? files : files[0]);
    setError('');

    if (isImage && !isMultiple && onSubmit) {
      onSubmit({ files: files[0] });
    }
  };

  const title = label || isImage ? t('imageTitle') : t('fileTitle');

  return (
    <Formik
      initialValues={{ files: isMultiple ? [] : null }}
      onSubmit={(values, { resetForm }) => {
        if (isRequired && !values.files) {
          setError('fileRequired');
          return;
        }

        if (
          isMultiple &&
          Array.isArray(values.files) &&
          values.files.length === 0
        ) {
          setError('filesRequired');
          return;
        }

        if (onSubmit) {
          onSubmit(values);
          resetForm();
        }
      }}
    >
      {({ setFieldValue }) => (
        <Form className="flex flex-col items-center rounded-md p-1">
          <div className="mb-1">
            <Label
              className="text-center"
              htmlFor="file-upload"
              value={title}
            />
            <FileInput
              id="file-upload"
              name="files"
              color="purple"
              sizing={isPhone ? `sm` : ``}
              accept={allowedExtensions.join(', ')}
              multiple={isMultiple}
              onChange={(event) => handleFileChange(event, setFieldValue)}
              className="custom-file-input rounded-md"
            />
            <ErrorMessage
              name="files"
              component="div"
              className="mt-1 text-red-500"
            />

            {error && <p className="text-sm text-red-500">{t(error)}</p>}
          </div>
        </Form>
      )}
    </Formik>
  );
};

export { UploadFile };
