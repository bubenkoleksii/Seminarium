'use client';

import { FC, Fragment, useState } from 'react';
import Image from 'next/image';
import { useRouter } from 'next/navigation';
import { useLocale } from 'next-intl';
import { Menu, Transition } from '@headlessui/react';
import { ChevronDownIcon } from '@heroicons/react/20/solid';
import { languages } from './languages';

const LanguageDropdown: FC = () => {
  const router = useRouter();
  const activeLocale = useLocale();

  const activeLanguage = languages.find((l) => l.code === activeLocale);
  const [selectedLanguage, setSelectedLanguage] = useState(activeLanguage);

  const handleLanguageSelect = (language) => {
    const nextLocale = language.code;
    router.push(`/${nextLocale}`);
    setSelectedLanguage(language);
  };

  return (
    <Menu as="div" className="relative inline-block cursor-pointer text-left">
      <div>
        <Menu.Button
          className="inline-flex items-center justify-center rounded-md px-2 py-1 text-sm font-semibold
          text-gray-900 hover:bg-gray-200 focus:outline-none"
        >
          <Image
            src={selectedLanguage.flag}
            width={35}
            height={35}
            alt={`${selectedLanguage.name} flag`}
            className="h-5 w-5 rounded-full"
          />
          <ChevronDownIcon
            className="h-4 w-4 text-gray-400"
            aria-hidden="true"
          />
        </Menu.Button>
      </div>

      <Transition
        as={Fragment}
        enter="transition ease-out duration-100"
        enterFrom="transform opacity-0 scale-95"
        enterTo="transform opacity-100 scale-100"
        leave="transition ease-in duration-75"
        leaveFrom="transform opacity-100 scale-100"
        leaveTo="transform opacity-0 scale-95"
      >
        <Menu.Items
          className="absolute right-0 z-10 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1
          ring-black ring-opacity-5 focus:outline-none"
        >
          <div className="py-1">
            {languages.map((language, index) => (
              <Menu.Item key={index}>
                {({ active }) => (
                  <div
                    className={classNames(
                      active ? 'bg-gray-100 text-gray-900' : 'text-gray-700',
                      'flex items-center px-4 py-2 text-sm',
                    )}
                    onClick={() => handleLanguageSelect(language)}
                  >
                    <Image
                      src={language.flag}
                      width={25}
                      height={25}
                      alt={`${language.name} flag`}
                      className="mr-2 h-5 w-5 rounded-full"
                    />
                    {language.name}
                  </div>
                )}
              </Menu.Item>
            ))}
          </div>
        </Menu.Items>
      </Transition>
    </Menu>
  );
};

export { LanguageDropdown };

function classNames(...classes) {
  return classes.filter(Boolean).join(' ');
}
