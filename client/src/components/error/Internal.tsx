import { FC } from 'react';
import { useTranslations } from 'next-intl';
import Image from 'next/image';

interface InternalProps {
  message?: string;
}

const Internal: FC<InternalProps> = ({ message }) => {
  const t = useTranslations('Error');

  return (
    <div className="bg-gray-100 h-screen flex justify-center">
      <div className="mt-24 m-auto">
        <div className="emoji-error">
          <Image
            alt="internal error"
            src="/errors/500.svg"
            height={160}
            width={260}
            className="rounded-full m-auto"
          />
        </div>

        <div class="font-bold tracking-widest mt-4 flex flex-col gap-3 justify-center">
          <span class="text-gray-700 text-xl">
            {t('internal')}
          </span>
        </div>
      </div>
    </div>
  );
};

export { Internal };
