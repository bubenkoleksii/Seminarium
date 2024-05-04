import type { Metadata } from 'next';
import { Montserrat } from 'next/font/google';
import { NextIntlClientProvider, useMessages } from 'next-intl';
import { Navbar } from '@/features/nav';
import { Footer } from '@/features/footer';
import { Sidebar } from '@/features/sidebar';

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

      <body className={montserrat.className} suppressHydrationWarning={true}>
        <NextIntlClientProvider messages={messages}>
          <div className="flex flex-col min-h-screen bg-gray-100">
            <Navbar />
            <main className="flex-grow min-w-screen flex justify-center">
              {false && <Sidebar />}
              {children}
            </main>
            <Footer />
          </div>
        </NextIntlClientProvider>
      </body>
    </html>
  );
}

export const metadata: Metadata = {
  title: 'Seminarium',
  description: 'Seminarium - інноваційна платформа для навчання школярів, що ' +
    'пропонує широкий вибір унікальних навчальних ресурсів та інтерактивних занять.',
  keywords: ['освіта', 'курси', 'навчання', 'школа'],
  contentType: 'website',
  language: 'uk',
  author: 'Seminarium'
};