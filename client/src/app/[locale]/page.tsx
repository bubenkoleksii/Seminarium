'use client';

import { Welcome } from '@/features/welcome';
import { SessionProvider } from 'next-auth/react';

export default function HomePage() {
  return (
    <SessionProvider>
      <Welcome />
    </SessionProvider>
  );
}
