import { FC } from 'react';
import { useTranslations } from 'next-intl';
import Image from 'next/image';

interface NotFoundProps {
  message?: string;
}

const NotFound: FC<NotFoundProps> = ({ message }) => {
  const t = useTranslations('Error');

  return (
    <div className="bg-gray-100 h-screen flex justify-center">
      <div className="mt-8 m-auto">
        <div className="emoji-error">
          <Image
            alt="Not Found"
            src="/errors/404.svg"
            height={160}
            width={260}
            className="rounded-full m-auto"
          />
        </div>

        <div class="font-bold tracking-widest mt-4 flex flex-col gap-3 justify-center">
          <span class="text-gray-700 text-2xl text-center p-2 text-xl">
            {message ? message : t('notFound')}
          </span>
        </div>
      </div>
    </div>
  );
};

export { NotFound };
