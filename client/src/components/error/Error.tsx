import { FC } from 'react';
import { NotFound } from './NotFound';
import { Internal } from './Internal';
import { BadRequest } from '@/components/error/BadRequest';
import { Unauthorized } from '@/components/error/Unauthorized';
import { Forbidden } from '@/components/error/Forbidden';
import { useTranslations } from 'next-intl';

interface ErrorProps {
  error: any;
}

const errorComponents = {
  404: <NotFound />,
  400: <BadRequest />,
  401: <Unauthorized />,
  403: <Forbidden />,
};

const errorComponentsByMessage = {
  'Unauthorized': <Unauthorized />,
  'Forbidden': <Forbidden />,
  'Not Found': <NotFound />,
};

const Error: FC<ErrorProps> = ({ error }) => {
  const t = useTranslations('Error');

  if (error?.detail?.includes('school_profile') ||
    error?.detail?.includes('school_id')) {
    return <Forbidden message={t('invalid_school_profile')} />;
  }

  return (
    errorComponents[error.status] ||
    errorComponentsByMessage[error.message] || <Internal />
  );
};

export { Error };
