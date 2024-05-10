import { FC } from 'react';
import Image from 'next/image';

const Loader: FC = () => {
  return (
    <div className="absolute inset-0 flex items-center justify-center bg-gray-200">
      <div className="relative h-24 w-24 animate-spin rounded-full border-b-4 border-t-4 border-purple-500"></div>

      <Image
        alt="loader"
        src="/loader/loader-avatar.svg"
        height={60}
        width={60}
        className="rounded-full"
      />
    </div>
  );
};

export { Loader };
