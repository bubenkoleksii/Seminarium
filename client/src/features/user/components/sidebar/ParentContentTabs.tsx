import { FC } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { useUserStore } from '@/features/user/store/userStore';
import { useNavStore } from '@/features/nav';
import { Tooltip } from 'flowbite-react';
import { CurrentTab } from '@/features/user/constants';
import Link from 'next/link';
import { ChevronsLeft, Home, User, Baby } from 'lucide-react';
import { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';

type ParentContentTabsProps = {
  activeProfile: SchoolProfileResponse;
};

const ParentContentTabs: FC<ParentContentTabsProps> = ({ activeProfile }) => {
  const activeLocale = useLocale();
  const t = useTranslations('UserContentTabs');

  const currentTab = useUserStore((state) => state.currentTab);
  const setSidebarOpen = useNavStore((store) => store.setSidebarOpen);

  return (
    <>
      <Tooltip content={t('profile')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/u/`}
          className={`flex h-[50px] w-[50px] items-center justify-center 
         ${currentTab === CurrentTab.Profile ? `bg-purple-950` : `bg-gray-50 hover:bg-gray-200`} 
         text-gray-800 transition duration-300`}
        >
          <User
            color={`${currentTab === CurrentTab.Profile ? `#f9fafb` : `#3B0764`}`}
            size={20}
          />
        </Link>
      </Tooltip>

      <Tooltip content={t('children')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/u/children/${activeProfile.id}`}
          className={`flex h-[50px] w-[50px] items-center justify-center 
         ${currentTab === CurrentTab.Children ? `bg-purple-950` : `bg-gray-50 hover:bg-gray-200`} 
         text-gray-800 transition duration-300`}
        >
          <Baby
            color={`${currentTab === CurrentTab.Children ? `#f9fafb` : `#3B0764`}`}
            size={20}
          />
        </Link>
      </Tooltip>

      <Tooltip content={t('home')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/u/`}
          className={`mt-[50px] flex h-[50px] w-[50px] items-center justify-center bg-gray-50 text-gray-800 
          transition duration-300 hover:bg-gray-200`}
        >
          <Home color={`#3B0764`} size={20} />
        </Link>
      </Tooltip>

      <Tooltip content={t('hide')} placement="right" style="light">
        <div
          onClick={() => setSidebarOpen(false)}
          className={`flex h-[50px] w-[50px] cursor-pointer items-center justify-center bg-gray-50 text-gray-800 
          transition duration-300 hover:bg-gray-200`}
        >
          <ChevronsLeft color={`#3B0764`} size={20} />
        </div>
      </Tooltip>
    </>
  );
};

export { ParentContentTabs };
