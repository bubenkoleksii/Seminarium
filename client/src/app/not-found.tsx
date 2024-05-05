'use client';

import { Montserrat } from 'next/font/google';
import Link from 'next/link';

const montserrat = Montserrat({
  subsets: ['cyrillic'],
});

export default function NotFoundPage() {
  return (
    <html>
      <body className={montserrat.className}>
        <section className="flex h-screen items-center bg-gray-100 bg-gray-50 p-16">
          <div className="container flex flex-col items-center ">
            <div className="flex max-w-md flex-col gap-6 text-center">
              <h2 className="text-9xl font-extrabold text-gray-600">404</h2>
              <p className="text-xl md:text-2xl">
                Вибачте, ми не змогли знайти цю сторінку.
              </p>
              <Link
                href="/"
                className="rounded bg-purple-800 px-8 py-4 font-semibold text-gray-50 hover:text-gray-200"
              >
                Повернутись на головну
              </Link>
            </div>
          </div>
        </section>
      </body>
    </html>
  );
}
