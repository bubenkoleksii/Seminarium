import { FC } from 'react';
import Image from 'next/image';

const Loader: FC = () => {
  return (
    <div className="absolute inset-0 flex h-[75%] items-center justify-center">
      <div
        className="relative mr-1 flex h-24 w-24 animate-spin
      justify-center rounded-full border-b-4 border-t-4 border-purple-500"
      >
        <Image
          alt="loader"
          src="/loader/loader-avatar.svg"
          height={60}
          width={60}
          className="rounded-full"
        />
      </div>
    </div>
  );
};

export { Loader };
