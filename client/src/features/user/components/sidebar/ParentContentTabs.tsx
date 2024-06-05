import { FC } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { useUserStore } from '@/features/user/store/userStore';
import { useNavStore } from '@/features/nav';
import { Tooltip } from 'flowbite-react';
import { CurrentTab } from '@/features/user/constants';
import Link from 'next/link';
import { ChevronsLeft, Home, School, User } from 'lucide-react';
import { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';

type ParentContentTabsProps = {
  activeProfile: SchoolProfileResponse;
};

const ParentContentTabs: FC<ParentContentTabsProps> = ({ activeProfile }) => {
  const activeLocale = useLocale();
  const t = useTranslations('UserContentTabs');

  const currentTab = useUserStore((state) => state.currentTab);
  const setSidebarOpen = useNavStore((store) => store.setSidebarOpen);

  return <div>qwerty</div>;
};

export { ParentContentTabs };
