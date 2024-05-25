'use client';

import { AdminProfile } from '@/features/admin';
import { SessionProvider } from 'next-auth/react';

export default function AdminPage() {
  return (
    <div className="p-3">
      <SessionProvider>
        <AdminProfile />
      </SessionProvider>
    </div>
  );
}
