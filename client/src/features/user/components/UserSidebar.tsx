'use client';

import { FC } from 'react';
import { useNavStore } from '@/features/nav';
import { useScrollOffset } from '@/shared/hooks';
import { useProfiles } from '@/features/user';
import { SchoolAdminContentTabs } from '@/features/user/components/sidebar/SchoolAdminContentTabs';
import { StudentContentTabs } from '@/features/user/components/sidebar/StudentContentTabs';
import { ParentContentTabs } from '@/features/user/components/sidebar/ParentContentTabs';
import { ClassTeacherContentTabs } from '@/features/user/components/sidebar/ClassTeacherContentTabs';
import { TeacherContentTabs } from '@/features/user/components/sidebar/TeacherContentTabs';

const UserSidebar: FC = () => {
  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const sidebarOpen = useNavStore((store) => store.sidebarOpen);
  const topOffset = useScrollOffset();

  const userContentTabs = {
    school_admin: <SchoolAdminContentTabs activeProfile={activeProfile} />,
    student: <StudentContentTabs activeProfile={activeProfile} />,
    teacher: <TeacherContentTabs activeProfile={activeProfile} />,
    parent: <ParentContentTabs activeProfile={activeProfile} />,
    class_teacher: <ClassTeacherContentTabs activeProfile={activeProfile} />
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
