import { Montserrat } from 'next/font/google';
import Link from 'next/link';

const montserrat = Montserrat({
  subsets: ['cyrillic']
});

export default function NotFoundPage() {
  return (
    <html>
    <body className={montserrat.className}>
      <section className="flex items-center h-screen p-16 bg-gray-50">
        <div className="container flex flex-col items-center ">
          <div className="flex flex-col gap-6 max-w-md text-center">
            <h2 className="font-extrabold text-9xl text-gray-600">
              404
            </h2>
            <p className="text-xl md:text-2xl">Вибачте, ми не змогли знайти цю сторінку.</p>
            <Link href="/"
               className="px-8 py-4 font-semibold rounded bg-purple-800 text-gray-50 hover:text-gray-200">
              Повернутись на головну
            </Link>
          </div>
        </div>
      </section>
    </body>
    </html>
  )
}
