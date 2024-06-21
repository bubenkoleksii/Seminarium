import { useNavStore } from '@/features/nav';
import { CurrentTab } from '@/features/user/constants';
import { useUserStore } from '@/features/user/store/userStore';
import { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { Tooltip } from 'flowbite-react';
import {
  ChevronsLeft,
  Contact,
  Home,
  Megaphone,
  School,
  User,
  Users,
  LibraryBig,
} from 'lucide-react';
import { useLocale, useTranslations } from 'next-intl';
import Link from 'next/link';
import { FC } from 'react';

type ClassTeacherContentTabsProps = {
  activeProfile: SchoolProfileResponse;
};

const ClassTeacherContentTabs: FC<ClassTeacherContentTabsProps> = ({
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

      <Tooltip content={t('courses')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/u/courses/?groupId=${activeProfile.classTeacherGroupId}`}
          className={`flex h-[50px] w-[50px] items-center justify-center 
         ${currentTab === CurrentTab.Course ? `bg-purple-950` : `bg-gray-50 hover:bg-gray-200`} 
         text-gray-800 transition duration-300`}
        >
          <LibraryBig
            color={`${currentTab === CurrentTab.Course ? `#f9fafb` : `#3B0764`}`}
            size={20}
          />
        </Link>
      </Tooltip>

      <Tooltip content={t('school')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/u/my-school/${activeProfile.schoolId || activeProfile.school?.id}`}
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

      <Tooltip content={t('group')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/u/groups/${activeProfile.classTeacherGroupId || activeProfile.classTeacherGroup?.id}`}
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

      <Tooltip content={t('groupNotices')} placement="right" style="light">
        <Link
          href={`/${activeLocale}/u/group-notices/${activeProfile.classTeacherGroupId || activeProfile.classTeacherGroup?.id}`}
          className={`flex h-[50px] w-[50px] items-center justify-center 
         ${currentTab === CurrentTab.GroupNotices ? `bg-purple-950` : `bg-gray-50 hover:bg-gray-200`} 
         text-gray-800 transition duration-300`}
        >
          <Megaphone
            color={`${currentTab === CurrentTab.GroupNotices ? `#f9fafb` : `#3B0764`}`}
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

export { ClassTeacherContentTabs };