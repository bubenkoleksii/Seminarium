import { FC } from 'react';
import { Button } from 'flowbite-react';
import { useTranslations } from 'next-intl';
import Link from 'next/link';

type Props = {
  params: {
    slug: string[];
  };
};

const CreateJoiningRequestSuccessPage: FC<Props> = ({ params }) => {
  const t = useTranslations('CreateJoiningRequestSuccess');
  const [id, email] = params.slug.map((param) => decodeURIComponent(param));

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-100">
      <div className="max-w-xl rounded-lg bg-white p-8 text-center shadow-lg">
        <h1 className="mb-4 text-2xl font-semibold text-gray-800">
          {t('success')}
        </h1>
        <p className="mb-6 text-gray-600">{t('text')}</p>
        <div className="mb-6 text-left">
          <p className="text-gray-800">
            <strong>{t('id')}</strong>
            <p className>{id}</p>
          </p>
          <p className="mt-2 text-gray-800">
            <strong>{t('email')}</strong>
            <p className>{email}</p>
          </p>
        </div>
        <Link href="/" className="flex items-center justify-center">
          <Button gradientMonochrome="purple">
            <span className="text-white">{t('goHome')}</span>
          </Button>
        </Link>
      </div>
    </div>
  );
};

export default CreateJoiningRequestSuccessPage;
