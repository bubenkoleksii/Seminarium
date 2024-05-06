import { FC } from 'react';
import NextTopLoader from 'nextjs-toploader';

const TopLoader: FC = () => {
  return (
    <NextTopLoader
      color="#4a044e"
      initialPosition={0.08}
      crawlSpeed={100}
      height={4}
      crawl={true}
      showSpinner={false}
      easing="ease"
      speed={200}
      shadow="0 0 10px #86198f,0 0 5px #86198f"
      zIndex={1600}
      showAtBottom={false}
    />

  );
};

export { TopLoader };
