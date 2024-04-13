import { FC } from 'react';
import { useTranslations } from 'next-intl';

const Footer: FC = () => {
  const t = useTranslations('Footer');
  return (
    <footer className="bg-gray-200 shadow-md text-xs py-2 px-4">
      <div>© 2024</div>
      <div>© {t('author')}</div>
    </footer>
  );
};

export { Footer };
