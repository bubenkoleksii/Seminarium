import { FC } from 'react';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { Tooltip } from 'flowbite-react';
import { useLocale, useTranslations } from 'next-intl';
import Link from 'next/link';
import { useUserStore } from '@/features/user/store/userStore';
import { useNavStore } from '@/features/nav';
import { CurrentTab } from '@/features/user/constants';
import { User, School, Home, ChevronsLeft, Users, Contact } from 'lucide-react';

interface SchoolAdminContentTabsProps {
  activeProfile: SchoolProfileResponse;
}

const SchoolAdminContentTabs: FC<SchoolAdminContentTabsProps> = ({
  activeProfile,
}) => {
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

      <Tooltip content={t('school')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/u/my-school/${activeProfile.schoolId}`}
          className={`flex h-[50px] w-[50px] items-center justify-center 
         ${currentTab === CurrentTab.School ? `bg-purple-950` : `bg-gray-50 hover:bg-gray-200`} 
         text-gray-800 transition duration-300`}
        >
          <School
            color={`${currentTab === CurrentTab.School ? `#f9fafb` : `#3B0764`}`}
            size={20}
          />
        </Link>
      </Tooltip>

      <Tooltip content={t('groups')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/u/groups`}
          className={`flex h-[50px] w-[50px] items-center justify-center 
         ${currentTab === CurrentTab.Group ? `bg-purple-950` : `bg-gray-50 hover:bg-gray-200`} 
         text-gray-800 transition duration-300`}
        >
          <Users
            color={`${currentTab === CurrentTab.Group ? `#f9fafb` : `#3B0764`}`}
            size={20}
          />
        </Link>
      </Tooltip>

      <Tooltip content={t('schoolProfiles')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/u/school-profile/all/`}
          className={`flex h-[50px] w-[50px] items-center justify-center 
         ${currentTab === CurrentTab.SchoolProfiles ? `bg-purple-950` : `bg-gray-50 hover:bg-gray-200`} 
         text-gray-800 transition duration-300`}
        >
          <Contact
            color={`${currentTab === CurrentTab.SchoolProfiles ? `#f9fafb` : `#3B0764`}`}
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

export { SchoolAdminContentTabs };
