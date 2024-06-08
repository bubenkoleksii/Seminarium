'use client';

import { FC } from 'react';
import {
  GitPullRequestArrow,
  User,
  Home,
  ChevronsLeft,
  School,
} from 'lucide-react';
import Link from 'next/link';
import { useAdminStore } from '@/features/admin/store/adminStore';
import { AdminClientPaths, CurrentTab } from '../constants';
import { useLocale, useTranslations } from 'next-intl';
import { Tooltip } from 'flowbite-react';
import { useScrollOffset } from '@/shared/hooks';
import { useNavStore } from '@/features/nav';

const AdminSidebar: FC = () => {
  const activeLocale = useLocale();
  const t = useTranslations('AdminContentTabs');

  const currentTab = useAdminStore((state) => state.currentTab);
  const sidebarOpen = useNavStore((store) => store.sidebarOpen);
  const setSidebarOpen = useNavStore((store) => store.setSidebarOpen);

  const topOffset = useScrollOffset();

  return (
    <div
      style={{
        paddingTop: `${topOffset}px`,
        display: !sidebarOpen ? 'none' : 'block',
        transition: 'padding-top 0.7s ease',
      }}
      className={`flex h-screen flex-col items-center justify-start bg-gray-50`}
    >
      <Tooltip content={t('joiningRequest')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/${AdminClientPaths.JoiningRequests}/`}
          className={`flex h-[50px] w-[50px] items-center justify-center 
           ${
             currentTab === CurrentTab.JoiningRequest
               ? `bg-purple-950`
               : `bg-gray-50 hover:bg-gray-200`
           } 
           text-gray-800 transition duration-300`}
        >
          <GitPullRequestArrow
            color={`${currentTab === CurrentTab.JoiningRequest ? `#f9fafb` : `#3B0764`}`}
            size={20}
          />
        </Link>
      </Tooltip>

      <Tooltip content={t('school')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/${AdminClientPaths.Schools}/`}
          className={`flex h-[50px] w-[50px] items-center justify-center 
           ${
             currentTab === CurrentTab.School
               ? `bg-purple-950`
               : `bg-gray-50 hover:bg-gray-200`
           } 
           text-gray-800 transition duration-300`}
        >
          <School
            color={`${currentTab === CurrentTab.School ? `#f9fafb` : `#3B0764`}`}
            size={20}
          />
        </Link>
      </Tooltip>

      <Tooltip content={t('profile')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/${AdminClientPaths.Profile}/`}
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

      <Tooltip content={t('home')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/admin/joining_requests`}
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
    </div>
  );
};

export { AdminSidebar };
