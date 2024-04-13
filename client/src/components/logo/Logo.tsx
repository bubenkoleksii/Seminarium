import { FC } from 'react';
import Image from 'next/image';

interface Props {
  height?: number;
  width?: number;
}

const Logo: FC<Props> = ({ height = 26.75, width = 30.25 }) => {
  return (
    <Image
      className="cursor-pointer"
      src="/logo.png"
      height={height}
      width={width}
      alt="Логотип платформи"
    />
  );
};

export { Logo };
