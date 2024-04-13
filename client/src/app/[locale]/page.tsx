'use client'

import { useTranslations } from 'use-intl';

export default function Home() {
  const t = useTranslations('Index');

  return <p className="text-red-900">{t('title')}</p>;
}
