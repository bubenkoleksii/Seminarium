'use client';

import { JoiningRequest } from '@/features/admin';
import { SessionProvider } from 'next-auth/react';

type Props = {
  params: {
    id: string;
  };
};

export default function JoiningRequestPage({ params }: Props) {
  return (
    <div className="p-3">
      <SessionProvider>
        <JoiningRequest id={params.id} />
      </SessionProvider>
    </div>
  );
}
