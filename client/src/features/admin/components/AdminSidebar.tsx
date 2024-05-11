'use client';

import { FC } from 'react';
import { GitPullRequestArrow, User } from 'lucide-react';
import Link from 'next/link';
import { useAdminStore } from '@/features/admin/store/adminStore';
import { AdminClientPaths, CurrentTab } from '../constants';
import { useLocale } from 'next-intl';

const AdminSidebar: FC = () => {
  const activeLocale = useLocale();
  const currentTab = useAdminStore((state) => state.currentTab);

  return (
    <div className="sidebar flex h-screen flex-col items-center justify-start bg-gray-50">
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

      <Link
        href={`/${activeLocale}/${AdminClientPaths.JoiningRequests}/`}
        className={`flex h-[50px] w-[50px] items-center justify-center 
         ${currentTab === CurrentTab.JoiningRequest ? `bg-purple-950` : `bg-gray-50 hover:bg-gray-200`} 
         text-gray-800 transition duration-300`}
      >
        <GitPullRequestArrow
          color={`${currentTab === CurrentTab.JoiningRequest ? `#f9fafb` : `#3B0764`}`}
          size={20}
        />
      </Link>
    </div>
  );
};

export { AdminSidebar };
