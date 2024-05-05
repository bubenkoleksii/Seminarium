import { FC } from 'react';
import { useTranslations } from 'next-intl';

const Footer: FC = () => {
  const t = useTranslations('Footer');
  return (
    <footer className="bg-gray-200 px-4 py-2 text-xs shadow-md">
      <div>© 2024</div>
      <div>© {t('author')}</div>
    </footer>
  );
};

export { Footer };
