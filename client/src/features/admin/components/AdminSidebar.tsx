'use client';

import { FC } from 'react';
import { GitPullRequestArrow, User } from 'lucide-react';
import Link from 'next/link';
import { useAdminStore } from '@/features/admin/store/adminStore';
import { AdminRoutes, CurrentTab } from '@/features/admin/types';
import { useLocale } from 'next-intl';
import { useRouter } from 'next/navigation';

const AdminSidebar: FC = () => {
  const activeLocale = useLocale();
  const router = useRouter();
  const currentTab = useAdminStore(state => state.currentTab)

  const handle = (route: string) => {
    const routeToRedirect = `/${activeLocale}/${route}/`;

    alert(route)
    router.replace(routeToRedirect);
  };

  return (
    <div className="sidebar flex h-screen w-12 flex-col items-center justify-start bg-gray-50">
      <Link
        href={`/${activeLocale}/${AdminRoutes.Profile}/`}
        className={`flex h-12 w-full items-center justify-center 
         ${currentTab === CurrentTab.Profile ? `bg-purple-950` : `bg-gray-50 hover:bg-gray-200`} 
         text-gray-800 transition duration-300`}
      >
        <User
          color={`${currentTab === CurrentTab.Profile ? `#f9fafb` : `#3B0764`}`}
          size={20}
        />
      </Link>

      <Link
        href={`/${activeLocale}/${AdminRoutes.JoiningRequests}/`}
        className={`flex h-12 w-full items-center justify-center 
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
