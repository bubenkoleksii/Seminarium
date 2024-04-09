import { FC } from 'react';
import Image from 'next/image';

interface Props {
  height?: number;
  width?: number;
}

const Logo: FC<Props> = ({ height = 30, width = 30 }) => {
  return (
    <Image
      src="/logo.png"
      height={height}
      width={width}
      alt="Логотип платформи"
    />
  );
};

export { Logo };
