import { FC } from 'react';
import { useTranslations } from 'next-intl';
import Image from 'next/image';

interface InternalProps {
  message?: string;
}

const Internal: FC<InternalProps> = ({ message }) => {
  const t = useTranslations('Error');

  return (
    <div className="flex h-screen justify-center bg-gray-100">
      <div className="m-auto mt-24">
        <div className="emoji-error">
          <Image
            alt="internal error"
            src="/errors/500.svg"
            height={160}
            width={260}
            className="m-auto rounded-full"
          />
        </div>

        <div className="mt-4 flex flex-col justify-center gap-3 font-bold tracking-widest">
          <span className="text-xl text-gray-700">
            {message || t('internal')}
          </span>
        </div>
      </div>
    </div>
  );
};

export { Internal };
