'use client';

import { useTranslations } from 'use-intl';

export default function Home() {
  const t = useTranslations('Index');

  const items = Array.from({ length: 100 }, (_, index) => (
    <p key={index} className="text-red-900">
      {t('title')}
    </p>
  ));

  return <div className="text-red-900 h-100">
    {items}
  </div>;
}
