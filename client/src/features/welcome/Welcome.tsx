import { FC } from 'react';
import { useTranslations } from 'next-intl';
import Image from 'next/image';

const Welcome: FC = () => {
  const t = useTranslations('Welcome');

  return (
    <section className="text-gray-700 body-font flex">
      <div className="flex px-5 py-10 lg:flex-row flex-col gap-8 items-center justify-center">
        <div className="lg:max-w-lg lg:w-full md:w-1/2 w-5/6">
          <Image
            src="/welcome/banner.jpg"
            width={720}
            height={600}
            alt={"Баннер"}
            style={{ borderRadius: "10px" }}
          />
        </div>

        <div
          className="lg:order-first lg:flex-grow md:w-1/2 lg:pr-24 md:pr-16 flex flex-col md:items-start md:text-left mb-16 md:mb-0 items-center text-center pl-2">
          <h1 className="title-font sm:text-4xl text-3xl mb-4 font-medium text-gray-900">
            {t('title')}
            <br className="hidden lg:inline-block" />
          </h1>
          <p className="mb-8 leading-relaxed">{t('description')}</p>
          <div className="flex justify-center">
            <button
              className="w-200 inline-flex justify-center items-center text-white bg-purple-900 border-0 py-2 px-6 focus:outline-none hover:bg-purple-700 rounded text-lg">
              {t('registrationBtn')}
            </button>
            <button
              className="w-200 ml-4 inline-flex justify-center items-center text-gray-700 bg-gray-200 border-0 py-2 px-6 focus:outline-none hover:bg-gray-300 rounded text-lg">
              {t('joiningRequestBtn')}
            </button>
          </div>
        </div>
      </div>
    </section>
  );
};

export { Welcome };
