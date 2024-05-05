import { FC } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import Image from 'next/image';
import Link from 'next/link';
import { routes } from '@/shared/constants';
import { useSession } from 'next-auth/react';

const Welcome: FC = () => {
  const activeLocale = useLocale();
  const { status } = useSession();
  const t = useTranslations('Welcome');

  return (
      <section className="body-font flex text-gray-700">
        <div className="flex flex-col items-center justify-center gap-8 px-5 py-10 lg:flex-row">
          <div className="w-5/6 md:w-1/2 lg:w-full lg:max-w-lg">
            <Image
              src="/welcome/banner.jpg"
              width={720}
              height={600}
              alt={'Баннер'}
              style={{ borderRadius: '10px' }}
            />
          </div>

          <div className="mb-16 flex flex-col items-center pl-2 text-center md:mb-0 md:w-1/2 md:items-start md:pr-16 md:text-left lg:order-first lg:flex-grow lg:pr-24">
            <h1 className="title-font mb-4 text-3xl font-medium text-gray-900 sm:text-4xl">
              {t('title')}
              <br className="hidden lg:inline-block" />
            </h1>
            <p className="mb-8 leading-relaxed">{t('description')}</p>
            <div className="flex justify-center gap-4">
              {status === 'unauthenticated' &&
                <button className="w-200 inline-flex items-center justify-center rounded border-0 bg-purple-900 px-6 py-2 text-lg text-white hover:bg-purple-700 focus:outline-none">
                  <Link href="http://localhost:5000/Account/Register">
                    {t('registrationBtn')}
                  </Link>
                </button>
              }
              <button className="w-200 inline-flex items-center justify-center rounded border-0 bg-gray-200 px-6 py-2 text-lg text-gray-700 hover:bg-gray-300 focus:outline-none">
                <Link href={routes.getCreateJoiningRequest(activeLocale)}>
                  {t('joiningRequestBtn')}
                </Link>
              </button>
            </div>
          </div>
        </div>
      </section>
  );
};

export { Welcome };
