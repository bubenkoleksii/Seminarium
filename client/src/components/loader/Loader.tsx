import { FC } from 'react';
import Image from 'next/image';

const Loader: FC = () => {
  return (
    <div className="relative flex justify-center items-center">
      <div className="absolute animate-spin rounded-full h-24 w-24 border-t-4 border-b-4 border-purple-500">
      </div>

      <Image
        alt='loader'
        src="/loader/loader-avatar.svg"
        height={60}
        width={60}
        className="rounded-full"
      />
    </div>
  );
};

export { Loader };
