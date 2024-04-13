import type { Metadata } from 'next';
import { Montserrat } from 'next/font/google';
import { NextIntlClientProvider, useMessages } from 'next-intl';

import { Navbar } from '@/features/nav';

export const metadata: Metadata = {
  title: 'Seminarium',
  description: '',
};

const montserrat = Montserrat({
  subsets: ['cyrillic', 'latin'],
  weights: [300, 400, 500, 600, 700],
});

export default function LocaleLayout({ children, params: { locale } }) {
  const messages = useMessages();

  return (
    <html lang={locale}>
      <head>
        <link rel="icon" href="/public/favicon.ico" />
        <title>Seminarium</title>
      </head>

      <body className={montserrat.className}>
        <NextIntlClientProvider messages={messages}>
          <Navbar />
          <main className="container mx-auto px-1 pt-5">{children}</main>
        </NextIntlClientProvider>
      </body>
    </html>
  );
}
