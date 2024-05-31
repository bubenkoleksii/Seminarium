'use client';

import { FC } from 'react';
import { useNavStore } from '@/features/nav';
import { useScrollOffset } from '@/shared/hooks';
import { useProfiles } from '@/features/user';
import { SchoolAdminContentTabs } from '@/features/user/components/sidebar/SchoolAdminContentTabs';

const UserSidebar: FC = () => {
  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const sidebarOpen = useNavStore((store) => store.sidebarOpen);
  const topOffset = useScrollOffset();

  const userContentTabs = {
    school_admin: <SchoolAdminContentTabs activeProfile={activeProfile} />,
  };
  const contentTabs = userContentTabs[activeProfile?.type] || null;

  if (profilesLoading || !contentTabs) return null;

  return (
    <div
      style={{
        paddingTop: `${topOffset}px`,
        display: !sidebarOpen ? 'none' : 'block',
        transition: 'padding-top 0.7s ease',
      }}
      className={`flex h-screen flex-col items-center justify-start bg-gray-50`}
    >
      {contentTabs}
    </div>
  );
};

export { UserSidebar };
