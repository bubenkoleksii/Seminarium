import { FC } from 'react';
import { useTranslations } from 'next-intl';
import Image from 'next/image';

interface BadRequestProps {
  message?: string;
}

const BadRequest: FC<BadRequestProps> = ({ message }) => {
  const t = useTranslations('Error');

  return (
    <div className="flex h-screen justify-center bg-gray-100">
      <div className="m-auto mt-8">
        <div className="emoji-error">
          <Image
            alt="Bad Request"
            src="/errors/400.svg"
            height={160}
            width={260}
            className="m-auto rounded-full"
          />
        </div>

        <div className="mt-4 flex flex-col justify-center gap-3 font-bold tracking-widest">
          <span className="p-2 text-center text-2xl text-xl text-gray-700">
            {message ? message : t('badRequest')}
          </span>
        </div>
      </div>
    </div>
  );
};

export { BadRequest };
