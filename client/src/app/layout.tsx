import type { Metadata } from 'next';
import { Montserrat } from 'next/font/google';

import { Navbar } from '@/features/nav';

import './globals.css';

export const metadata: Metadata = {
  title: 'Seminarium',
  description: '',
};

const montserrat = Montserrat({
  subsets: ['cyrillic', 'latin'],
  weights: [300, 400, 500, 600, 700],
});

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="uk">
      <head>
        <link rel="icon" href="/favicon.ico" />
      </head>
      <body className={montserrat.className}>
        <Navbar />
        <main className="container mx-auto px-1 pt-5">
          {children}
        </main>
      </body>
    </html>
  );
}
