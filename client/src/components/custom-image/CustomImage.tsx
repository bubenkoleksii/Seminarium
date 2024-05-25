import Image from 'next/image';
import { useState } from 'react';
import clsx from 'clsx';

const   CustomImage = ({ src, width, height, alt = '' }) => {
  const [isLoading, setIsLoading] = useState(true);

  return (
    <div
      className={clsx(
        "relative overflow-hidden",
        { "bg-gray-300 animate-pulse": isLoading },
        "rounded-lg"
      )}
      style={{ width, height }}
    >
      <Image
        loader={() => src}
        src={src}
        alt={alt}
        layout="fill"
        objectFit="cover"
        loading="lazy"
        onLoadingComplete={() => setIsLoading(false)}
        className={clsx({ "opacity-0": isLoading })}
      />
    </div>
  );
};

export { CustomImage };
