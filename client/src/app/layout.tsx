import type { Metadata } from 'next';
import { Montserrat } from 'next/font/google';

import { Navbar } from "@/features/nav";

import './globals.css';

export const metadata: Metadata = {
  title: 'Seminarium',
  description: ''
};

const montserrat = Montserrat({
  subsets: ['cyrillic', 'latin'],
  weights: [300, 500, 700]
});

export default function RootLayout({ children }: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="uk">
      <body className={montserrat.className}>
        <Navbar />
        {children}
      </body>
    </html>
  );
}
