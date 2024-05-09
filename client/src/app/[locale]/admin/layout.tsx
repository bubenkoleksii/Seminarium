import type { PropsWithChildren } from 'react';
import { AdminSidebar } from '@/features/admin';

export default function AdminLayout({ children }: PropsWithChildren) {
  return (
    <div className="flex w-screen">
      <AdminSidebar className="w-1/20" />
      {children}
    </div>
  );
}
