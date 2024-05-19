import { Montserrat } from 'next/font/google';
import { NextIntlClientProvider, useMessages } from 'next-intl';
import { Navbar } from '@/features/nav';
import { Footer } from '@/features/footer';
import Head from 'next/head';
import { Metadata } from 'next';
import { TopLoader } from '@/components/loader';
import { Toaster } from 'react-hot-toast';

const montserrat = Montserrat({
  subsets: ['cyrillic', 'latin'],
  weights: [300, 400, 500, 600, 700],
});

export const metadata: Metadata = {
  title: 'Seminarium',
  description:
    'Seminarium - інноваційна платформа для навчання школярів, що ' +
    'пропонує широкий вибір унікальних навчальних ресурсів та інтерактивних занять.',
  keywords: ['освіта', 'курси', 'навчання', 'школа'],
  contentType: 'website',
  language: 'uk',
  author: 'Seminarium',
};

export default function LocaleLayout({ children, params: { locale } }) {
  const messages = useMessages();

  return (
    <html lang={locale}>
      <Head>
        <link rel="icon" href="/public/favicon.ico" />
        <title>Seminarium</title>
      </Head>

      <body className={montserrat.className} suppressHydrationWarning={true}>
        <TopLoader />

        <NextIntlClientProvider messages={messages}>
          <Toaster position="top-right" />

          <div className="flex min-h-screen flex-col bg-gray-100">
            <Navbar />
            <main className="min-w-screen flex flex-grow justify-center">
              {children}
            </main>
            <Footer />
          </div>
        </NextIntlClientProvider>
      </body>
    </html>
  );
}
